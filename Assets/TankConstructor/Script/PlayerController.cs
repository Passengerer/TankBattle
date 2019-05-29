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
    public GameObject tower;
    //public GameObject projectilePrefab;
    public ParticleSystem brokenParticle1;
    public ParticleSystem brokenParticle2;
    public ParticleSystem explodeParticle;
    public GameObject plate;
    public float plateTime = 6.0f;
    public float launchTime = 1.0f;

    Rigidbody2D rigidbody2d;
    Animator animator1;
    Animator animator2;
    Vector2 direction = new Vector2(0, 1);
    int currentHealth;
    float plateTimer;
    float launchTimer;
    TowerController towerCtrl;
    float rotateSpeed = 55.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator1 = track1.GetComponent<Animator>();
        animator2 = track2.GetComponent<Animator>();
        currentHealth = maxHealth;
        plateTimer = plateTime;
        launchTimer = launchTime;
        towerCtrl = tower.GetComponent<TowerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal < 0)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }else if (horizontal > 0)
        {
            transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
        }

        float rad = -1.0f * transform.eulerAngles.z * Mathf.Deg2Rad;
        float x = Mathf.Sin(rad);
        float y = Mathf.Cos(rad);
        direction.Set(x, y);

        Vector2 position = rigidbody2d.position;
        position += vertical * direction * speed * Time.deltaTime;

        TrackAnimation(horizontal, vertical);
        rigidbody2d.MovePosition(position);

        if (launchTimer > 0)
        {
            launchTimer -= Time.deltaTime;
        }
        if (launchTimer <=0 && Input.GetKeyDown(KeyCode.J))
        {
            Launch();
            launchTimer = launchTime;
        }

        if (plateTimer > 0)
        {
            plateTimer -= Time.deltaTime;
        }
        else
        {
            plate.SetActive(false) ;
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
        towerCtrl.Launch();
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        switch(maxHealth - currentHealth)
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
                rigidbody2d.position, Quaternion.identity);
                Destroy(gameObject);
                break;
        }
    }

    public void SetPlate()
    {
        plate.SetActive(true);
        plateTimer = plateTime;
    }
}

