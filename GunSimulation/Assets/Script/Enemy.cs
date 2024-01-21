using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent nma;
    public Transform player;
    public Transform[] target;
    private float time = 0;
    public int index = 0;
    void Start()
    {
        if(nma ==null)
        {
            nma = GetComponent<NavMeshAgent>();
            Debug.LogWarning("nma가 설정이 안돼있음");
        }
        nma.destination = target[index].position;
    }

    // Update is called once per frame
    void Update()
    {
        //거점 방어인지 거점 공격인지 마구잡이 사냥인지
        time += Time.deltaTime;
        if(time >= 3) //만약 플레이어를 발견 했다면 타겟 변경 안되게 변경하기 && 큐브가 아닌 특정 경로로 바꾸기
        {
            time = 0;
            index = Random.Range(0, 3);
            nma.destination = target[index].position;
        }
        
        RaycastHit hit;
        // 플레이어를 발견하면 큐브 리스트중에 가장 가까운 큐브가 있는지 확인하고 있으면 가장 가까운 큐브로 이동(이동 속도 매우 빠르게) 없으면 그냥 싸우기
        // 플레이어와 큐브로 플레이어가 바라보는 반대의 위치로 이동
        if (Physics.Raycast(transform.position, player.position + Vector3.up - transform.position, out hit, 10) && hit.transform.tag == "Player")
        {
            nma.destination = player.position;
            transform.LookAt(hit.point);
        }


        // 큐브 뒤에서 공격은 잠깐 오른쪽 또는 왼쪽으로 나가서 플레이어를 바라보며 2발 쏘고 들어가기
        // 큐브에 hp 추가해서 파괴 가능하게 하기
        // 큐브가 파괴되면 적은 또 다른 큐브 찾기 없으면 그냥 싸우기

        // 총쏘는 함수, 근처 큐브를 찾는 함수 
    }
    private void OnTriggerEnter(Collider other)
    { 
        // 현재 큐브 리스트에 추가하기
    }

    private void OnTriggerExit(Collider other)
    {
        // 리스트에서 삭제하기
    }
}
