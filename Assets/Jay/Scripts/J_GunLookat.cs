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
        transform.LookAt(target.transform);
        //플레이어의 위아래만(높이)만 따라가도록
    }
}
