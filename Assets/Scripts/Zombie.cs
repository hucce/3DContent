using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    Animator animator = null;

    // 플레이어 캐릭터
    private GameObject playerObject = null;
    public float checkDistance = 5;
    public float attackDistance = 2;
    public int HP = 100;

    Player.State state = Player.State.Idle;

    public float attackSecond = 1;

    public float hitSecond = 1;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
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
        Vector3 playerVector = playerObject.transform.position;
        Vector3 zombieVector = transform.position;
        float playerZombieDistance = Vector3.Distance(zombieVector, playerVector);

        // 좀비와 플레이어 사이의 거리가 일정 수치 이하일 때(가까울 때)
        if(playerZombieDistance < attackDistance)
        {
            // 공격범위 안이라면 공격
            Attack();
        }
        else if (playerZombieDistance < checkDistance)
        {
            // 공격범위는 밖이고 이동 범위는 안인 경우
            // 추격한다.
            Move();
        }
        else
        {
            // 다시 거리가 멀어졌다면 혹은 원래부터 거리가 멀었다면 move애니메이션을 false로 만든다.
            animator.SetBool("isMove", false);
            state = Player.State.Idle;
        }
    }

    private void Move()
    {
        // 대기상태일 때만 이동
        if(state == Player.State.Idle || state == Player.State.Move)
        {
            GetComponent<NavMeshAgent>().enabled = true;
            state = Player.State.Move;
            animator.SetBool("isMove", true);
            GetComponent<NavMeshAgent>().destination = playerObject.transform.position;
        }
    }

    void Attack()
    {
        StartCoroutine(CoAttack());
    }

    IEnumerator CoAttack()
    {
        if (state == Player.State.Idle || state == Player.State.Move)
        {
            // 이동을 멈춘다.
            GetComponent<NavMeshAgent>().enabled = false;

            state = Player.State.Attack;
            animator.SetBool("isMove", false);
            animator.SetTrigger("isAttack");

            // 일정 시간동안 대기한다.
            yield return new WaitForSeconds(attackSecond);

            // yield return () null > 1프레임 대기한다.
            //new WaitForSeconds 특정 초를 대기한다.
            state = Player.State.Idle;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PlayerSword")
        {
            Hit();
        }
    }

    private void Hit()
    {
        StartCoroutine(CoHit());
    }

    IEnumerator CoHit()
    {
        if (state != Player.State.Hit || state != Player.State.Death)
        {
            // 맞았으면 피격 상태로 변경
            state = Player.State.Hit;

            // HP가 -가 된다. hp = hp-상대방의 공격력
            HP = HP - 10;

            // 만약 HP가 0이하라면 죽어야한다.
            if (HP <= 0)
            {
                Death();
            }
            else
            {
                // 애니메이션 출력(피격)
                animator.SetTrigger("isHit");

                // 피격초까지 대기
                yield return new WaitForSeconds(hitSecond);

                // 다 맞았으면 대기 상태로 돌리기
                state = Player.State.Idle;
            }
        }
    }

    private void Death()
    {
        animator.SetBool("isDeath", true);
        state = Player.State.Death;
    }
}
