using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float offset;
    public ParticleSystem particleLauncher; 
    List<ParticleCollisionEvent> particles;
    public GameObject parent;

    // Start is called before the first frame update
    private void Start()
    {
        particles = new List<ParticleCollisionEvent>();
    }
    private void OnParticleCollision(GameObject other)
    {

        if (other.GetComponent<Target>() != null)
        {
            ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, particles);
            GameObject e = Instantiate(other.GetComponent<Target>().Effect);
            foreach (ParticleCollisionEvent particleCollisionEvent in particles)
            {
                e.transform.position = particleCollisionEvent.intersection + transform.forward * offset;
                e.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);
                //e.transform.parent = other.transform;
            }
        }
        
        if(other.GetComponent<Enemy>() != null && other != parent)
        {
            Debug.Log("들어옴");
            other.GetComponent<Enemy>().hp -= 20;
        }
 
    }
}
