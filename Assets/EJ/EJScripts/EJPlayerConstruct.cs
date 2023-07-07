using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerConstruct : MonoBehaviour
{
    //Construct ���
    public GameObject sentryGunFactory;
    public GameObject dispenserFactory;
    public GameObject teleportEnterFactory;
    public GameObject teleportExitFactory;

    public Transform constructPosition;

    //Construct ����â
    public GameObject UIConstructSelection;

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
            
            ONConsturctSelection();
                //SentryGun
                if (Input.GetKeyDown(KeyCode.Alpha1) && EJPSGoldOnHand.instance.GOLD >= 130)
                {
                    EJPSGoldOnHand.instance.GOLD -= 130;
                    GameObject centryGun = Instantiate(sentryGunFactory);
                    centryGun.transform.position = constructPosition.transform.position;
                    OFFConstructSelection();
                }

                //Dispenser
                if (Input.GetKeyDown(KeyCode.Alpha2) && EJPSGoldOnHand.instance.GOLD >= 100)
                {
                    EJPSGoldOnHand.instance.GOLD -= 100;
                    GameObject dispenser = Instantiate(dispenserFactory);
                    dispenser.transform.position = constructPosition.transform.position;
                    OFFConstructSelection();
                }

                //Teleport_enter
                if (Input.GetKeyDown(KeyCode.Alpha3) && EJPSGoldOnHand.instance.GOLD >= 50)
                {
                    EJPSGoldOnHand.instance.GOLD -= 150;
                    GameObject teleportEnter = Instantiate(teleportEnterFactory);
                    teleportEnter.transform.position = constructPosition.transform.position;
                    OFFConstructSelection();
                }

                //Teleport_exit
                if (Input.GetKeyDown(KeyCode.Alpha4) && EJPSGoldOnHand.instance.GOLD >= 50)
                {
                    EJPSGoldOnHand.instance.GOLD -= 150;
                    GameObject teleportExit = Instantiate(teleportExitFactory);
                    teleportExit.transform.position = constructPosition.transform.position;
                    OFFConstructSelection();
                }
            
            //Q�� �� �� �� ������ ����â�� ������.
            if (Input.GetKeyDown(KeyCode.Q))
            {
                OFFConstructSelection();
            }
        }
    }
    void ONConsturctSelection()
    {
        UIConstructSelection.gameObject.SetActive(true);
        print("q pressed");
    }

    void OFFConstructSelection()
    {
        UIConstructSelection.gameObject.SetActive(false);
    }
}
