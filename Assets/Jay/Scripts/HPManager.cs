using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//MP를 추가한다
public class HPManager : MonoBehaviour
{
    public static HPManager instance;
    private void Awake()
    {
        instance = this;
    }
    public TextMeshProUGUI textHP;
    public int hp;
    public int maxhp = 185;
    // Start is called before the first frame update
    void Start()
    {
        HP = 125;
    }

    public int HP
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
