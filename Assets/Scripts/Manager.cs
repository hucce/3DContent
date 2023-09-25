using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public int zombieCount = 0;

    public string nextScene = "";

    // Start is called before the first frame update
    void Start()
    {
        zombieCount = GameObject.FindGameObjectsWithTag("Zombie").Length;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ZombieDeath()
    {
        zombieCount = zombieCount - 1;

        if (zombieCount == 0)
        {
            StartCoroutine(CoStageClear());
        }
    }

    IEnumerator CoStageClear()
    {
        // �������� Ŭ���� ȭ�� ��� �� ���� �ð��� ������ ���� ���������� �ѱ��.

        // �������� Ŭ���� ȭ�� ���
        GetComponent<GUIManager>().ShowStageClear();

        // ���� �ð� ��
        yield return new WaitForSeconds(3);

        // ���������� ���� ���������� �ѱ��
        SceneManager.LoadScene(nextScene);
    }
}
