using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.AI;
//MP
//level 스킬 숙련도  마력 소모
//거리와 스킬은 구분
//스킬 판단기준
//쿨타임
//AI level


//거리와 스킬 구분하자


public class J_Hiller : MonoBehaviour
{
    public float distance = 5f;
    public float speed = 6.1f;
    private GameObject player;
    private Vector3 targetPosition;
    public float jumpPower = 5f;
    public float gravity = -9.81f;
    float yVelocity;
    public float maxDeviation = 2f;
    public float oscillationFrequency = 1f;
    public float oscillationMagnitude = 2f;
    public float jumpProbability = 10f;
    public float backwardDuration = 1f;
    public float jumpInterval = 10f;
    public int maxHP = 185;
    public float timeInterval = 0.1f;
    private float elapsedTime = 0f;
    //public int baseHPIncresase = 1;
    public int maxMultiplier = 10;
    public float multiplierDistanceThreshold = 2f;

    NavMeshAgent agent;
    CharacterController cc;
    float timer = 0f;
    bool isMovingBackward = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        cc = gameObject.GetComponent<CharacterController>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        targetPosition = player.transform.position;


    }

    // Update is called once per frame
    void Update()
    {
        //중력의 힘이 y속도에 작용
        yVelocity += gravity * Time.deltaTime;

        if (cc.isGrounded && isMovingBackward && Random.value < jumpProbability)
        {
            yVelocity = jumpPower;
        }

        Vector3 dir = player.transform.position - transform.position;
        dir.Normalize();
        dir.y = 0f;

        agent.destination = player.transform.position;
        Vector3 normalizedDirection = dir.normalized;
        //Vector3 deviation = Random.insideUnitCircle.normalized * Random.Range(0f, maxDeviation);

        targetPosition = player.transform.position - (normalizedDirection * distance);

        float offset = Mathf.Sin(timer * oscillationFrequency) * oscillationMagnitude;
        Vector3 offsetVector = transform.right * offset;
        Vector3 targetPositionWithOscillation = targetPosition + offsetVector;

        Vector3 velocity = (targetPositionWithOscillation - transform.position).normalized * speed;
        velocity.y = yVelocity;
        cc.Move(velocity * Time.deltaTime);

        timer += Time.deltaTime;

        if (timer >= jumpInterval)
        {
            timer = 0f;
            if (Random.value < jumpProbability)
            {
                yVelocity = jumpPower;
            }
        }
        if (Vector3.Distance(transform.position, player.transform.position) <= distance)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= timeInterval)
            {
                elapsedTime = 0f;

                //float currentDistance = Vector3.Distance(transform.position, player.transform.position);
                //int HPIncrease = baseHPIncresase;

                //if (currentDistance > multiplierDistanceThreshold)
                {
                    //float multiplier = Mathf.Lerp(1f, maxMultiplier, 1f - currentDistance / multiplierDistanceThreshold);
                    //HPIncrease = Mathf.RoundToInt(baseHPIncresase * multiplier);
                }


                //HPManager.instance.HP += HPIncrease;

                //만약에 HP가 maxHP 커지면
                // if(HPManager.instance.HP > maxHP)
                {
                    // HPManager.instance.HP = maxHP;  
                }
                //멈춰라(다시 maxHP 값으로 셋팅)
                //if(HPManager.instance.HP < maxHP)
                //{
                //    HPManager.instance.HP++;
                //}
            }

        }




        //private void OnCollisionEnter(Collision collision)
        //{
        //    if(collision.gameObject.name.Contains("Player"))
        //    {
        //        PlayerHP php = collision.gameObject.GetComponent<PlayerHP>();
        //        php.SetHP(php.GetHP() - 1);

        //        if(php.GetHP() < 0)
        //        {

        //        }
        //        else
        //        {
        //            Destroy(gameObject);

        //            HPManager.instance.hp++;
        //        }
        //    }
        //}
    }
}
