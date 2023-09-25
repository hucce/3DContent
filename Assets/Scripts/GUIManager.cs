using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public GameObject background = null;
    public GameObject gameOverText = null;

    public GameObject HP_Obj = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGameOver()
    {
        background.SetActive(true);
        gameOverText.SetActive(true);
    }

    public void ShowStageClear()
    {
        gameOverText.SetActive(true);
        gameOverText.GetComponent<TextMeshProUGUI>().text = "STAGE CLEAR";
    }

    public void ShowHP(int _HP)
    {
        HP_Obj.GetComponent<TextMeshProUGUI>().text = _HP.ToString();
    }
}
