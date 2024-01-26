using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent nma;
    public Transform player;
    public Transform[] target;
    private float time = 0;
    public int index = 0;
    public int state;

    public GameObject weapon;

    public float radius;
    private RaycastHit rayHit;
    private RaycastHit[] sphereHit;

    void Start()
    {
        if(nma ==null)
        {
            nma = GetComponent<NavMeshAgent>();
            Debug.LogWarning("nma가 설정이 안돼있음");
        }
        nma.destination = target[index].position;
        state = 0;
    }
    
    void FixedUpdate()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Enviroment");
        sphereHit = Physics.SphereCastAll(transform.position, radius, transform.up, 0, layerMask);
        for (int i = 0; i < sphereHit.Length; i++)
        {
            Debug.Log(sphereHit[i].transform.gameObject.name);
        }

        if (Physics.Raycast(transform.position, transform.forward, out rayHit, 10) && rayHit.transform.tag == "Player")
        {
            state = 1;
        }

        if(state == 1 && transform.position.x == nma.destination.x && transform.position.z == nma.destination.z)
        {
            state = 2;
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
                    index = Random.Range(0, 3);
                    nma.destination = target[index].position;
                }
                break;
            case 1:
                if(sphereHit.Length == 0)
                {
                    nma.destination = transform.position;
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
                    nma.destination = player.position - (player.position - sphereHit[minIndex].transform.position) * 1.3f;

                }
                break;
            case 2:
                transform.LookAt(player.position + new Vector3(0,1f,0));

                break;
        }
    }
}
