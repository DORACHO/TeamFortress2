using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

        //1. �ڷ���Ʈ ��� ( ���ⱸ ��� �Ǽ� �� ��� ����)
        // enterance�� trigger �� ĳ���Ͱ� �÷��̾� ���̶��
        // exit���� �̵���Ų��.
        // �� �� �̵��ϸ�, 10�� �ڿ� �����ϴ�.

        //2. ����ų
        // exit�� trigger�� ĳ���Ͱ� ���̶��
        // �׸��� ���ÿ� enterance�� trigger�� ĳ���Ͱ� �÷��̾� ���̶��
        // enemy�� �״´�.

        //����
        //bool isTeam? isBothThere ,
        //gameObject enterance, exit,
        //float �ⱸ���̵��ð�, delayTime, 
public class EJCTeleportEnter : MonoBehaviour
{
    bool isTeam;
    bool isBothThere;

    GameObject targetObj;
    GameObject exit;

    public float journeyTime;
    public float delayTime;

    private void Start()
    {
        //exit = GetComponent<EJPlayerConstruct>().publicTeleportExit;//null���� ����.
        
    }

    //1. �ڷ���Ʈ ��� ( ���ⱸ ��� �Ǽ� �� ��� ����)
    // enterance�� trigger �� ĳ���Ͱ� �÷��̾� ���̶��
    // exit���� �̵���Ų��.
    // �� �� �̵��ϸ�, 10�� �ڿ� �����ϴ�.
    //EJConstructTeleportEnter gate = this.GetComponent<EJConstructTeleportEnter>();
    private void Update()
    {
        exit = GameObject.FindWithTag("TeleportExit");
    }
    private void OnTriggerEnter(Collider other)
    {
        print("�ڷ���Ʈ �Ա��� �����ϴ�");
        print(exit);// �� ��� null�ΰ�..

        EJPlayerMove Player = other.GetComponent<EJPlayerMove>();
        Vector3 exitPos = exit.transform.position + Vector3.up *10;
        Player.Teleport(exitPos);

        //zoomInCam
        //���� �ȵǴ� ���� ��?
        other.GetComponentInChildren<EJCameraRotate>().TeleportZoomIn();
        other.GetComponentInChildren<EJCameraRotate>().TeleportDissolve();
        
    }

}
