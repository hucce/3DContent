using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // ����
    public float moveSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float ver = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");



        // ���ǹ� ver�� 0�� �ƴ� ���
        // ex) ���� ==, ���� ���� ��� !=, �ʰ� <, �̻�(���ų� ����) <=
        if (ver != 0)
        {
            Vector3 moveVector = new Vector3();
            moveVector.x = hor * moveSpeed * Time.deltaTime;
            moveVector.z = ver * moveSpeed * Time.deltaTime;
            
            transform.position = transform.position + moveVector;
        }
        else if(hor != 0)
        {
            Vector3 moveVector = new Vector3();
            moveVector.x = hor * moveSpeed * Time.deltaTime;
            moveVector.z = ver * moveSpeed * Time.deltaTime;

            transform.position = transform.position + moveVector;
        }
        else
        {

        }
    }
}
