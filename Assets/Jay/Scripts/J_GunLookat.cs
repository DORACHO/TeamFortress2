using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class J_GunLookat : MonoBehaviour
{
    private Vector3 targetPos;
    public Transform Players;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //targetPos = new Vector3(Players.position.x, transform.position.y, Players.position.z);
        transform.LookAt(targetPos);
        //플레이어의 위아래만(높이)만 따라가도록
    }
}
