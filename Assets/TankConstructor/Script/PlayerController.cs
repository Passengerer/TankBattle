using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 3;
    public int health { get { return currentHealth; } }
    public float speed = 3.5f;
    public GameObject track1;
    public GameObject track2;
    public GameObject projectilePrefab;
    public ParticleSystem brokenParticle;

    Rigidbody2D rigidbody2d;
    Animator animator1;
    Animator animator2;
    Vector2 direction = new Vector2(0, 1);
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator1 = track1.GetComponent<Animator>();
        animator2 = track2.GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal < 0)
        {
            transform.Rotate(0, 0, 1);
        }else if (horizontal > 0)
        {
            transform.Rotate(0, 0, -1);
        }

        float rad = -1.0f * transform.eulerAngles.z * Mathf.Deg2Rad;
        float x = Mathf.Sin(rad);
        float y = Mathf.Cos(rad);
        direction.Set(x, y);

        Vector2 position = rigidbody2d.position;
        position += vertical * direction * speed * Time.deltaTime;

        TrackAnimation(horizontal, vertical);
        rigidbody2d.MovePosition(position);

        if (Input.GetKeyDown(KeyCode.J))
        {
            Launch();
        }
    }

    void TrackAnimation(float horizontal, float vertical)
    {
        if (Mathf.Approximately(horizontal, 0) && Mathf.Approximately(vertical, 0))
        {
            animator1.SetBool("Running", false);
            animator2.SetBool("Running", false);
        }
        else
        {
            animator1.SetBool("Running", true);
            animator2.SetBool("Running", true);
        }
    }

    void Launch()
    {
        Vector2 offset = new Vector2(1.9f * direction.x, 1.9f * direction.y);
        GameObject projectileObj = Instantiate(projectilePrefab,
                rigidbody2d.position + offset, transform.rotation);
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

