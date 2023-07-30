using PatrolNChase;
using System;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace MedicAI
{

    public class J_Medic1 : MonoBehaviour
    {
        J_MedicHP medicHP;
        NavMeshAgent agent;
        GameObject target;
        Animator anim;
        public enum State
        {
            Idle,
            Chase,
            Attack,
            React,
            Die,
            Patrol,
            mustFollow,
        }
        public State state;
        public float attackRange =40;

        float currTime;
        float respawnTime = 3f;

        bool isClicked = false;
        //�����Ϳ��� �������� ����
        public Transform respawnPoint;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            medicHP = GetComponent<J_MedicHP>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case State.Idle: UpdateIdle(); break;
                case State.Chase: UpdateChase();  break;
                case State.Attack: UpdateAttack(); break;
                case State.Patrol: UpdatePatrol(); break;
                case State.Die: Respawn(); break;
            }
            if (Input.GetKeyDown(KeyCode.M)  )
            {
                isClicked = true;
                UpdateChase();
            }
        }

        public int targetIndex;
   

        void UpdateIdle()
        {
            //agent.SetDestination(J_PathManager. instance.points[0].position);
            target = GameObject.FindGameObjectWithTag("Player");
            //target = GameObject.Find("PlayerArm");
            //target = GameObject.FindWithTag("Player");
            if (target != null)
            {
                //�������·� �����ϰ�ʹ�
                state = State.Patrol;
                anim.SetTrigger("Move");
                agent.isStopped = false;
            }
        }

        void UpdateChase()
        {
            //agent.SetDestination(J_PathManager.instance.points[0].position);
            agent.destination = target.transform.position;

            //�������� ���� �Ÿ��� ���ʹ�

            float distance = Vector3.Distance(this.transform.position, target.transform.position);
            if (distance < attackRange)
            {
                state = State.Attack;
                anim.SetTrigger("Attack");
                agent.isStopped = true;
            }
            else if (distance > farDistance)
            {
                state = State.Patrol;
            }

        }
        //�÷��̾ 2�� ��ư�� ������ �÷��̾�� ���ϰ�ʹ�
        //State�� mustFollow 
        
        void UpdatePatrol()
        {
            //�������� �˰�ʹ�
            Vector3 pos = J_PathManager.instance.points[targetIndex].position;

            //���� ���� � ��ġ�� �������� �˰�ʹ�

            //�װ����� �̵��ϰ�ʹ�
            agent.SetDestination(pos);
            //print(pos);
            //1���� �����ߴٸ� �����Ѱ����� �ϰ�ʹ�

            pos.y = transform.position.y;
            float dist = Vector3.Distance(transform.position, pos);
            //�����ߴٸ� targetIndex�� 1������Ű��ʹ�
            if (dist <= agent.stoppingDistance)
            {
                targetIndex = (targetIndex + 1) % J_PathManager.instance.points.Length;
                //targetdex = (targetIndex + 1);

            }
            //���� �÷��̾ �� �νİŸ��ȿ� ���Դٸ�
            float dist2 = Vector3.Distance(transform.position, target.transform.position);
            if (dist2 < attackDistance || isClicked == true)
            {
                //�������·� �����ϰ�ʹ�
                state = State.Chase;
            }
        }
        float attackDistance = 10;
        public float farDistance = 20;
        public Transform Players;
        private Vector3 targetPos;

        void UpdateAttack()
        {
            //y���� �÷��̾�� �����ϰ��Ѵ�
            targetPos = new Vector3(Players.position.x, transform.position.y, Players.position.z);
            transform.LookAt(targetPos);
        }

       void UpdateDie()
        {
            
            state = State.Die;
            anim.SetTrigger("Die");
        }
        void Respawn()
        {
            currTime += Time.deltaTime;
            if (currTime <= respawnTime)
            {
                transform.position = respawnPoint.position;
                respawnPoint.eulerAngles = respawnPoint.eulerAngles;
                state = State.Idle;
                transform.position = respawnPoint.position;
                respawnPoint.rotation = respawnPoint.rotation;
            }
        }

        #region �ִϸ��̼� �̺�Ʈ�Լ��� ���� ȣ��Ǵ� �Լ���...
        // �ִϸ��̼� �̺�Ʈ�Լ��� ���� ȣ��Ǵ� �Լ���
        void OnAttack_Hit()
        {
            anim.SetBool("bAttack", false);
            float distance = Vector3.Distance(this.transform.position, target.transform.position);
            if (distance < attackRange)
            {
                print("Enemy -> PlayerHit");

            }
        }

        void OnAttack_Finished()
        {
            float distance = Vector3.Distance(this.transform.position, target.transform.position);
            if (distance > attackRange)
            {
                print("AttackFinished");
                state = State.Chase;
                anim.SetTrigger("Move");
                agent.isStopped = false;
            }
        }
        void OnAttackWait_Finished()
        {
            float distance = Vector3.Distance(this.transform.position, target.transform.position);
            if (distance > attackRange) // ���� ���� ���
            {
                state = State.Chase;
                anim.SetTrigger("Move");
                agent.isStopped = false;
            }
            else // ���� ������ �Ÿ�
            {
                anim.SetBool("bAttack", true);
            }
        }
        internal void OnReact_Finished()
        {
            // ���׼��� �������� Move���·� �����ϰ�ʹ�.
            state = State.Chase;
            anim.SetTrigger("Move");
            agent.isStopped = false;
        }
        #endregion
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name.Contains("Player"))
            {
                Destroy(collision.gameObject);
            }
            else
            {

            }
        }
        //���ݴ���
        public void DamageProcess(int damage = 1)
        {
            if (state == State.Die)
            {
                return;
            }
            agent.isStopped = true;
            medicHP.HP -= 1;
            if (medicHP.HP < 0)
            {

                state = State.Die;

                Destroy(gameObject, 5);
                anim.SetTrigger("Die");

                Collider col = GetComponentInChildren<Collider>();
                if (col)
                {
                    col.enabled = false;
                }
            }
            else
            {
                state = State.Chase;
                agent.isStopped = false;
                anim.SetTrigger("Move");
            }

        }



    }
}
