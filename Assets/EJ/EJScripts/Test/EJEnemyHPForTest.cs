using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//enemy�� �¾��� ���� ��Ÿ���� ���� UI

public class EJEnemyHPForTest : MonoBehaviour
{
    public static EJEnemyHPForTest instance;

    public int hp = 150;
    public TextMeshProUGUI enemyHPText;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        ENEMY_HP = ENEMY_HP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int ENEMY_HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            enemyHPText.text = $"{value}";
        }
    }
}
