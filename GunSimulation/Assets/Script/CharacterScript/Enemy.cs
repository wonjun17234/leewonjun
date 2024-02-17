using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Drawing;

public class Enemy : Character
{

    public NavMeshAgent nma;
    public Animator anim;
    public GameObject player = null;
    private float time = 0;
    public int state;
    

    public Vector3 positoin;

    public GameObject weapon;
    public float radius;
    private RaycastHit rayHit;
    private RaycastHit forwardHit;
    private RaycastHit[] sphereHit;
    private float dist = 80;

    

    public float XSpeed;
    public float ZSpeed;
    void Start()
    {
        
        if (anim == null)
        {
            anim = GetComponent<Animator>();
            Debug.LogWarning("anim가 설정이 안돼있음");
        }
        
        if (nma ==null)
        {
            nma = GetComponent<NavMeshAgent>();
            Debug.LogWarning("nma가 설정이 안돼있음");
        }
        nma.speed = 1.7f;
        state = 0;
        
    }
    void FixedUpdate()
    {

        int layerMask = 1 << LayerMask.NameToLayer("Enviroment");
        sphereHit = Physics.SphereCastAll(transform.position, radius, transform.up, 0, layerMask);


        layerMask = (1 << LayerMask.NameToLayer("Enviroment")) + (1 << LayerMask.NameToLayer("Player"));
        if (Physics.Raycast(weapon.transform.position + weapon.transform.up * 0.05f, weapon.transform.forward, out forwardHit, 10f, layerMask) && state == 0)
        {
            if (forwardHit.transform.GetComponentInParent<Character>()
                && (transform.tag != forwardHit.transform.GetComponentInParent<Character>().tag))
            {
                Character charHit = forwardHit.transform.GetComponentInParent<Character>();
                player = charHit.gameObject;
                //Debug.Log(player.name);
                state = 1;
                time = 0;
            }
        }

        if (Mathf.Ceil(transform.position.x) != Mathf.Ceil(nma.destination.x) || Mathf.Ceil(transform.position.z) != Mathf.Ceil(nma.destination.z))
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);

        }

        if (state != 0 && player.GetComponent<Character>().hp <= 0)
        {
            player = null;
            state = 0;
        }

    }
    

    /*void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }*/
    void Update()
    {
        time += Time.deltaTime;
        anim.SetFloat("speed", nma.velocity.magnitude);

        Debug.DrawRay(weapon.transform.position + weapon.transform.up * 0.05f, weapon.transform.forward * 40f);
        Debug.DrawRay(transform.position, transform.forward * 40f, UnityEngine.Color.red);


        anim.SetFloat("zSpeed", Mathf.Sin(Vector3.SignedAngle(transform.forward, nma.destination - transform.position, Vector3.up) * Mathf.Deg2Rad));
        anim.SetFloat("xSpeed", Mathf.Cos(Vector3.SignedAngle(transform.forward, nma.destination - transform.position, Vector3.up) * Mathf.Deg2Rad));
    }

    void LateUpdate()
    {
        switch (state)
        {
            case 0:
                if (time >= 1.5)
                {
                    time = 0;
                    float xRand = Random.Range(-1f, 1f);
                    float zRand = Random.Range(-1f, 1f);
                    nma.destination = transform.position + new Vector3(xRand * 5,0,zRand * 5);
                }
                break;
            case 1:
                if(sphereHit.Length == 0 || time > 2.5f)
                {
                    positoin = transform.position;
                    nma.destination = positoin;
                    state = 2;
                    time = 0;
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
                    nma.destination = player.transform.position - (player.transform.position - sphereHit[minIndex].transform.position) * 1.6f;
                }
                break;
            case 2:
                transform.LookAt(player.transform.position);
                int layerMask = (1 << LayerMask.NameToLayer("Enviroment"))  + (1 << LayerMask.NameToLayer("Player")); 
                bool c1 = Physics.Raycast(weapon.transform.position + weapon.transform.up * 0.05f, weapon.transform.forward, out rayHit, 10f, layerMask);
                bool c2 = false;
                if(c1) { 
                    c2 = !rayHit.transform.tag.Equals("Cube") && transform.tag != rayHit.transform.GetComponentInParent<Character>().tag;
                }
                
                if(time >= 1f)
                { 
                    time = 0;
                    if (c1 && c2)
                    {
                        nma.destination = transform.position;
                        if (!weapon.GetComponent<PlayerWeapon>().enemyShot())
                        {
                            state = 1;
                        }
                        else
                        {
                            int temp = Random.Range(0, 2);
                            if (temp == 0)
                            {
                                nma.destination = transform.position + transform.right * 1f;
                            }
                            else
                            {
                                nma.destination = transform.position - transform.right * 1f;
                            }
                        }
                    }
                    else
                    {
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


    public void LookPlayer(GameObject targget)
    {
        if (player == null)
        {
            player = targget;
            transform.LookAt(targget.transform);
            state = 2;
            time = 0;
        }
    }
}
