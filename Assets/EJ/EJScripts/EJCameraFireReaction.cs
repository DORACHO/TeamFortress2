using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJCameraFireReaction : MonoBehaviour
{
    //rebound의 회전 변수
    float cameraX;

    //나를 죽인 사람을 찾는 방법?
    GameObject murderer;

    // Start is called before the first frame update
    void Start()
    {
        // 현재 각도
        cameraX = transform.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = new Vector3(cameraX, 0, 0);

        //질문 부분
        if (EJPSHP.instance.HP < 0)
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            transform.position = Vector3.Lerp(transform.position, murderer.transform.position, 0.7f);
        }
    }

    // 더해 줄 각도
    float addX = 3;
    // 나중 각도
    float upX;
    // 원래 각도
    float originalX;

    public void CameraRebound()
    {
        //원래 각도
        originalX = cameraX;
        //현재 각도에서 위로 각도 전환한 뒤
        //나중 각도
        upX = cameraX + addX;
        cameraX += addX;

        if (cameraX > upX)
        {
            //Lerp로 보간해서 원래 각도로 내려오고 싶다.
            cameraX = Mathf.Lerp(cameraX, originalX, 0.7f);
        }
    }
}
