using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    Queue<GameObject> queue = new Queue<GameObject>();
    GameObject bullet;
    Transform pos;


    // Start is called before the first frame update
    void Start()
    {
        for(int i  = 0; i < 30; i++)
        {
            GameObject B = Instantiate(bullet, pos.position, Quaternion.identity);
            B.SetActive(false);
            queue.Enqueue(B);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireBullet()
    {
        GameObject b = queue.Dequeue();
        b.SetActive(true);
    }

    public void ReturnBullet(GameObject obj)
    {
        queue.Enqueue(obj);
        obj.SetActive(false);
    }
}
