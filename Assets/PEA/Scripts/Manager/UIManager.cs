using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject main;

    // ���� UI
    public GameObject wagonProgress;
    public Image checkPointImage;
    public Sprite checkPointImageRed;

    // ���ѽð� 
    public GameObject countUI;
    public TMP_Text countText;                  // ���ѽð� �ؽ�Ʈ
    public Image countPercentImage;                 // ���ѽð� �ؽ�Ʈ ���� ���׶� �̹���

    // ���� ����� UI
    public GameObject gameClearUI;
    public GameObject gameOverUI;
    public GameObject scoreUI;
    public TMP_Text blueScoreText;
    public TMP_Text redScoreText;

    // �÷��̾� ���� UI
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

    //���� ã�� ��ư�� ������ ȣ��
    public void OnClickFindGame()
    {
        main.SetActive(false);
        wagonProgress.SetActive(true);
        countUI.SetActive(true);
        gameOverUI.SetActive(false);
    }

    // ī��Ʈ�ٿ� UI(ui�� ������ ī��Ʈ�ٿ� �ؽ�Ʈ, ���� �ð� �ۼ�Ʈ)
    public void UpdtateCountDown(string countDownText, float countPercent)
    {
        countText.text = countDownText;
        countPercentImage.fillAmount = countPercent;
    }

    // ������ üũ����Ʈ�� ������ üũ����Ʈ �̹��� �ٲٱ�
    public void ChangeCheckPointImage()
    {
        checkPointImage.sprite = checkPointImageRed;
    }

    // ���� ����� ���� �����ֱ�
    private void SetScoreText()
    {
        redScoreText.text = GameManager.instance.RedKillCount.ToString();
        blueScoreText.text = GameManager.instance.BlueKillCount.ToString();
        scoreUI.SetActive(true);
    }

    // ������ ������ �������� �������� �� ���ӸŴ����� Goal()���� ȣ��
    public void Goal()
    {
        wagonProgress.SetActive(false);
        //PlayerUISetActive(false);
        countUI.SetActive(false);
        SetScoreText();
        gameClearUI.SetActive(true);
    }

    // ���ѽð��� ��� ������ �� ���ӸŴ����� GameOver()���� ȣ��
    public void GameOver()
    {
        wagonProgress.SetActive(false);
        //PlayerUISetActive(false);
        countUI.SetActive(false);
        SetScoreText();
        gameOverUI.SetActive(true);
    }

    // �÷��̾� ���� UI �ѱ�/����
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
