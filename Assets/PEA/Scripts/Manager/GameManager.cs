
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private int redKillCount = 0;
    private int blueKillCount = 0;
    private float curTime = 0f;
    private float countMinutes = 0f;
    private float countSeconds = 0f;
    private float countPercent = 0f;
    private bool isStart = false;
    private bool isOver = false;
    private bool isCountDown = false;
    private Coroutine coroutine;

    private readonly float startCountTime = 10f;
    private readonly float playCountTime = 180f;
    private readonly float overScreenTime = 5f;                    // 게임 종료 화면이 보여질 시간

    public GameObject mainImage;

    public bool IsStart {
        get { return isStart; } 
    }

    public int RedKillCount
    {
        get { return redKillCount; }
    }

    public int BlueKillCount
    {
        get { return blueKillCount; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            Destroy(gameObject);
        }

        //if(SceneManager.GetActiveScene().buildIndex == 0)
        //{
        //    CursorUnlock();
        //}
        //else if(SceneManager.GetActiveScene().buildIndex == 1)
        //{
        //    CursorLock();
        //}

    }

    // Update is called once per frame
    void Update()
    {
        //print(coroutine);
        //print(isStart);

        if (isStart)
        {
            PlayCountDown();
        }

        if (isOver)
        {
            End();
        }
    }

    public void CursorLock()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CursorUnlock()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        UIManager.instance.OnClickFindGame();
        if (coroutine == null)
        {
            coroutine = StartCoroutine(StartCountDown());
        }
    }

    private void PlayCountDown()
    {
        curTime += Time.deltaTime;
        countMinutes = (playCountTime - curTime) / 60;
        countSeconds = (playCountTime - curTime) % 60;

        // 9초 이하일 때 0으로 채워 두 글자로 맞춤
        string countText = (int)countMinutes + " : " + ((int)countSeconds).ToString().PadLeft(2, '0');

        countPercent = 1 - (curTime / playCountTime); 

        UIManager.instance.UpdtateCountDown(countText, countPercent);

        if(curTime >= playCountTime)
        {
            curTime = 0f;
            GameOver();
        }
    }

    // 레드팀(플레이어)이 킬을 했을 때 호출.
    public void RedKillBlue()
    {
        redKillCount++;
    }

    // 블루팀(스카웃)이 킬을 했을 때 호출
    public void BlueKillRed()
    {
        blueKillCount++;
    }

    // 수레가 마지막 종착지에 도착했을 때 호출
    public void Goal()
    {
        curTime = 0f;
        isOver = true;
        UIManager.instance.Goal();
    }

    // 제한시간 초과시 호출
    private void GameOver()
    {
        isOver = true;
        isStart = false;
        //Time.timeScale = 0;
        UIManager.instance.GameOver();
    }

    // 엔딩 화면을 일정 시간동안 보여주고 메인화면으로 돌아감
    private void End()
    {
        curTime += Time.deltaTime;

        if(curTime >= overScreenTime)
        {
            curTime = 0f;


            // 게임매니저와 UI매니저를 삭제하고 새로 만듦.
            instance = null;
            UIManager.instance.End();

            SceneManager.LoadScene(0);

            Destroy(this);
        }
    }

    public void Restart()
    {
        curTime = 0f;
        print("restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator StartCountDown()
    {
        yield return new WaitForSeconds(startCountTime);
        isStart = true;
        UIManager.instance.SetRuleUIActive(false);
        yield return null;
        coroutine = null;
    }
}
