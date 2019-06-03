using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{
    public float delayTime = 1.0f;
    public ParticleSystem explode;
    public float explodeRadius = 1.5f;
    public int damage = -1;

    float timer = 0;
    Animator animator;
    bool triggered = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer < 0)
        {
            Bomb();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered)
        {
            animator.SetBool("Trigger", true);
            GetComponent<AudioSource>().Play();
            timer = delayTime;
            triggered = true;
        }
    }

    void Bomb()
    {
        Collider2D[] others = Physics2D.OverlapCircleAll(
            transform.position, explodeRadius);
        for (int i = 0; i < others.Length; ++i)
        {
            if (others[i].gameObject.layer == LayerMask.NameToLayer("Players"))
            {
                others[i].GetComponent<PlayerController>().ChangeHealth(damage);
            }
            else if(others[i].gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                others[i].GetComponent<EnemyController>().ChangeHealth(damage);
            }
        }
        Instantiate(explode, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
