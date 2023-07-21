using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class J_HPBackGround : MonoBehaviour
{
    Image blinkingImage;
    Color originalColor;
    public Color blinkingColor;

    // Use this for initialization
    void Start()
    {
        blinkingImage = GetComponent<Image>();
        originalColor = blinkingImage.color;
        StartCoroutine(BlinkImage());
    }

    public IEnumerator BlinkImage()
    {
        while (true)
        {
            blinkingImage.color = blinkingColor;
            yield return new WaitForSeconds(0.5f);
            blinkingImage.color = originalColor;
            yield return new WaitForSeconds(0.5f);
        }
    }
}