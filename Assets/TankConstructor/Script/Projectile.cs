using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float force = 800.0f;
    public ParticleSystem explodeEffect;

    Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
            return;

        ParticleSystem explode = Instantiate(explodeEffect,
            rigidbody2d.position, Quaternion.identity);

        if (collision.gameObject.layer == 11)
        {
            Destroy(gameObject);
            return;
        }
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }

        EnemyController enemy = collision.GetComponent<EnemyController>();
        if (enemy != null && gameObject.layer != 12)
        {
            enemy.ChangeHealth(-1);
        }

        Destroy(gameObject);
    }

    public void Launch(Vector2 direction)
    {
        rigidbody2d.AddForce(direction * force);
    }
}
