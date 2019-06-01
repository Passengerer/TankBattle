using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BasicTank
{
    public float launchTime = 1.5f;
    public float warningTime = 0.95f;
    public GameObject target;
    public float distance = 15.0f;
    public float accuracy = 20;
    public float eludeRate = 0.8f;
    
    float launchTimer = 2.0f;
    float runningTime = 4.5f;
    float runningTimer;
    float towerTimer;
    float towerRotateRate = 0.5f;
    float towerRotateDirection = 0;
    float currentHorizontal;
    float currentVertical;
    bool attacked = false;
    float warningTimer;
    bool seen = false;
    bool elude = false;
    Vector2 attackedPoint;
    bool aiming = false;
    float aimingTimer = 0.5f;
    float randomAim = 0;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator1 = track1.GetComponent<Animator>();
        animator2 = track2.GetComponent<Animator>();
        currentHealth = maxHealth;
        runningTimer = Random.Range(0, runningTime);
        towerTimer = Random.Range(0, 2.0f);
        currentHorizontal = Random.Range(-1.0f, 1.0f);
        currentVertical = Random.Range(-1, 2);
        towerCtrl = tower.GetComponent<TowerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (towerTimer > 0)
        {
            towerTimer -= Time.deltaTime;
        }
        if (runningTimer > 0)
        {
            runningTimer -= Time.deltaTime;
        }
        if (launchTimer > 0)
        {
            launchTimer -= Time.deltaTime;
        }
        else if (launchTimer <= 0 && !aiming)
        {
            launchTimer = Random.Range(launchTime, 6.0f);
            Launch();
        }
        else
        {
            launchTimer = Random.Range(launchTime, 2.5f);
            Launch();
        }
        if (warningTimer > 0)
        {
            warningTimer -= Time.deltaTime;
        }
        else if (attacked)
        {
            attacked = false;
        }
        CheckState();
        if (target != null)
            CheckDistance();
    }

    void CheckDistance()
    {
        if (Vector2.Distance(target.transform.position, transform.position) < distance)
        {
            RaycastHit2D hit = Physics2D.Linecast(
                transform.position, target.transform.position, LayerMask.GetMask("Wall"));
            if (hit.collider != null) {
                seen = false;
            }
            else seen = true;
        }
        else seen = false;
    }

    void CheckState()
    {
        if (!attacked)
        {
            //空闲状态，划水
            if (runningTimer <= 0)
            {
                runningTimer = Random.Range(0, runningTime);

                currentHorizontal = Random.Range(-1.0f, 1.0f);
                if (Mathf.Abs(currentHorizontal) > 0.5f)
                    currentHorizontal = 0;
                currentVertical = Random.Range(-1, 2);
            }
            MovePosition(currentHorizontal, currentVertical);
        }
        else
        {
            if (attacked && elude)   //靶子状态，一定几率躲避
            {
                elude = false;
                float rate = Random.Range(0, 1.0f);
                if (rate < eludeRate)
                {
                    Vector2 vector = rigidbody2d.position - attackedPoint;
                    float dot = Vector2.Dot(vector, direction);
                    float angle = Vector2.Angle(vector, direction);
                    if (dot > 0)
                    {
                        currentVertical = 1;
                    }
                    else
                    {
                        currentVertical = -1;
                    }
                    //判断躲避方向是否有障碍物
                    RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position,
                        direction * currentVertical, 3, LayerMask.GetMask("Wall"));
                    //如果夹角太小，直走不能躲避，必须转弯
                    if (angle < 30.0f || angle > 150.0f || hit.collider != null)
                    {
                        currentHorizontal = Random.Range(0, 2) == 0 ? 1 : -1;
                    }
                    else
                    {
                        currentHorizontal = Random.Range(-1.0f, 1.0f);
                        if (Mathf.Abs(currentHorizontal) > 0.6f)
                            currentHorizontal = 0;
                    }
                }
            }
            MovePosition(currentHorizontal, currentVertical);
        }
        if (!seen)
        {
            if (towerTimer > 0)
                towerCtrl.RotateTower(towerRotateDirection, false);
            else
            {
                towerTimer = Random.Range(0, 2.0f);
                towerRotateDirection = Random.Range(-1.0f, 1.0f);
                if (Mathf.Abs(towerRotateDirection) < towerRotateRate)
                    towerRotateDirection = 0;
            }
        }
        else if (seen && target != null)
        {
            Vector2 targetDirection = target.transform.position - towerCtrl.transform.position;
            float angle = Vector2.SignedAngle(targetDirection, towerCtrl.towerDirection);
            if (aimingTimer > 0)
            {
                aimingTimer -= Time.deltaTime;
            }
            else
            {
                aimingTimer = Random.Range(0, 1.0f);
                randomAim = Random.Range(-accuracy, accuracy);
            }
            if (angle < randomAim + 3.0f && angle > randomAim - 3.0f)
            {
                aiming = true;
                towerRotateDirection = 0;
            }
            else
            {
                aiming = false;
                towerRotateDirection = randomAim - angle < 0 ? 1 : -1;
            }
            towerCtrl.RotateTower(towerRotateDirection, false);
        }
        if (target == null)
            seen = false;
    }

    void Launch()
    {
        towerCtrl.Launch("enemy");
    }

    public void IsAttacked(Vector2 location)
    {
        attacked = true;
        warningTimer = warningTime;
        elude = true;
        attackedPoint = location;
    }
}
