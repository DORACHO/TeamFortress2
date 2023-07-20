using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PatrolNChase
{
    public class J_PathManager1 : MonoBehaviour
    {
        public static J_PathManager1 instance;
        private void Awake()
        {
            instance = this;
        }

        public Transform[] points;
        // Start is called before the first frame update
        void Start()
        {
            points = new Transform[transform.childCount];
            // 태어날 때 points배열에 값을 채우고싶다.
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