using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject projectilePrefab;

    Vector2 direction;
    float rotateSpeed = 55.0f;

    void Start()
    {
        direction = new Vector2(1, 0);
    }

    void Update()
    {
        float gunRotate = Input.GetAxis("GunRotate");

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

        float rad = -1.0f * transform.eulerAngles.z * Mathf.Deg2Rad;
        float x = Mathf.Sin(rad);
        float y = Mathf.Cos(rad);
        direction.Set(x, y);
    }

    public void Launch()
    {
        Vector3 offset = new Vector3(1.9f * direction.x, 1.9f * direction.y, 0);
        GameObject projectileObj = Instantiate(projectilePrefab,
                transform.position + offset, transform.rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Launch(direction);
    }
}
