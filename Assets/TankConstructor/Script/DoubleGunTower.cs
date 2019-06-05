using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleGunTower : TowerController
{
    Vector3 offset1 = new Vector3(0.29f, 3, 0);
    Vector3 offset2 = new Vector3(-0.29f, 3, 0);

    public override void Launch(string launcher)
    {
        Vector3 off1 = Quaternion.AngleAxis(transform.eulerAngles.z, new Vector3(0, 0, 1)) * offset1;
        Vector3 off2 = Quaternion.AngleAxis(transform.eulerAngles.z, new Vector3(0, 0, 1)) * offset2;

        GameObject projectileObj1 = Instantiate(projectilePrefab,
                transform.position + off1, transform.rotation);
        Projectile projectile1 = projectileObj1.GetComponent<Projectile>();
        projectile1.Launch(direction);

        GameObject projectileObj2 = Instantiate(projectilePrefab,
                transform.position + off2, transform.rotation);
        Projectile projectile2 = projectileObj2.GetComponent<Projectile>();
        projectile2.Launch(direction);

        projectileObj1.gameObject.layer = LayerMask.NameToLayer(launcher + "Projectile");
        projectileObj2.gameObject.layer = LayerMask.NameToLayer(launcher + "Projectile");
    }
}
