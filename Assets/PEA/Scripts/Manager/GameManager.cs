using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private float curTime = 0f;
    private bool isStart = false;
    private bool isGoal = false;
    private bool isCountDown = false;
    private Coroutine coroutine;

    private readonly float startCountTime = 10f;
    private readonly float playCountTime = 180f;

    public GameObject mainImage;

    public bool IsStart {
        get { return isStart; } 
    }

    public bool IsGoal
    {
        get { return isGoal; }
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

        if (isStart && !isGoal)
        {
            PlayCountDown();

            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                GameOver();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Goal();
            }
        }
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
        if(curTime >= playCountTime)
        {
            GameOver();
        }
    }

    // 수레가 마지막 종착지에 도착했을 때 호출
    public void Goal()
    {
        isGoal = true;
        UIManager.instance.Goal();
    }

    // 제한시간 초과시 호출
    private void GameOver()
    {
        isStart = false;
        Time.timeScale = 0;
        UIManager.instance.GameOver();
    }

    public void Restart()
    {
        curTime = 0f;
        print("restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        StartGame();
    }

    public void GoToMain()
    {
        Destroy(gameObject);
        Destroy(UIManager.instance.gameObject);
        SceneManager.LoadScene("Start");
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
