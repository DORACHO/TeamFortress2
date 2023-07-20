using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerAttainItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("ItemBulletBoxS"))
        {
            Destroy(other.gameObject);
            EJPSGoldOnHand.instance.GOLD += 40;
        }
        if (other.gameObject.name.Contains("ItemBulletBoxM"))
        {
            Destroy(other.gameObject);
            EJPSGoldOnHand.instance.GOLD += 100;
        }
        if (other.gameObject.name.Contains("ItemBulletBoxL"))
        {
            Destroy(other.gameObject);
            EJPSGoldOnHand.instance.GOLD += 200;
        }
    }
}
