
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
    private readonly float overScreenTime = 5f;                    // ���� ���� ȭ���� ������ �ð�

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
        else
        {
            Destroy(gameObject);
        }
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
        if(coroutine == null)
        {
            coroutine = StartCoroutine(StartCountDown());
        }
    }

    private void PlayCountDown()
    {
        curTime += Time.deltaTime;
        countMinutes = (playCountTime - curTime) / 60;
        countSeconds = (playCountTime - curTime) % 60;

        // 9�� ������ �� 0���� ä�� �� ���ڷ� ����
        string countText = (int)countMinutes + " : " + ((int)countSeconds).ToString().PadLeft(2, '0');

        countPercent = 1 - (curTime / playCountTime); 

        UIManager.instance.UpdtateCountDown(countText, countPercent);

        if(curTime >= playCountTime)
        {
            curTime = 0f;
            GameOver();
        }
    }

    // ������(�÷��̾�)�� ų�� ���� �� ȣ��.
    public void RedKillBlue()
    {
        redKillCount++;
    }

    // �����(��ī��)�� ų�� ���� �� ȣ��
    public void BlueKillRed()
    {
        blueKillCount++;
    }

    // ������ ������ �������� �������� �� ȣ��
    public void Goal()
    {
        curTime = 0f;
        isOver = true;
        UIManager.instance.Goal();
    }

    // ���ѽð� �ʰ��� ȣ��
    private void GameOver()
    {
        isOver = true;
        isStart = false;
        //Time.timeScale = 0;
        UIManager.instance.GameOver();
    }

    // ���� ȭ���� ���� �ð����� �����ְ� ����ȭ������ ���ư�
    private void End()
    {
        curTime += Time.deltaTime;

        if(curTime >= overScreenTime)
        {
            curTime = 0f;


            // ���ӸŴ����� UI�Ŵ����� �����ϰ� ���� ����.
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
        yield return null;
        coroutine = null;
    }
}
