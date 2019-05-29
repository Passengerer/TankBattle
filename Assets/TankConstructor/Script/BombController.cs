using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public ParticleSystem explode;
    public int damage = -2;
    public float explodeRadius = 2.5f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.GetComponent<Projectile>();

        if (projectile != null)
        {
            Instantiate(explode, transform.position, Quaternion.identity);
            Bomb();
            Destroy(gameObject);
        }
    }

    void Bomb()
    {
        Collider2D[] others = Physics2D.OverlapCircleAll(
            transform.position, explodeRadius);
        for (int i = 0; i < others.Length; ++i)
        {
            if (others[i].gameObject.layer == 9)
            {
                others[i].GetComponent<PlayerController>().ChangeHealth(damage);
            }
            else if (others[i].gameObject.layer == 10)
            {
                others[i].GetComponent<EnemyController>().ChangeHealth(damage);
            }
        }
    }
}
