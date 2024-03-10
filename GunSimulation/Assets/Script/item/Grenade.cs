using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Grenade : Item
{
    public float explosionRadius = 5f; 
    public float explosionForce = 1000f; 
    public float fuseTime = 3f;
    public float forceOffset; 

    private bool isArmed = false; 
    private bool isExploded = false; 

    public GameObject myParticleSystem;
    private void Start()
    {
       
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            this.transform.parent = null;
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();

            rb.AddForce(transform.forward * forceOffset, ForceMode.Impulse);
        }
        if (isArmed && !isExploded)
        {
            fuseTime -= Time.deltaTime;
            if (fuseTime <= 0)
            {
                Explode();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.gameObject.layer == 8)
        {
            if (!isArmed)
            {
                isArmed = true;
            }
        }
        
    }

    void Explode()
    {
        
        Instantiate(myParticleSystem, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if(collider.gameObject.GetComponentInParent<Character>() != null)
            {
                collider.gameObject.GetComponentInParent<Character>().hit(200);
            }
        }
        isExploded = true;
        Destroy(gameObject); 
    }

}
