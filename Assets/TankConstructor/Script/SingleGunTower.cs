using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGunTower : TowerController
{
    public float offset = 2.1f;

    public override void Launch(string launcher)
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
