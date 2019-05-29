using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 3;
    public int health { get { return currentHealth; } }
    public GameObject projectilePrefab;
    public float speed = 3.0f;
    public ParticleSystem brokenParticle1;
    public ParticleSystem brokenParticle2;
    public ParticleSystem explodeParticle;
    public GameObject track1;
    public GameObject track2;

    Rigidbody2D body;
    float time = 2.0f;
    Vector2 direction = new Vector2(0, -1);
    Animator animator1;
    Animator animator2;
    int currentHealth;
    float runningTime;
    float currentHorizontal;
    float currentVertical;
    float rotateSpeed = 55.0f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator1 = track1.GetComponent<Animator>();
        animator2 = track2.GetComponent<Animator>();
        currentHealth = maxHealth;
        runningTime = Random.Range(0, 5.0f);
        currentHorizontal = Random.Range(-1.0f, 1.0f);
        currentVertical = Random.Range(-1, 2);
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
        MovePosition();
    }

    void MovePosition()
    {
        if (runningTime > 0)
        {
            runningTime -= Time.deltaTime;
        }
        else
        {
            runningTime = Random.Range(0, 5.0f);

            currentHorizontal = Random.Range(-1.0f, 1.0f);
            currentVertical = Random.Range(-1, 2);
        }

        if (currentHorizontal < -0.6f)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
        else if (currentHorizontal > 0.6f)
        {
            transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
        }

        float rad = -1.0f * transform.eulerAngles.z * Mathf.Deg2Rad;
        float x = Mathf.Sin(rad);
        float y = Mathf.Cos(rad);
        direction.Set(x, y);

        Vector2 position = body.position;
        position += currentVertical * direction * speed * Time.deltaTime;

        TrackAnimation(currentHorizontal, currentVertical);
        body.MovePosition(position);
    }

    void TrackAnimation(float horizontal, float vertical)
    {
        if ((horizontal > -0.6f && horizontal < 0.6f) && Mathf.Approximately(vertical, 0))
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
                body.position + offset, transform.rotation);
        projectileObj.layer = 10;
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Launch(direction);
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        switch (maxHealth - currentHealth)
        {
            case 0:
                brokenParticle1.Stop();
                brokenParticle2.Stop();
                break;
            case 1:
                brokenParticle1.Play();
                brokenParticle2.Stop();
                break;
            case 2:
                brokenParticle2.Play();
                brokenParticle1.Stop();
                break;
            default:
                ParticleSystem explode = Instantiate(explodeParticle,
                body.position, Quaternion.identity);
                Destroy(gameObject);
                break;
        }
    }
}
