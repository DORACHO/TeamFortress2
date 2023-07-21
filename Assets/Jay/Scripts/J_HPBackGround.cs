using MedicAI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class J_HPBackGround : MonoBehaviour
{
    public static J_HPBackGround Instance;

    private void Awake()
    {
        Instance = this; 
    }

    Image blinkingImage;
    Color originalColor;
    public Color blinkingColor;
    public bool isAUp;

    // Use this for initialization
    void Start()
    {
        blinkingImage = GetComponent<Image>();
        originalColor = blinkingImage.color;
      //  StartCoroutine(BlinkImage());
    }

    public IEnumerator BlinkImage()
    {
        Color color = originalColor;
        while (true)
        {
            if (isAUp)
            {
                color.a += Time.deltaTime;
                if(color.a >= 1 )
                {
                    isAUp = !isAUp;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                color.a -= Time.deltaTime;
                if(color.a <= 0 )
                {
                    isAUp = !isAUp;
                  
                    yield return new WaitForSeconds(0.1f);
                }
            }
            blinkingImage.color = color;
            yield return 0;
        }
    }

    Coroutine currCo = null;
    public void StartImage()
    {
        //만약에 시작되고 있는 코루틴 없다면
        if(currCo == null)
        {
            currCo = StartCoroutine(BlinkImage());
        }
    }
    public void EndImage()
    {
        //코루틴이 있을때 멈추게한다
        if(currCo != null )
        {
            StopCoroutine(currCo);
            currCo = null;
        }
        

    }
}