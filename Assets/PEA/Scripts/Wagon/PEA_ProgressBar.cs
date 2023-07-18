using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PEA_ProgressBar : MonoBehaviour
{
    private float distanceStartToEnd = 0f;       // 이전 웨이포인트부터 다음 웨이포인트까지의 거리
    private float distanceToEnd = 1f;            // 현재 위치에서 다음 웨이포인트까지의 거리
    private float progressByStep = 0f;
    private float progress = 0f;
    private int step = 0;

    private Slider progressBarSlider;
    private Transform wagonTr;
    
    public Transform[] wayPoints;

    private PEA_Wagon wagon;

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
            ShowProgress();
        }       

        progressByStep = 1 - (distanceToEnd / distanceStartToEnd);
        progress = progressByStep + step;
        progressBarSlider.value = progress;
    }

    private void FindWagon()
    {
        wagon = GameObject.FindWithTag("Wagon").GetComponent<PEA_Wagon>();
        wagonTr = wagon.GetComponent<Transform>();
    }

    private void ShowProgress()
    {
        if (wagon.IsBacking)
        {
            step = wagon.TargetIndex;
            distanceStartToEnd = Vector3.Distance(wayPoints[step].position, wayPoints[step + 1].position);
            distanceToEnd = Vector3.Distance(wayPoints[step + 1].position, wagonTr.position);
        }
        else
        {
            if (wagon.TargetIndex != 0)
            {
                step = wagon.TargetIndex - 1;
                distanceStartToEnd = Vector3.Distance(wayPoints[step].position, wayPoints[step + 1].position);
                distanceToEnd = Vector3.Distance(wayPoints[step + 1].position, wagonTr.position);
            }
        }
    }
}
