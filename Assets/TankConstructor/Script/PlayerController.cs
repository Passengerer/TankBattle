using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BasicTank
{
    public GameObject plate;
    public float plateTime = 10.0f;
    public float launchTime = 1.0f;
    
    float plateTimer;
    float launchTimer;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator1 = track1.GetComponent<Animator>();
        animator2 = track2.GetComponent<Animator>();
        currentHealth = maxHealth;
        SetPlate();
        launchTimer = launchTime;
        towerCtrl = tower.GetComponent<TowerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float gunRotate = Input.GetAxis("GunRotate");

        MovePosition(horizontal, vertical);
        towerCtrl.RotateTower(gunRotate, true);

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
        towerCtrl.Launch("Player");
    }

    public void SetPlate()
    {
        plate.SetActive(true);
        plateTimer = plateTime;
    }
}

