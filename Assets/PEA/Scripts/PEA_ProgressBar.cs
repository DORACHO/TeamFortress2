using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public PEA_Wagon wagon;

    // Start is called before the first frame update
    void Start()
    {
        progressBarSlider = GetComponent<Slider>();
        wagonTr = wagon.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
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

        progressByStep = 1 - (distanceToEnd / distanceStartToEnd);
        progress = progressByStep + step;
        progressBarSlider.value = progress;
    }
}
