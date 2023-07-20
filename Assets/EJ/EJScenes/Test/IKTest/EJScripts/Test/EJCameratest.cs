/*using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public static PlayerHP instance;

    int hp;
    public int MaxHp;

    public SkinnedMeshRenderer[] bodyRendererList;
    public bool isDie;
    public Image damageImage;
    public GameObject otherbody;
    bool Damage;
    bool die;
    bool InkDamage;
    float currentTime;
    public float recoveryTime;
    int count;

    public Camera mainCamera;
    public Vector3 cameraPos;
    [Range(0.01f, 0.1f)] float shakeRange = 0.05f;
    [Range(0.1f, 0.5f)] float duration = 0.2f;

    [Range(0.01f, 0.5f)] float shakeAttackRange = 0.2f;
    [Range(0.1f, 0.8f)] float Attackduration = 0.4f;

    [Range(0.01f, 0.02f)] float shakeInkRange = 0.01f;

    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        MaxHp = 100;
        HP = MaxHp;
        isDie = false;
        Damage = false;
        damageImage.enabled = false;
        die = false;
        InkDamage = false;
        currentTime = 0;
        bodyRendererList = HumanBodyMeshManager.Instance.MeshList;
        count = HumanBodyMeshManager.Instance.count;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInkDamageProcess();
        ShowPlayerDamageUIImage();
    }

    public void PlayerStrongDamageProcess()
    {
        //내 체력 1 감소
        HP -= 60;
        //내 체력 0일때
        if (HP <= 0)
        {
            Die();
        }
        else
        {
            if (Damage == false)
            {
                StartCoroutine(PlayerDamageManager());
                StartCoroutine(DamageUIManage());
                AttackShake();
                StartCoroutine(UnBeat());
                Recure();
                Damage = true;
            }
            //내 체력 0이 아닐 때
        }
    }

    public void PlayerWeakDamageProcess()
    {
        HP -= 20;
        if (HP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(PlayerDamageManager());
            StartCoroutine(DamageUIManage());
            StartCoroutine(UnBeat());
            Shake();
            Recure();
        }
    }

    public void PlayerInkDamageProcess()
    {
        if (Player_CameraAndMove.instance.inkState == Player_CameraAndMove.InkState.other)
        {
            if (HP >= 100)
            {
                HP -= 20;
            }
            InkShake();
            InkDamage = true;
        }
        else if (Player_CameraAndMove.instance.inkState != Player_CameraAndMove.InkState.other)
        {
            StopInkShake();
            if (InkDamage == true && die == false)
            {
                Recure();
            }
        }
    }

    public void Shake()
    {
        cameraPos = mainCamera.transform.position;
        InvokeRepeating("StartShake", 0f, 0.05f);
        Invoke("StopShake", duration);
    }

    public void InkShake()
    {
        if (Damage == false)
        {
            cameraPos = mainCamera.transform.position;
            InvokeRepeating("StartInkShake", 0f, 0.05f);
        }
        //if(Player_CameraAndMove.instance.inkState != Player_CameraAndMove.InkState.other)
        //{
        //    CancelInvoke("Shake");
        //    mainCamera.transform.position = cameraPos;
        //}
    }

    public void StartShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 cameraPos = mainCamera.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        mainCamera.transform.position = cameraPos;
    }

    public void StopShake()
    {
        CancelInvoke("StartShake");
        mainCamera.transform.position = cameraPos;
    }


    public void StartInkShake()
    {
        float cameraPosX = Random.value * shakeInkRange * 2 - shakeInkRange;
        float cameraPosY = Random.value * shakeInkRange * 2 - shakeInkRange;
        Vector3 cameraPos = mainCamera.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        mainCamera.transform.position = cameraPos;
    }

    public void StopInkShake()
    {
        CancelInvoke("StartInkShake");
        // mainCamera.transform.position = cameraPos;
    }

    public void AttackShake()
    {
        cameraPos = mainCamera.transform.position;
        InvokeRepeating("StartAttackShake", 0f, 0.001f);
        Invoke("StopAttackShake", Attackduration);
    }

    public void StartAttackShake()
    {
        float cameraPosX = Random.value * shakeAttackRange * 2 - shakeAttackRange;
        float cameraPosY = Random.value * shakeAttackRange * 2 - shakeAttackRange;
        Vector3 cameraPos = mainCamera.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        mainCamera.transform.position = cameraPos;
    }

    public void StopAttackShake()
    {
        CancelInvoke("StartAttackShake");
        mainCamera.transform.position = cameraPos;
    }

    public void Die()
    {
        GameObject beforeimage = DamageUIManager.instance.imageList[3];
        beforeimage.SetActive(false);
        GameObject image = DamageUIManager.instance.imageList[4];
        image.SetActive(true);
        for (int i = 0; i < count; i++)
        {
            bodyRendererList[i].enabled = false;
        }
        die = true;
    }

    public void ShowPlayerDamageUIImage()
    {
        if (HP >= 100)
        {
            ;
        }
        else if (HP < 100 && HP >= 80)
        {
            GameObject image = DamageUIManager.instance.imageList[1];
            image.SetActive(true);
        }
        else if (HP < 80 && HP >= 40)
        {
            GameObject beforeimage = DamageUIManager.instance.imageList[1];
            beforeimage.SetActive(false);
            GameObject image = DamageUIManager.instance.imageList[2];
            image.SetActive(true);
        }
        else if (hp < 40 && die == false)
        {
            for (int i = 1; i < 3; i++)
            {
                GameObject beforeimage = DamageUIManager.instance.imageList[(int)i];
                beforeimage.SetActive(false);
            }
            GameObject image = DamageUIManager.instance.imageList[3];
            image.SetActive(true);
        }
    }

    public void Recure()
    {
        currentTime += Time.deltaTime;
        if (currentTime > recoveryTime)
        {
            HP = 100;
            for (int i = 1; i < DamageUIManager.instance.count; i++)
            {
                GameObject image = DamageUIManager.instance.imageList[i];
                image.SetActive(false);
                currentTime = 0;
                InkDamage = false;
            }
        }

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.name.Contains("Fist") || collision.gameObject.name.Contains("Hand"))
    //    {
    //        HP--;
    //        StartCoroutine(PlayerDamage());
    //        print("Hit");
    //    }
    //}
    IEnumerator DamageUIManage()
    {
        yield return DamageUI();
        yield return damageImage.enabled = false;
    }

    IEnumerator DamageUI()
    {
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            damageImage.enabled = true;
            yield return 0;
        }
    }

    IEnumerator PlayerDamageManager()
    {
        yield return PlayerDamage(false);
        BodyChange(true);
    }

    IEnumerator PlayerDamage(bool value)
    {

        for (float t = 0; t < 1.5f; t += Time.deltaTime)
        {
            BodyChange(value);
            yield return 0;
        }
    }

    void BodyChange(bool value)
    {
        for (int i = 0; i < count; i++)
        {
            bodyRendererList[i].enabled = value;
        }
    }

    IEnumerator UnBeat()
    {


        yield return new UnityEngine.WaitForSeconds(1f)
        {

        };
        Damage = false;
    }
}*/