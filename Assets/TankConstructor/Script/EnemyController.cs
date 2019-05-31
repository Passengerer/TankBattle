using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BasicTank
{
    public GameObject projectilePrefab;
    public float speed = 3.0f;
    public float launchTime = 1.5f;
    public GameObject track1;
    public GameObject track2;
    public float warningTime = 0.95f;
    public GameObject target;
    public float distance = 15.0f;
    
    float time = 2.0f;
    Vector2 direction = new Vector2(0, -1);
    float runningTime;
    float currentHorizontal;
    float currentVertical;
    float rotateSpeed = 55.0f;
    bool attacked = false;
    float warningTimer;
    bool seen = false;
    bool elude = false;
    float eludeRate = 0.8f;
    Vector2 attackedPoint;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3;
        rigidbody2d = GetComponent<Rigidbody2D>();
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
            time = Random.Range(launchTime, 6.0f);
            Launch();
        }
        if (runningTime > 0)
        {
            runningTime -= Time.deltaTime;
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
            if (runningTime <= 0)
            {
                runningTime = Random.Range(0, 3.0f);

                currentHorizontal = Random.Range(-1.0f, 1.0f);
                if (Mathf.Abs(currentHorizontal) > 0.5f)
                    currentHorizontal = 0;
                currentVertical = Random.Range(-1, 2);
            }
            MovePosition();
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
                    if (angle < 30.0f || hit.collider != null)
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
            MovePosition();
        }
    }

    void MovePosition()
    {
        
        if (currentHorizontal < 0)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
        else if (currentHorizontal > 0)
        {
            transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
        }

        float rad = -1.0f * transform.eulerAngles.z * Mathf.Deg2Rad;
        float x = Mathf.Sin(rad);
        float y = Mathf.Cos(rad);
        direction.Set(x, y);

        Vector2 position = rigidbody2d.position;
        position += currentVertical * direction * speed * Time.deltaTime;

        TrackAnimation(currentHorizontal, currentVertical);
        rigidbody2d.MovePosition(position);
    }

    void Launch()
    {
        Vector2 offset = new Vector2(1.9f * direction.x, 1.9f * direction.y);
        GameObject projectileObj = Instantiate(projectilePrefab,
                rigidbody2d.position + offset, transform.rotation);
        projectileObj.layer = 12;
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Launch(direction);
    }

    public void IsAttacked(Vector2 location)
    {
        attacked = true;
        warningTimer = warningTime;
        elude = true;
        attackedPoint = location;
    }
}
