using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticProjectile : Projectile
{
    public float explodeRadius = 1.5f;
    public float staticTime = 1.5f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        ParticleSystem explode = Instantiate(explodeEffect,
            rigidbody2d.position, Quaternion.identity);

        Collider2D player = Physics2D.OverlapCircle(
            transform.position, explodeRadius, LayerMask.GetMask("Players"));
        if (player != null)
        {
            player.GetComponent<PlayerController>().SetMovable(false, staticTime);
        }

        Destroy(gameObject);
    }
}
