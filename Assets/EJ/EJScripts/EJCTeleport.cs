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
public class EJCTeleport : MonoBehaviour
{
    bool isTeam;
    bool isBothThere;
    public bool isPlayerEnter;

    public GameObject targetObj;
    public GameObject enterance;
    public GameObject exit;

    public float journeyTime;
    public float delayTime;

    private void Start()
    {
    }

    //1. �ڷ���Ʈ ��� ( ���ⱸ ��� �Ǽ� �� ��� ����)
    // enterance�� trigger �� ĳ���Ͱ� �÷��̾� ���̶��
    // exit���� �̵���Ų��.
    // �� �� �̵��ϸ�, 10�� �ڿ� �����ϴ�.
    //EJConstructTeleportEnter gate = this.GetComponent<EJConstructTeleportEnter>();

    private void OnTriggerEnter(Collider other)
    {
        //bool�� �������� ���?
        //if (other.name.Contains("Enemy"))
        

            targetObj = other.gameObject;

        //EJConstructTeleportExit exit = other.GetComponent<EJConstructTeleportExit>();

        //if(exit.isTeam == true)
        //{

        //}
        EJPlayerMove pm = other.GetComponent<EJPlayerMove>();
        pm.Teleport(exit.transform.position);
       // teleport();



    }


    //private void teleport()
    //{

    //    targetObj.transform.position = exit.transform.position;


    //}
}
