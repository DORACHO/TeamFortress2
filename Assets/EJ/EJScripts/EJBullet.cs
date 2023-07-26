using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBullet : MonoBehaviour
{
    //������ Ÿ���� ���󰡴� �Ѿ�
    public float speed = 15;

    EJCSentryGun ejcsentrygun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 sentry2EnemyDir = ejcsentrygun.sentryTarget().transform.position - transform.position;

        transform.position += sentry2EnemyDir * speed * Time.deltaTime;
    }
}
