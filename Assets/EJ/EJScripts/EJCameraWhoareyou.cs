using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XXX
{
    public class EJCameraWhoareyou : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Whoareyou();
        }

        public void Whoareyou()
        {
            bool chaseCameraON = GetComponent<EJCameraRotate>().chaseCameraON;
            bool mainCameraON = GetComponent<EJCameraRotate>().mainCameraON;

            chaseCameraON = true;
            if (chaseCameraON)
            {
                print("whoareyou 실행 중입니다");

                Vector3 dir2Enemy = EJPSHP.instance.murdererPos - transform.position;
                dir2Enemy.Normalize();

                // transform.parent = null;
                Vector3 murdererCamPos = EJPSHP.instance.murdererPos + (-dir2Enemy + Vector3.up * 3);

                transform.position = Vector3.Lerp(transform.position, murdererCamPos, Time.deltaTime * 5);

                transform.LookAt(EJPSHP.instance.murdererPos);
                chaseCameraON = false;
            }

        }

    }
}
