using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float rotateSpeed = 55.0f;
    public Vector2 towerDirection { get { return direction; } }

    Vector2 direction = new Vector2(1, 0);
    float offset;

    private void Start()
    {
        offset = projectilePrefab.GetComponent<Projectile>().offset;
    }

    public void RotateTower(float gunRotate, bool limit)
    {
        if (limit)
        {
            if (gunRotate < 0 && (transform.localEulerAngles.z < 60.0f ||
                    transform.localEulerAngles.z > 295.0f))
            {
                transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            }
            else if (gunRotate > 0 && (transform.localEulerAngles.z < 65.0f ||
                transform.localEulerAngles.z > 300.0f))
            {
                transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (gunRotate < 0)
            {
                transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            }
            else if (gunRotate > 0)
            {
                transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            }
        }

        float rad = -1.0f * transform.eulerAngles.z * Mathf.Deg2Rad;
        float x = Mathf.Sin(rad);
        float y = Mathf.Cos(rad);
        direction.Set(x, y);
        direction.Normalize();
    }

    public void Launch(string launcher)
    {
        Vector3 off = new Vector3(offset * direction.x, offset * direction.y, 0);
        if (launcher == "Player")
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position + off, direction);
            if (hit.collider != null &&
                hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                hit.collider.GetComponent<EnemyController>().IsAttacked(hit.point);
            }
        }

        GameObject projectileObj = Instantiate(projectilePrefab,
                transform.position + off, transform.rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Launch(direction);

        projectileObj.gameObject.layer = LayerMask.NameToLayer(launcher + "Projectile");
    }
}
