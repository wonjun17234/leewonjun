using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent nma;
    public Transform target;


    void Start()
    {
        if(nma ==null)
        {
            nma = GetComponent<NavMeshAgent>();
            Debug.LogWarning("nma가 설정이 안돼있음");
        }
    }

    // Update is called once per frame
    void Update()
    {
        nma.destination = target.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, target.position + Vector3.up * 0.5f - transform.position, out hit, 10))
        {
            if(hit.transform.tag == "Player")
            {
                nma.speed = 0;
                transform.LookAt(hit.point);
            }
            
            Debug.DrawRay(transform.position, target.position - transform.position);

        }
        else
        {
            nma.speed = 3.5f;
        }
    }

    
}
