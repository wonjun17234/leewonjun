using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        print("hit" + other.name + "!");
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.enemyDistroy();
            Destroy(gameObject);
        }

    }
}
