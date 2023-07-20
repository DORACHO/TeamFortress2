using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class J_ObjectPool : MonoBehaviour
{
    public static J_ObjectPool instance;

    public GameObject bullet = null; // 생성할 총알
    public Transform pos;
    public Queue<GameObject> bulletqueue = new Queue<GameObject>();//객체를 저장할 큐

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        for(int i = 0; i< 20; i++)
        {
            GameObject b = Instantiate(bullet, pos.position, Quaternion.identity);
            bulletqueue.Enqueue(b);
            b.SetActive(false);
        }
    }
    public void Fire()
    {
        GameObject b = bulletqueue.Dequeue();
        b.transform.position = pos.position;
        b.transform.rotation = pos.rotation;
        b.SetActive(true);
    }
    
    public void Fire_Finished(GameObject b)
    {
        bulletqueue.Enqueue(b);
        b.SetActive(false);
    }
    // Update is called once per frame
    private void Update()
    {
        
    }

}
