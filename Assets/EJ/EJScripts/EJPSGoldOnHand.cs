using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EJPSGoldOnHand : MonoBehaviour
{
    public static EJPSGoldOnHand instance;

    public GameObject itemBulletBoxS;
    public GameObject itemBulletBoxM;
    public GameObject itemBulletBoxL;

    public int goldOnHand;

    public TextMeshProUGUI goldText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        goldOnHand = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GOLD = GOLD;
    }
   

    public int GOLD
    {
        get { return goldOnHand; }
        set 
        { 
            goldOnHand = value;
            goldText.text = $"{goldOnHand}";
        }
    }
}
