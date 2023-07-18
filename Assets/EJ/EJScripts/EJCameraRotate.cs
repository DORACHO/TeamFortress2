using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJCameraRotate : MonoBehaviour
{
    //Rotate ����
    float rx, ry;
    float rotSpeed = 100;
    public GameObject player;

    //Camera React ����
    float cx;
    float targetCX;

    //Shake ����
    float knockbackSpeed = 5;
    float knockbackRotation = 1;
    bool isUp;

    //Audio ����
    public AudioSource fireSFX;
    public AudioSource reloadSFX;

    // Start is called before the first frame update
    void Start()
    {
        //rx, ry ���� ���� �ʱ� eulerAngle ���� ���� 
        ry = player.transform.localEulerAngles.y;

        //cx �ʱ�ȭ
        //targetCX = 6;
        targetCX = transform.localEulerAngles.x;

        //Q.������� �������� ���?
        fireSFX = GetComponent<AudioSource>();
        reloadSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse�� �����ӿ� ���� Rotate
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rx += my * rotSpeed * Time.deltaTime;
        ry += mx * rotSpeed * Time.deltaTime;

        rx = Mathf.Clamp(rx, -75, 75);
        
        transform.localEulerAngles = new Vector3(-rx+cx, 0, 0);
        player.transform.localEulerAngles = new Vector3(0, ry, 0);

       //UpdateCameraReact();
        cx = Mathf.Lerp(cx, targetCX, Time.deltaTime * 8);

    }

    public bool isKnockBack;
    Vector3 knockbackPos;
    Vector3 currentPosition;
    
    
    //��ư�� ������ fireScript���� �Լ� ȣ���ϰų�,
    // fireScript�� canFire bool�� true��� �۵��ϰ� �ϰų�.

    public void UpdateCameraReact()
    {               
        //if (Input.GetButtonDown("Fire1"))
        //if (GetComponent<EJPlayerFireShotGun>().canFire)
        {
            cx -= 10;
        }
    }


    /*public void KnockBack()
    {
        //���� �ö󰬴ٰ�, �������ϰ� �������� �ʹ�.
        transform.position = knockbackPos;
        cx += knockbackRotation;

        //���࿡ ���� ��ġ�� KnockbackPos�� ��������ٸ�
        float distance = Vector3.Distance(transform.position, player.transform.position + knockbackPos);

        if (distance < 0.1f)
        {
            transform.position = Vector3.Lerp(knockbackPos, currentPosition, knockbackSpeed * Time.deltaTime);
            cx = Mathf.Lerp(cx, cx - knockbackRotation, 0.8f);
            isKnockBack = false;
        }
                
    }*/

    public void StartKnockBack()
    {
        isKnockBack = true;
        Vector3 knockBackdirection = -GetComponentInParent<EJPlayerFire>().firePosition.forward;

        // knockbackPos �ʱ�ȭ
        knockbackPos = transform.position + knockBackdirection ;
        // player�� knockbackPos�� ���ϴ� ����
        knockbackPos = knockbackPos - player.transform.position;

        // currentPosition �ʱ�ȭ
        currentPosition = transform.position;
        // ���� �������� ���� ��ġ���� �̵��� ��ġ ���ϴ� ����
        currentPosition = currentPosition - player.transform.position;
    }

    //���� ���� ����� ��� ã�Ƽ� �����
    //������ �Ѿ��� ��ü
    //Parent�� �������� ���?

    GameObject murderer ;

    public void Whoareyou()
    {
        //transform.localEulerAngles = new Vector3(cx, 0, 0);

        if(EJPSHP.instance.HP <= 0)
        {
            transform.parent = null;

            //���Ͻ��� ���� �̻��� ��

            //GetComponentInChildren<MeshRenderer>().enabled = false;
            transform.position = Vector3.Lerp(transform.position, EJPSHP.instance.murdererPos, 0.7f);
        }
    }

    public void PlayFireSFX()
    {
        fireSFX.Play();
    }

    public void PlayReloadSFX()
    {
        reloadSFX.Play();
    }
}
