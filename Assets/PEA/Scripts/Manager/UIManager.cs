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

    //게임 찾기 버튼을 누르면 호출
    public void OnClickFindGame()
    {
        main.SetActive(false);
        wagonProgress.SetActive(true);
        gameOverUI.SetActive(false);
    }

    // 수레가 마지막 종착지에 도찯했을 때 게임매니저의 Goal()에서 호출
    public void Goal()
    {
        wagonProgress.SetActive(false);
        gameClearUI.SetActive(true);
    }

    // 제한시간이 모두 끝났을 때 게임매니저의 GameOver()에서 호출
    public void GameOver()
    {
        wagonProgress.SetActive(false);
        gameOverUI.SetActive(true);
    }

}
