using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator = null;
    public int HP = 100;

    // ����
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

            // ���ǹ� ver�� 0�� �ƴ� ���
            // ex) ���� ==, ���� ���� ��� !=, �ʰ� <, �̻�(���ų� ����) <=
            // || > �Ǵ� or, and && > �׸���
            if (ver != 0 || hor != 0)
            {
                // �̵��� �Ǹ� �̵� ���·� ����
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
                // �̵� ���°� ������ �ٽ� �����·� ����
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
                // ���� ��ư�� ������ ���û��·� ����
                state = State.Attack;
                animator.SetBool("isMove", false);
                animator.SetTrigger("isAttack");

                // ���� �ð����� ����Ѵ�.
                yield return new WaitForSeconds(attackSecond);

                // yield return () null > 1������ ����Ѵ�.
                //new WaitForSeconds Ư�� �ʸ� ����Ѵ�.
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
            // �¾����� �ǰ� ���·� ����
            state = State.Hit;

            // HP�� -�� �ȴ�. hp = hp-������ ���ݷ�
            HP = HP - _zombieAtt;

            // ���� HP�� 0���϶�� �׾���Ѵ�.
            if (HP <= 0)
            {
                Death();
            }
            else
            {
                // �ִϸ��̼� ���(�ǰ�)
                animator.SetTrigger("isHit");

                // �ǰ��ʱ��� ���
                yield return new WaitForSeconds(hitSecond);

                // �� �¾����� ��� ���·� ������
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
            //�׾����� ���� ���·� ����
            state = State.Death;

            animator.SetBool("isDeath", true);

            // ���� �ð� ���Ŀ� ���� ���� GUI�� ���δ�.
            yield return new WaitForSeconds(2);

            manager.GetComponent<GUIManager>().ShowGameOver();
        }
    }
}
