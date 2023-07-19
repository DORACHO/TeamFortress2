using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PatrolNChase
{
    public class J_PathManager : MonoBehaviour
    {
        public static J_PathManager instance;
        private void Awake()
        {
            instance = this;
        }

        public Transform[] points;
        // Start is called before the first frame update
        void Start()
        {
            points = new Transform[transform.childCount];
            // �¾ �� points�迭�� ���� ä���ʹ�.
            for (int i = 0; i < transform.childCount; i++)
            {
                points[i] = transform.GetChild(i);
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}