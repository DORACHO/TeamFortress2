using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject main;

    // 수레 UI
    public GameObject wagonProgress;
    public Image checkPointImage;
    public Sprite checkPointImageRed;

    // 제한시간 
    public GameObject countUI;
    public TMP_Text countText;                  // 제한시간 텍스트
    public Image countPercentImage;                 // 제한시간 텍스트 옆에 동그란 이미지

    // 게임 종료시 UI
    public GameObject gameClearUI;
    public GameObject gameOverUI;
    public GameObject scoreUI;
    public TMP_Text blueScoreText;
    public TMP_Text redScoreText;

    // 플레이어 관련 UI
    public GameObject[] playerUI;


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
        countUI.SetActive(true);
        gameOverUI.SetActive(false);
    }

    // 카운트다운 UI(ui로 보여질 카운트다운 텍스트, 남은 시간 퍼센트)
    public void UpdtateCountDown(string countDownText, float countPercent)
    {
        countText.text = countDownText;
        countPercentImage.fillAmount = countPercent;
    }

    // 수레가 체크포인트를 지나면 체크포인트 이미지 바꾸기
    public void ChangeCheckPointImage()
    {
        checkPointImage.sprite = checkPointImageRed;
    }

    // 게임 종료시 점수 보여주기
    private void SetScoreText()
    {
        redScoreText.text = GameManager.instance.RedKillCount.ToString();
        blueScoreText.text = GameManager.instance.BlueKillCount.ToString();
        scoreUI.SetActive(true);
    }

    // 수레가 마지막 종착지에 도착했을 때 게임매니저의 Goal()에서 호출
    public void Goal()
    {
        wagonProgress.SetActive(false);
        //PlayerUISetActive(false);
        countUI.SetActive(false);
        SetScoreText();
        gameClearUI.SetActive(true);
    }

    // 제한시간이 모두 끝났을 때 게임매니저의 GameOver()에서 호출
    public void GameOver()
    {
        wagonProgress.SetActive(false);
        //PlayerUISetActive(false);
        countUI.SetActive(false);
        SetScoreText();
        gameOverUI.SetActive(true);
    }

    // 플레이어 관련 UI 켜기/끄기
    private void PlayerUISetActive(bool isActive)
    {
        for (int i = 0; i < playerUI.Length; i++)
        {
            playerUI[i].SetActive(isActive);
        }
    }

    public void End()
    {
        instance = null;
        Destroy(this);
    }

}
