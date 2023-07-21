using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJCameraFireReaction : MonoBehaviour
{
    //rebound�� ȸ�� ����
    float cameraX;

    //���� ���� ����� ã�� ���?
    GameObject murderer;

    // Start is called before the first frame update
    void Start()
    {
        // ���� ����
        cameraX = transform.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = new Vector3(cameraX, 0, 0);

        //���� �κ�
        if (EJPSHP.instance.HP < 0)
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            transform.position = Vector3.Lerp(transform.position, murderer.transform.position, 0.7f);
        }
    }

    // ���� �� ����
    float addX = 3;
    // ���� ����
    float upX;
    // ���� ����
    float originalX;

    public void CameraRebound()
    {
        //���� ����
        originalX = cameraX;
        //���� �������� ���� ���� ��ȯ�� ��
        //���� ����
        upX = cameraX + addX;
        cameraX += addX;

        if (cameraX > upX)
        {
            //Lerp�� �����ؼ� ���� ������ �������� �ʹ�.
            cameraX = Mathf.Lerp(cameraX, originalX, 0.7f);
        }
    }
}
