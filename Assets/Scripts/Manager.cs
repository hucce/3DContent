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
        // 스테이지 클리어 화면 출력 후 일정 시간이 지나면 다음 스테이지로 넘긴다.

        // 스테이지 클리어 화면 출력
        GetComponent<GUIManager>().ShowStageClear();

        // 일정 시간 후
        yield return new WaitForSeconds(3);

        // 스테이지를 다음 스테이지로 넘긴다
        SceneManager.LoadScene(nextScene);
    }
}
