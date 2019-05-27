using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 2;
    public int health { get { return currentHealth; } }
    public GameObject projectilePrefab;
    public float speed = 3.0f;
    public ParticleSystem brokenParticle;

    Rigidbody2D body;
    float time = 2.0f;
    Vector2 direction = new Vector2(0, 1);
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time = 2.0f;
            Launch();
        }
    }

    void Launch()
    {
        Vector2 offset = new Vector2(1.9f * direction.x, 1.9f * direction.y);
        GameObject projectileObj = Instantiate(projectilePrefab,
                body.position + offset, transform.rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Launch(direction);
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth < maxHealth)
        {
            brokenParticle.Play();
        }
        else
        {
            brokenParticle.Stop();
        }
    }
}
