using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTank : MonoBehaviour
{
    public int maxHealth = 3;
    public int health { get { return currentHealth; } }
    public ParticleSystem brokenParticle1;
    public ParticleSystem brokenParticle2;
    public ParticleSystem explodeParticle;

    protected Rigidbody2D rigidbody2d;
    protected int currentHealth;
    protected Animator animator1;
    protected Animator animator2;

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
}
