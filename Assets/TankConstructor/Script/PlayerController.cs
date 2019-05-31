using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BasicTank
{
    public float speed = 3.5f;
    public GameObject track1;
    public GameObject track2;
    public GameObject tower;
    public GameObject plate;
    public float plateTime = 6.0f;
    public float launchTime = 1.0f;
    
    Vector2 direction = new Vector2(0, 1);
    float plateTimer;
    float launchTimer;
    TowerController towerCtrl;
    float rotateSpeed = 55.0f;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3;
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

    void Launch()
    {
        towerCtrl.Launch();
    }

    public void SetPlate()
    {
        plate.SetActive(true);
        plateTimer = plateTime;
    }
}

