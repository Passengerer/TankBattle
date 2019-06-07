using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BasicTank
{
    public GameObject plate;
    public GameObject AudioListenerObject;  //子物体对象，保持监听器不旋转
    public float plateTime = 10.0f;
    public float launchTime = 1.0f;
    public bool movable = true;
    
    float plateTimer;
    float launchTimer;
    AudioSource audioSource;
    float staticTime = 0;
    float fixTime = 5.0f;
    float fixTimer;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator1 = track1.GetComponent<Animator>();
        animator2 = track2.GetComponent<Animator>();
        currentHealth = maxHealth;
        launchTimer = launchTime;
        towerCtrl = tower.GetComponent<TowerController>();
        audioSource = GetComponent<AudioSource>();
        SetPlate();
        fixTimer = fixTime;
    }

    void Fix()
    {
        if (fixTimer > 0)
        {
            fixTimer -= Time.deltaTime;
        }
        else
        {
            ChangeHealth(1);
            fixTimer = fixTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!movable)
        {
            if (staticTime > 0)
            {
                staticTime -= Time.deltaTime;
                return;
            }
            else SetMovable(true, -1);
        }

        AudioListenerObject.transform.eulerAngles = new Vector3(0, 0, 0);

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
        if (plate.activeInHierarchy)
            Fix();
    }

    public void SetMovable(bool movable, float time)
    {
        this.movable = movable;
        staticTime = time;
    }

    void Launch()
    {
        towerCtrl.Launch("Player");
    }

    public void SetPlate()
    {
        audioSource.Play();
        plate.SetActive(true);
        plateTimer = plateTime;
        fixTimer = fixTime;
    }
}
