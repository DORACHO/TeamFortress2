using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerModelChangeState : MonoBehaviour
{
    public static EJPlayerModelChangeState instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OFFArmModel()
    {
        GameObject.FindWithTag("ArmBody").SetActive(false);
    }
    public void ONArmModel()
    {
        GameObject.FindWithTag("ArmBody").SetActive(true);
    }
    public void OFFFullBodyModel()
    {
        GameObject.FindWithTag("fullBodyPlayer").SetActive(false);
    }
    public void ONFullBodyModel()
    {
        GameObject.FindWithTag("fullBodyPlayer").SetActive(true);
    }
}
