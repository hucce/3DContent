using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    Animator animator = null;

    // �÷��̾� ĳ����
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

    // ���, �߰�(�̵�), ����, �ǰ�, ����
    void Idle()
    {
        Vector3 playerVector = playerObject.transform.position;
        Vector3 zombieVector = transform.position;
        float playerZombieDistance = Vector3.Distance(zombieVector, playerVector);

        // ����� �÷��̾� ������ �Ÿ��� ���� ��ġ ������ ��(����� ��)
        if(playerZombieDistance < attackDistance)
        {
            // ���ݹ��� ���̶�� ����
            Attack();
        }
        else if (playerZombieDistance < checkDistance)
        {
            // ���ݹ����� ���̰� �̵� ������ ���� ���
            // �߰��Ѵ�.
            Move();
        }
        else
        {
            // �ٽ� �Ÿ��� �־����ٸ� Ȥ�� �������� �Ÿ��� �־��ٸ� move�ִϸ��̼��� false�� �����.
            animator.SetBool("isMove", false);
            state = Player.State.Idle;
        }
    }

    private void Move()
    {
        // �������� ���� �̵�
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
            // �̵��� �����.
            GetComponent<NavMeshAgent>().enabled = false;

            state = Player.State.Attack;
            animator.SetBool("isMove", false);
            animator.SetTrigger("isAttack");

            // ���� �ð����� ����Ѵ�.
            yield return new WaitForSeconds(attackSecond);

            // yield return () null > 1������ ����Ѵ�.
            //new WaitForSeconds Ư�� �ʸ� ����Ѵ�.
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
            // �¾����� �ǰ� ���·� ����
            state = Player.State.Hit;

            // HP�� -�� �ȴ�. hp = hp-������ ���ݷ�
            HP = HP - 10;

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
