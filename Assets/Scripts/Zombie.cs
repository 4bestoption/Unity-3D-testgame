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

    // ´ë±â, Ãß°Ý(ÀÌµ¿), °ø°Ý, ÇÇ°Ý, Á×À½
    void Idle()
    {
        animator.SetBool("isMove", false);

        Vector3 playerVector = playerObject.transform.position;
        Vector3 zombieVector = transform.position;
        float playerZombieDistance = Vector3.Distance(playerVector, zombieVector);

        // distance between zombie and player is smaller than certain threashold.
        if (playerZombieDistance < attackDistance)
        {
            Attack();
        }
        else if (playerZombieDistance < checkDistance)
        {
            // Ãß°ÝÇÑ´Ù
            Move();
        }
        else // ´Ù½Ã °É¸®°¡ ¸Ö¾îÁ³´Ù¸é °Å¸®°¡ ¸Ö¾ú´Ù¸é set move to false
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
            // ÀÏÁ¤½Ã°£µ¿¾È ´ë±âÇÑ´Ù.
            yield return new WaitForSeconds(attackSeconds);
            // yield return () null > 1 ÇÁ·¹ÀÓ ´ë±âÇÑ´Ù
            // new WaitForSeconds Æ¯Á¤ ÃÊ¸¦ ´ë±âÇÑ´Ù.

            state = State.Idle;

        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        // HP°¡ ±ïÀÎ´Ù. hp = hp - »ó´ë¹æÀÇ °ø°Ý·Â
        // ¾Ö´Ï¸ÞÀÌ¼Ç Ãâ·Â (ÇÇ°Ý)
        //collision.gameObject.  ---  getComponent on added components
        if (collision.gameObject.tag == "PlayerSword")
        {
            Hit();
        }

        // ¸¸¾à HP°¡ 0 ÀÌÇÏ¶ó¸é Á×¾î¾ßÇÑ´Ù.
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

            // HP°¡ ±ïÀÎ´Ù. hp = hp - »ó´ë¹æÀÇ °ø°Ý·Â
            HP = HP - 10;
            // ¾Ö´Ï¸ÞÀÌ¼Ç Ãâ·Â (ÇÇ°Ý)

            // ¸¸¾à HP°¡ 0 ÀÌÇÏ¶ó¸é Á×¾î¾ßÇÑ´Ù.
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
