using MedicAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�¾ �� ü���� �ִ�ü���� �ǰ��ϰ�ʹ�
//�ѿ� ������ ü���� 1�����ϰ�ʹ�
//ü���� ����Ǹ� UI�� ǥ���ϰ�ʹ�
public class J_MedicHP : MonoBehaviour
{
    int hp;
    private readonly int maxHp = 150;

    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            //ü���� ����Ǹ� UI�� ǥ���ϰ�ʹ�

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
