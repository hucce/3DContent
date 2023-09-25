using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator = null;
    public int HP = 100;

    // 변수
    public float moveSpeed = 0.2f;

    State state = State.Idle;

    public float attackSecond = 1;

    public float hitSecond = 1;

    GameObject manager = null;

    public int attackDamage = 50;

    public enum State
    {
        Idle, Move, Attack, Hit, Death
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        manager = GameObject.FindGameObjectWithTag("Manager");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
    }

    private void Move()
    {
        if (state == State.Idle || state == State.Move)
        {
            float ver = Input.GetAxis("Vertical");
            float hor = Input.GetAxis("Horizontal");

            // 조건문 ver가 0이 아닌 경우
            // ex) 같다 ==, 같지 않은 경우 !=, 초과 <, 이상(같거나 많다) <=
            // || > 또는 or, and && > 그리고
            if (ver != 0 || hor != 0)
            {
                // 이동이 되면 이동 상태로 변경
                state = State.Move;
                animator.SetBool("isMove", true);

                Vector3 moveVector = new Vector3();
                moveVector.x = hor * moveSpeed * Time.deltaTime;
                moveVector.z = ver * moveSpeed * Time.deltaTime;

                transform.position = transform.position + moveVector;

                Vector3 checkVector = new Vector3();
                if (moveVector != checkVector)
                {
                    transform.forward = moveVector;
                }
            }
            else
            {
                // 이동 상태가 끝나면 다시 대기상태로 변경
                state = State.Idle;
                animator.SetBool("isMove", false);
            }
        }
    }

    void Attack()
    {
        StartCoroutine(CoAttack());
    }

    IEnumerator CoAttack()
    {
        if(state == State.Idle || state == State.Move)
        {
            bool att = Input.GetButtonDown("Fire1");

            if (att == true)
            {
                // 공격 버튼을 누르면 어택상태로 변경
                state = State.Attack;
                animator.SetBool("isMove", false);
                animator.SetTrigger("isAttack");

                // 일정 시간동안 대기한다.
                yield return new WaitForSeconds(attackSecond);

                // yield return () null > 1프레임 대기한다.
                //new WaitForSeconds 특정 초를 대기한다.
                state = State.Idle;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ZombieHand")
        {
            int zombieAtt = collision.transform.root.GetComponent<Zombie>().attackDamage;

            Hit(zombieAtt);
        }
    }

    private void Hit(int _zombieAtt)
    {
        StartCoroutine(CoHit(_zombieAtt));
    }

    IEnumerator CoHit(int _zombieAtt)
    {
        if(state != State.Hit || state != State.Death)
        {
            // 맞았으면 피격 상태로 변경
            state = State.Hit;

            // HP가 -가 된다. hp = hp-상대방의 공격력
            HP = HP - _zombieAtt;

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
                state = State.Idle;

                manager.GetComponent<GUIManager>().ShowHP(HP);
            }
        }
    }

    private void Death()
    {
        StartCoroutine(CoDeath());
    }

    IEnumerator CoDeath()
    {
        if(state != State.Death)
        {
            //죽었으면 죽음 상태로 변경
            state = State.Death;

            animator.SetBool("isDeath", true);

            // 일정 시간 이후에 게임 오버 GUI를 보인다.
            yield return new WaitForSeconds(2);

            manager.GetComponent<GUIManager>().ShowGameOver();
        }
    }
}
