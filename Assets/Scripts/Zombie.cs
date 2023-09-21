using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    Animator animator = null;
    public GameObject playerObject = null;

    // playerObject = GameObject.FindGameObjectWithTag("Player");
    public float checkDistance = 5f;
    public float attackDistance = 1.0f;
    public float HP = 100f;
    public float hitSeconds = 1f;
    public float attackSeconds = 1f;

    State state = State.Idle;

    public enum State
    {
        Idle, Move, Attack, Hit, Death
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Idle();
    }

    // 대기, 추격(이동), 공격, 피격, 죽음
    void Idle()
    {
        animator.SetBool("isMove", false);

        Vector3 playerVector = playerObject.transform.position;
        Vector3 zombieVector = transform.position;
        float playerZombieDistance = Vector3.Distance(playerVector, zombieVector);
        
        // distance between zombie and player is smaller than certain threashold.
        if(playerZombieDistance < attackDistance)
        {
            Attack();
        }
        else if (playerZombieDistance < checkDistance)
        {
            // 추격한다
            Move();
        }
        else // 다시 걸리가 멀어졌다면 거리가 멀었다면 set move to false
        {
            animator.SetBool("isMove", false);
        }
    }

    void Move()
    {
        // move only when idle
        if (state == State.Idle || state == State.Move)
        {
            GetComponent<NavMeshAgent>().enabled = true;
            state = State.Move;
            animator.SetBool("isMove", true);
            GetComponent<NavMeshAgent>().destination = playerObject.transform.position;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("isAttack");        
    }


    IEnumerator CoAttack()
    {

        if (state == State.Idle || state == State.Move)
        {
            
            // stop movement by turnning off the components.
            GetComponent<NavMeshAgent>().enabled = false;
            
                    state = State.Attack;

                    animator.SetBool("isMove", false);

                    //            GetComponent<Animator>().SetTrigger("isAttack");
                    animator.SetTrigger("isAttack");
                    // 일정시간동안 대기한다.
                    yield return new WaitForSeconds(attackSeconds);
                    // yield return () null > 1 프레임 대기한다
                    // new WaitForSeconds 특정 초를 대기한다.

                    state = State.Idle;

        }

    }


    private void OnCollisionEnter(Collision collision)  
    {
        // HP가 깍인다. hp = hp - 상대방의 공격력
        // 애니메이션 출력 (피격)
        //collision.gameObject.  ---  getComponent on added components
        if(collision.gameObject.tag == "PlayerSword")
        {
            Hit();
        }
        
        // 만약 HP가 0 이하라면 죽어야한다.
    }

    private void Hit()
    {
        StartCoroutine(CoHit());
    }

    IEnumerator CoHit()
    {

        if (state != State.Hit && state != State.Death)  // check this 
        {
            state = State.Hit;

            // HP가 깍인다. hp = hp - 상대방의 공격력
            HP = HP - 10;
            // 애니메이션 출력 (피격)

            // 만약 HP가 0 이하라면 죽어야한다.
            if (HP <= 0)
            {
                Death();
            }
            else
            {
                animator.SetTrigger("isHit");

                yield return new WaitForSeconds(hitSeconds);


                state = State.Idle;
            }
        }


    }


    private void Death()
    {
        animator.SetBool("isDeath", true);
        state = State.Death;

    



    }


}
