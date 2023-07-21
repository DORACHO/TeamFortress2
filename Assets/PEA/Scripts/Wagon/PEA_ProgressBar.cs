using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PEA_ProgressBar : MonoBehaviour
{
    private float distanceStartToEnd = 0f;       // ���� ��������Ʈ���� ���� ��������Ʈ������ �Ÿ�
    private float distanceStartToNow = 0f;       // ���ۺ��� ���ݱ��� �̵��� �� �Ÿ�
    private float distanceToEnd = 1f;            // ���� ��ġ���� ���� ��������Ʈ������ �Ÿ�
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

    // ���Ӿ����� ������ ������ ��������Ʈ���� ã��
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

        // ��������Ʈ �� ã���� �� �Ÿ� ����
        CalcTotalDistance();
    }

    // ���ۺ��� ������ ���� ���� �Ÿ� ���ϱ�
    private void CalcTotalDistance()
    {
        waypointsDistances = new float[wayPoints.Length - 1];

        for(int i = 0; i < wayPoints.Length - 1; i++)
        {
            // ������ �Ÿ� ���ϱ�
            waypointsDistances[i] = Vector3.Distance(wayPoints[i].position, wayPoints[i + 1].position);
            
            // ������ �Ÿ� �� ���� �� �Ÿ� ����
            distanceStartToEnd += waypointsDistances[i];
        }
    }

    // ���ݱ��� �� �Ÿ� ���ϱ�
    private void CalcNowDistance()
    {
        distanceStartToNow = 0f;
        if(step > 0)
        {

            // ���ݱ��� ������ �������� �Ÿ� �����ֱ�
            for(int i = 0; i < step; i++)
            {
                distanceStartToNow += waypointsDistances[i];
            }
        }

        // ������ ������ ������������ �̵��� �Ÿ� ���ؼ� �����ֱ�
        distanceStartToNow += Vector3.Distance(wayPoints[step].position, wagonTr.position);

    }

    // ����� ���
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
