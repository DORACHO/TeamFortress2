using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

        //1. 텔레포트 기능 ( 입출구 모두 건설 시 사용 가능)
        // enterance에 trigger 된 캐릭터가 플레이어 팀이라면
        // exit으로 이동시킨다.
        // 한 번 이동하면, 10초 뒤에 가능하다.

        //2. 텔포킬
        // exit에 trigger된 캐릭터가 적이라면
        // 그리고 동시에 enterance에 trigger된 캐릭터가 플레이어 팀이라면
        // enemy가 죽는다.

        //변수
        //bool isTeam? isBothThere ,
        //gameObject enterance, exit,
        //float 출구로이동시간, delayTime, 
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
        //exit = GetComponent<EJPlayerConstruct>().publicTeleportExit;//null값이 들어간다.
        
    }

    //1. 텔레포트 기능 ( 입출구 모두 건설 시 사용 가능)
    // enterance에 trigger 된 캐릭터가 플레이어 팀이라면
    // exit으로 이동시킨다.
    // 한 번 이동하면, 10초 뒤에 가능하다.
    //EJConstructTeleportEnter gate = this.GetComponent<EJConstructTeleportEnter>();
    private void Update()
    {
        exit = GameObject.FindWithTag("TeleportExit");
    }
    private void OnTriggerEnter(Collider other)
    {
        print("텔레포트 입구에 들어갔습니다");
        print(exit);// 왜 계속 null인가..

        EJPlayerMove Player = other.GetComponent<EJPlayerMove>();
        Vector3 exitPos = exit.transform.position + Vector3.up *10;
        Player.Teleport(exitPos);

        //zoomInCam
        //현재 안되는 상태 왜?
        other.GetComponentInChildren<EJCameraRotate>().TeleportZoomIn();
        other.GetComponentInChildren<EJCameraRotate>().TeleportDissolve();
        
    }

}
