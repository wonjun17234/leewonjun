using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Target>() != null)
        {
            
            GameObject e = Instantiate(collision.gameObject.GetComponent<Target>().Effect);
            e.transform.position = transform.position;
            e.transform.parent = collision.transform;
            
        }
        
    }
}
