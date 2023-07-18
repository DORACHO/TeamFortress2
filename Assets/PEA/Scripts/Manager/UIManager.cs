using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject wagonProgress;
    public GameObject main;
    public GameObject gameOverUI;
    public GameObject gameClearUI;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //���� ã�� ��ư�� ������ ȣ��
    public void OnClickFindGame()
    {
        main.SetActive(false);
        wagonProgress.SetActive(true);
        gameOverUI.SetActive(false);
    }

    // ������ ������ �������� ���F���� �� ���ӸŴ����� Goal()���� ȣ��
    public void Goal()
    {
        wagonProgress.SetActive(false);
        gameClearUI.SetActive(true);
    }

    // ���ѽð��� ��� ������ �� ���ӸŴ����� GameOver()���� ȣ��
    public void GameOver()
    {
        wagonProgress.SetActive(false);
        gameOverUI.SetActive(true);
    }

}
