using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerConstruct : MonoBehaviour
{
    //Construct ���
    public GameObject sentryGunFactory;
    public GameObject teleportEnterFactory;
    public GameObject teleportExitFactory;

    public Transform constructPosition;

    //Construct ����â
    public GameObject UIConstructSelection;
    public GameObject ConstPos;

    public GameObject UITextsentrygunO;
    public GameObject UITextsentrygunX;
    public GameObject UITextenterO;
    public GameObject UITextenterX;
    public GameObject UITextexitO;
    public GameObject UITextexitX;

    bool isPositionFixed;

    //���º���
    public EJPlayerAnim stateMgr;

    //Tool ����
    public GameObject ToolGun;
    public GameObject ToolWrench;


    //teleport �Ǽ� �� ����� ����
    public GameObject publicTeleportExit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Construct ����â�� ������.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMgr.ChangeState(EJPlayerAnim.State.Construct);
            Gun2Wrench();

            if (UIConstructSelection.activeSelf)
            {
                OFFConstructSelection();
                Wrench2Gun();
            }
            else
            {
                ONConsturctSelection();                 
            }
        }

        if (UIConstructSelection.activeSelf)
        {           
            //SentryGun
            if (Input.GetKeyDown(KeyCode.Alpha1) && EJPSGoldOnHand.instance.GOLD >= 130)
            {
                ConstPos.SetActive(true);
                //isPositionFixed = false;

                if (ConstPos.activeSelf && isPositionFixed)
                {
                    ConstPos.SetActive(false);
                    EJPSGoldOnHand.instance.GOLD -= 130;
                    GameObject centryGun = Instantiate(sentryGunFactory);
                    centryGun.transform.position = constructPosition.transform.position;
                    OFFConstructSelection();
                    Wrench2Gun();
                    UITextsentrygunX.SetActive(false);
                    UITextsentrygunO.SetActive(true);
                }
                else 
                {
                    ConstPos.SetActive(true);
                    isPositionFixed = true;
                }
            }

            //Teleport_enter
            if (Input.GetKeyDown(KeyCode.Alpha3) && EJPSGoldOnHand.instance.GOLD >= 50)
            {
                EJPSGoldOnHand.instance.GOLD -= 150;
                GameObject teleportEnter = Instantiate(teleportEnterFactory);
                teleportEnter.transform.position = constructPosition.transform.position;
                OFFConstructSelection();
                Wrench2Gun();
                UITextenterX.SetActive(false);
                UITextenterO.SetActive(true);
            }

            //Teleport_exit
            if (Input.GetKeyDown(KeyCode.Alpha4) && EJPSGoldOnHand.instance.GOLD >= 50)
            {
                EJPSGoldOnHand.instance.GOLD -= 150;
                GameObject teleportExit = Instantiate(teleportExitFactory);
                publicTeleportExit = teleportExit;
                print(publicTeleportExit);

                teleportExit.transform.position = constructPosition.transform.position;
                OFFConstructSelection();
                Wrench2Gun();
                UITextexitX.SetActive(false);
                UITextexitO.SetActive(true);
            }
        }
    }


    void ONConsturctSelection()
    {
        UIConstructSelection.SetActive(true);
    }

    void OFFConstructSelection()
    {
        UIConstructSelection.gameObject.SetActive(false);
    }

    public void Gun2Wrench()
    {
        print("wrench�� �ٲ�����ϴ�");
        OFFGun();
        ONWrench();
    }

    public void Wrench2Gun()
    {
        print("Gun���� �ٲ�����ϴ�");
        ONGun();
        OFFWrench();
    }

    public void ONGun()
    {
        ToolGun.SetActive(true);
    }

    public void OFFGun()
    {
        ToolGun.SetActive(false);
    }

    public void ONWrench()
    {
        ToolWrench.SetActive(true);
    }

    public void OFFWrench()
    {
        ToolWrench.SetActive(false);
    }

}
