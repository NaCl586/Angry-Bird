using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public TrailController TrailController;
    public List<Bird> Birds;
    public List<Enemy> Enemies;
    private Bird _shotBird;
    public BoxCollider2D TapCollider;
    public Text status;

    private bool _isGameEnded = false;
    private bool _win = false;

    private void Update()
    {
        if (Enemies.Count == 0)
        {
            _win = true;
            status.text = "You Win!!\nPress [R] to go to the next level";
            _isGameEnded = true;
        }

        if (_isGameEnded && Input.GetKey(KeyCode.R))
        {
            if(_win)
            {
                if (SceneManager.GetActiveScene().name == "Level 1") SceneManager.LoadScene("Level 2");
                else if (SceneManager.GetActiveScene().name == "Level 2") SceneManager.LoadScene("Level 1");
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void Start()
    {
        status.text = "";
        for (int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        SlingShooter.InitiateBird(Birds[0]);
        TapCollider.enabled = false;
        _shotBird = Birds[0];
    }

    public void ChangeBird()
    {
        TapCollider.enabled = false; //Line ini selalu bermasalah karena tiap restart/next level selalu dikira ke destroy makanya null, dan ini jadinya null reference error. To be honest, saya gak tau ini kenapa dan gimana. Tepatnya, saya gak ketemu alasan kenapa ini boxcollider2d selalu bisa dikira ke destroy/null (tapi gameplay wise ga ada masalah sih)

        if (_isGameEnded)
        {
            return;
        }

        Birds.RemoveAt(0);

        if (Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
        }
        else
        {
            _isGameEnded = true;
            if (Enemies.Count == 0)
            {
                _win = true;
                status.text = "You Win!!\nPress [R] to go to the next level";
            }
            else
            {
                _win = false;
                status.text = "You Lose!!\nPress [R] to restart the level";
            }
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].gameObject == destroyedEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }
        if (Enemies.Count == 0)
        {
            _win = true;
            status.text = "You Win!!\nPress[R] to go to the next level";
            _isGameEnded = true;
        }
    }
    public void AssignTrail(Bird bird)
    {
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        TapCollider.enabled = true;
    }
    void OnMouseUp()
    {
        if (_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }
}
