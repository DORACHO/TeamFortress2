using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//enemy가 맞았을 때만 나타나고 싶은 UI

public class EJEnemyHPForTest : MonoBehaviour
{

    public int hp = 150;
    public TextMeshProUGUI enemyHPText;
    
    // Start is called before the first frame update
    void Start()
    {
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
