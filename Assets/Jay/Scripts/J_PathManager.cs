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
            // 태어날 때 points배열에 값을 채우고싶다.
            for (int i = 0; i < transform.childCount; i++)
            {
                points[i] = transform.GetChild(i);
            }

            //Transform[] temp = GetComponentsInChildren<Transform>();
            //List<Transform> list = new List<Transform>(temp);
            //// 만약 목록에 내 transform이 있다면
            //if (list.Contains(transform))
            //{
            //    // 제외하고싶다.
            //    list.Remove(transform);
            //}
            //points = list.ToArray();



            //points = new Transform[temp.Length - 1];
            //for (int i = 0, j = 0; i < temp.Length; i++)
            //{
            //    if (temp[i] == transform)
            //        continue;

            //    points[j] = temp[i];
            //    j++;

            //}
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}