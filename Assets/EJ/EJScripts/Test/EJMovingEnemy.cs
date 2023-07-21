using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJMovingEnemy : MonoBehaviour
{
    float speed = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IEMoveManager());
    }

    IEnumerator IEMoveManager()
    {
        StartCoroutine(IEMove(Vector3.right, () => {
           
                StartCoroutine(IEMove(Vector3.left, () => {
                   
                        StartCoroutine(IEMoveManager());
                    
                }));
           
        }));

        yield return 0;
    }

    // �̵��ϴ� �ڷ�ƾ�Լ� �����ϰ�ʹ�.
    IEnumerator IEMove(Vector3 direction, System.Action callback)
    {
        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            transform.position += direction * speed * Time.deltaTime;
            yield return 0;
        }

        if (callback != null)
        {
            callback();
        }

    }
}
