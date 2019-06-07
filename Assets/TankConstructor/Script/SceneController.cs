using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject player;

    float timer = 2.0f;

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        Object[] enemys = GameObject.FindObjectsOfType(typeof(EnemyController));
        if (enemys.Length == 0)
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
    }
}
