using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//MP를 추가한다
public class MPManager : MonoBehaviour
{
    public static MPManager instance;
    private void Awake()
    {
        instance = this;
    }
    public TextMeshProUGUI textMP;
    public float mp;
    // Start is called before the first frame update
    void Start()
    {
       // MP = 150;
    }

    //public float MP
    //{
    //    get { return mp;}
    //    set
    //    {
    //        mp = value;
    //        textMP.text = "MP : " + (int)mp;
    //    }
    //}
    // Update is called once per frame
    void Update()
    {

    }
}
