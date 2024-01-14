using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("µé¾î¿È");
        Target target = collision.gameObject.GetComponent<Target>();
        if(target != null)
        {
            GameObject e = Instantiate(target.Effect);
            e.transform.position = transform.position;
            e.transform.parent = collision.transform;
        }
        
    }
}
