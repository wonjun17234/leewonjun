using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector3 offset;
    public ParticleSystem particleLauncher; 
    List<ParticleCollisionEvent> particles;
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
                e.transform.position = particleCollisionEvent.intersection + offset;
                e.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);
                //e.transform.parent = other.transform;
            }
        }

            
    }
}
