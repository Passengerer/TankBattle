using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTank : MonoBehaviour
{
    public int maxHealth = 3;
    public int health { get { return currentHealth; } }
    public float speed = 3.5f;
    public float rotateSpeed = 55.0f;
    public GameObject track1;
    public GameObject track2;
    public GameObject tower;
    public ParticleSystem brokenParticle1;
    public ParticleSystem brokenParticle2;
    public ParticleSystem explodeParticle;
    public GameObject AudioListenerObj; //玩家被击败，留下监听器

    protected Vector2 direction = new Vector2(0, 1);
    protected Rigidbody2D rigidbody2d;
    protected int currentHealth;
    protected Animator animator1;
    protected Animator animator2;
    protected TowerController towerCtrl;

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
                rigidbody2d.position, Quaternion.identity);
                if (GetType() == typeof(PlayerController))
                    Instantiate(AudioListenerObj, this.transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;
        }
    }

    protected void TrackAnimation(float horizontal, float vertical)
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

    protected void MovePosition(float horizontal, float vertical)
    {

        if (horizontal < 0)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
        else if (horizontal > 0)
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
    }
}
