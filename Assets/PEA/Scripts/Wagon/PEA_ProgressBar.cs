using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PEA_ProgressBar : MonoBehaviour
{
    private float distanceStartToEnd = 0f;       // 이전 웨이포인트부터 다음 웨이포인트까지의 거리
    private float distanceStartToNow = 0f;       // 시작부터 지금까지 이동한 총 거리
    private float distanceToEnd = 1f;            // 현재 위치에서 다음 웨이포인트까지의 거리
    private float progressByStep = 0f;
    private float progress = 0f;
    private int step = 0;

    private Slider progressBarSlider;
    private Transform wagonTr;
    

    private PEA_Wagon wagon;
    private Transform[] wayPoints;
    private float[] waypointsDistances;

    // Start is called before the first frame update
    void Start()
    {
        progressBarSlider = GetComponent<Slider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1 && wagon == null)
        {
            FindWagon();
        } 
        else if (wagon != null)
        {
            ClacProgress();
            ShowProgress();
        }       

        //progressByStep = 1 - (distanceToEnd / distanceStartToEnd);
        //progress = progressByStep + step;
        //progressBarSlider.value = progress;
    }

    // 게임씬으로 들어오면 수레와 수레포인트들을 찾음
    private void FindWagon()
    {
        wagon = GameObject.FindWithTag("Wagon").GetComponent<PEA_Wagon>();
        GameObject wayPoint = GameObject.FindWithTag("Wagon_WayPoint");
        wayPoints = new Transform[wayPoint.transform.childCount];
        for (int i = 0; i< wayPoints.Length; i++)
	    {
            wayPoints[i] = wayPoint.transform.GetChild(i).transform;
		}
        wagonTr = wagon.GetComponent<Transform>();

        // 웨이포인트 다 찾으면 총 거리 구함
        CalcTotalDistance();
    }

    // 시작부터 끝까지 가는 길의 거리 구하기
    private void CalcTotalDistance()
    {
        waypointsDistances = new float[wayPoints.Length - 1];

        for(int i = 0; i < wayPoints.Length - 1; i++)
        {
            // 구간별 거리 구하기
            waypointsDistances[i] = Vector3.Distance(wayPoints[i].position, wayPoints[i + 1].position);
            
            // 구간별 거리 다 더해 총 거리 구함
            distanceStartToEnd += waypointsDistances[i];
        }
    }

    // 지금까지 온 거리 구하기
    private void CalcNowDistance()
    {
        distanceStartToNow = 0f;
        if(step > 0)
        {

            // 지금까지 지나온 구간들의 거리 더해주기
            for(int i = 0; i < step; i++)
            {
                distanceStartToNow += waypointsDistances[i];
            }
        }

        // 마지막 지나온 구간에서부터 이동한 거리 구해서 더해주기
        distanceStartToNow += Vector3.Distance(wayPoints[step].position, wagonTr.position);

    }

    // 진행률 계산
    private void ClacProgress()
    {
        if (wagon.IsBacking)
        {

            step = wagon.TargetIndex;
            CalcNowDistance();
            ////distanceStartToEnd = Vector3.Distance(wayPoints[step].position, wayPoints[step + 1].position);
            //distanceToEnd = Vector3.Distance(wayPoints[step + 1].position, wagonTr.position);
        }
        else
        {
            if (wagon.TargetIndex != 0)
            {
                step = wagon.TargetIndex - 1;
                CalcNowDistance();
                ////distanceStartToEnd = Vector3.Distance(wayPoints[step].position, wayPoints[step + 1].position);
                //distanceToEnd = Vector3.Distance(wayPoints[step + 1].position, wagonTr.position);
            }
        }

        progress = distanceStartToNow / distanceStartToEnd;
    }

    private void ShowProgress()
    {
        progressBarSlider.value = progress;
    }
}
