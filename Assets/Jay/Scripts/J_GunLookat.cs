using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_GunLookat : MonoBehaviour
{
    public GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        //GameObject target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        print(target);
        transform.LookAt(target.transform);
    }
}
