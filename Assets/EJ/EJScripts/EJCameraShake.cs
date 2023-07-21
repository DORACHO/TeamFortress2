
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Update()
	{
		if (shakeDuration > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;
		}
	}


	/*public void KnockBack()
    {
        //위로 올라갔다가, 스무스하게 내려가고 싶다.
        transform.position = knockbackPos;
        cx += knockbackRotation;

        //만약에 나의 위치가 KnockbackPos와 가까워졌다면
        float distance = Vector3.Distance(transform.position, player.transform.position + knockbackPos);

        if (distance < 0.1f)
        {
            transform.position = Vector3.Lerp(knockbackPos, currentPosition, knockbackSpeed * Time.deltaTime);
            cx = Mathf.Lerp(cx, cx - knockbackRotation, 0.8f);
            isKnockBack = false;
        }
                
    }*/
}