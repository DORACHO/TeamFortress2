using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//MP�� �߰��Ѵ�
public class HPManager : MonoBehaviour
{
    public static HPManager instance;
    private void Awake()
    {
        instance = this;
    }
    public TextMeshProUGUI textHP;
    public float hp;
    public float maxhp = 185;
    // Start is called before the first frame update
    void Start()
    {
        HP = 125;
    }

    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            textHP.text = "HP : " + (int)hp;    
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
