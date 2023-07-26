using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJCConstPos : MonoBehaviour
{
    Vector3 originDir;
    bool hasChangedDir;

    // Start is called before the first frame update
    void Start()
    {
        originDir = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasChangedDir)
        {
            Vector3 newDir = changeDir(transform.forward);

            if (newDir != originDir)
            {
                transform.forward = newDir;
                hasChangedDir = true;

                // 감지하지 않았을 때 false로 해야하는데 지금 그냥 둠
                Invoke(nameof(ResetChangedDir), 3);
            }
        }
    }

    Vector3 changeDir(Vector3 dir)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 5))
        {
            return -transform.right;
        }
        return originDir;       
    }

    void ResetChangedDir()
    {
        hasChangedDir = false;
    }

}
