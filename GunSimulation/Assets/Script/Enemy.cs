using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent nma;
    public Animator anim;
    public GameObject player;
    private float time = 0;
    public int state;
    public int hp = 100;

    public Vector3 positoin;

    public GameObject weapon;
    public float radius;
    private RaycastHit rayHit;
    private RaycastHit forwardHit;
    private RaycastHit[] sphereHit;
    private float dist = 40;


    void Start()
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();
            Debug.LogWarning("anim가 설정이 안돼있음");
        }
        
        if (nma ==null)
        {
            nma = GetComponent<NavMeshAgent>();
            Debug.LogWarning("nma가 설정이 안돼있음");
        }
        nma.destination = transform.position + transform.forward * 5f;
        state = 0;
        
    }
    void FixedUpdate()
    {
        if(player == null)
        {
            state = 0;
        }
        int layerMask = 1 << LayerMask.NameToLayer("Enviroment");
        sphereHit = Physics.SphereCastAll(transform.position, radius, transform.up, 0, layerMask);
        
        
        if (state == 0 && Physics.SphereCast(transform.position, radius / 3, transform.forward, out forwardHit, dist))
        {
            if (GameManager.instance.teams.Contains(forwardHit.transform.tag) && (transform.tag != forwardHit.transform.tag))
            {
                player = forwardHit.transform.gameObject;

                state = 1;
                time = 0;
            }
        }

        if (transform.position.x != nma.destination.x && transform.position.z != nma.destination.z)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }

    }
    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    void Update()
    {
        time += Time.deltaTime;        
    }

    void LateUpdate()
    {
        switch (state)
        {
            case 0:
                if (time >= 1.5)
                {
                    time = 0;   
                    int temp = Random.Range(0, 3);
                    switch (temp)
                    {
                        case 0:
                            nma.destination = transform.position + transform.forward * 5f;
                            break;
                        case 1:
                            nma.destination = transform.position + transform.right * 5f;
                            break;
                        case 2:
                            nma.destination = transform.position - transform.right * 5f;
                            break;
                    }
                }
                break;
            case 1:
                if(sphereHit.Length == 0 || time > 3f)
                {
                    positoin = transform.position;
                    nma.destination = positoin;
                    state = 2;
                    nma.speed = 1.5f;
                    
                }
                else
                {
                    float minDistance = Vector3.Distance(transform.position, sphereHit[0].transform.position);
                    int minIndex = 0;
                    for(int i =1; i < sphereHit.Length; i++)
                    {
                        float temp = Vector3.Distance(transform.position, sphereHit[i].transform.position);
                        if (minDistance > temp)
                        {
                            minDistance = temp;
                            minIndex = i;
                        }
                    }
                    nma.destination = player.transform.position - (player.transform.position - sphereHit[minIndex].transform.position) * 1.3f;
                    nma.speed = 6;
                }
                break;
            case 2:
                transform.LookAt(player.transform.position);
                if (time >= 0.5f)
                {
                    time = 0;
                    if (Physics.Raycast(transform.position, transform.forward, out rayHit, dist)
                    && GameManager.instance.teams.Contains(rayHit.transform.tag)
                    && (transform.tag != rayHit.transform.tag))
                    {
                        nma.destination = transform.position;
                        if(weapon.GetComponent<PlayerWeapon>().enemyShot())
                        {
                            int temp = Random.Range(0, 2);
                            if(temp == 0)
                            {
                                nma.destination = transform.position + transform.right * 0.5f;
                            }
                            else
                            {
                                nma.destination = transform.position - transform.right * 0.5f;
                            }
                        }
                        else
                        {
                            state = 1;
                        }  
                    }
                    else
                    {
                        nma.speed = 3;
                        int temp = Random.Range(0, 2);
                        if (temp == 0)
                        {
                            nma.destination = transform.position + transform.right * 2f;
                        }
                        else
                        {
                            nma.destination = transform.position - transform.right * 2f;
                        }
                    }
                }
                break;
        }
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
