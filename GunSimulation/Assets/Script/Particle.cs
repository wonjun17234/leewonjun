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
        if (other.CompareTag("Cube") )   
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
        else if(!other.CompareTag(parent.tag))
        {
            ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, particles);
            for(int i = 0; i < particles.Count; i++)
            {
                string[] p = other.name.Split(":");
                GameObject targget = other.GetComponentInParent<Character>().gameObject;


                if (p[1].Equals("Head"))
                {
                    targget.GetComponent<Character>().hit(50);
                }
                else if (p[1].Equals("Spine2"))
                {
                    targget.GetComponent<Character>().hit(30);
                }
                else
                {
                    targget.GetComponent<Character>().hit(15);
                }

                if(targget.GetComponentInParent<Enemy>())
                {
                    targget.GetComponentInParent<Enemy>().LookPlayer(parent);
                }
            }
        }
    }
}
