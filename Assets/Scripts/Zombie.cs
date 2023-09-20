using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    Animator animator = null;
    public GameObject playerObject = null;
    public float checkDistance = 5;
    public float attackDistance = 2;
    public int HP = 100;

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
        }
    }

    private void Move()
    {
        animator.SetBool("isMove", true);
        GetComponent<NavMeshAgent>().destination = playerObject.transform.position;
    }

    private void Attack()
    {
        animator.SetTrigger("isAttack");
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
        }

    }

    private void Death()
    {
        animator.SetBool("isDeath", true);
    }
}
