using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float offset = 1.9f;
    public float force = 800.0f;
    public ParticleSystem explodeEffect;

    Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        ParticleSystem explode = Instantiate(explodeEffect,
            rigidbody2d.position, Quaternion.identity);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Plate"))
        {
            Destroy(gameObject);
        }
        else
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && gameObject.layer != LayerMask.NameToLayer("PlayerProjectile"))
            {
                player.ChangeHealth(-1);
            }

            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null && gameObject.layer != LayerMask.NameToLayer("EnemyProjectile"))
            {
                enemy.ChangeHealth(-1);
            }

            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction)
    {
        rigidbody2d.AddForce(direction * force);
    }
}
