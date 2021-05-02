using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : Bird
{
    [SerializeField]
    public float _explosionForce = 100;
    public float _explosionRadius = 4;
    public GameObject _explosionEffect;
    private bool _hasExploded = false;

    public void OnCollisionEnter2D(Collision2D col)
    {
        if(!_hasExploded && (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Obstacle")))
        {
            Explode();
            _hasExploded = true;
        }
    }

    public void Explode()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, -2.5f);
        GameObject duar = Instantiate(_explosionEffect, pos, Quaternion.identity);
        Destroy(duar, 2f);

        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach(Collider2D obj in objects)
        {
            if((obj.gameObject.CompareTag("Enemy") || obj.gameObject.CompareTag("Obstacle")))
            {
                Vector2 dir = obj.transform.position - transform.position;
                obj.GetComponent<Rigidbody2D>().AddForce(dir * _explosionForce);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
