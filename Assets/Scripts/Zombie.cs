using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    Animator animator = null;
    public GameObject playerObject = null;
    public float checkDistance = 5;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Idle();
    }

    // 대기, 추격(이동), 공격, 피격, 죽음
    void Idle()
    {
        animator.SetBool("isMove", false);

        Vector3 playerVector = playerObject.transform.position;
        Vector3 zombieVector = transform.position;
        float playerZombieDistance = Vector3.Distance(zombieVector, playerVector);

        // 좀비와 플레이어 사이의 거리가 일정 수치 이하일 때(가까울 때)
        if(playerZombieDistance < checkDistance)
        {
            // 추격한다.
            Move();
        }
    }

    private void Move()
    {
        GetComponent<NavMeshAgent>().destination = playerObject.transform.position;
    }
}
