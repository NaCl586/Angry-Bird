using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float Health = 50f;
    public GameObject puffEffect;

    public UnityAction<GameObject> OnEnemyDestroyed = delegate { };

    private bool _isHit = false;

    void OnDestroy()
    {
        if (_isHit)
        {
            OnEnemyDestroyed(gameObject);
        }
    }

    public void outOfBounds()
    {
        _isHit = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;

        if (col.gameObject.CompareTag("Bird"))
        {
            killEnemy();
        }
        else if (col.gameObject.CompareTag("Obstacle"))
        {
            //Hitung damage yang diperoleh
            float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            Health -= damage;

            if (Health <= 0)
            {
                killEnemy();
            }
        }
    }

    void killEnemy()
    {
        _isHit = true;

        Vector3 pos = new Vector3(transform.position.x, transform.position.y, -2.5f);
        GameObject duar = Instantiate(puffEffect, pos, Quaternion.identity);
        Destroy(duar, 2f);

        Destroy(gameObject);
    }
}