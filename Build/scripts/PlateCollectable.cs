using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCollectable : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.SetPlate();
            Destroy(gameObject);
        }
    }
}
