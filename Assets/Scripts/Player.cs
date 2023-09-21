using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UIElements;



public class Player : MonoBehaviour
{

    public float moveSpeed = 1;

    Animator animator = null;
    public GameObject playerObject = null;
    GameObject manager = null;
    public float HP = 100f;
    public float attackSeconds = 1f;

    public float hitSeconds = 1f;

    

    /*
    bool idleState = false;
    bool attackState = false;
    string state = "idle";
     */


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
    private void Update()
    {
        Move();
        Attack();
    }

    // Update is called once per frame
    private void Move()
    {
        if (state == State.Idle || state == State.Move)
        {
            float ver = Input.GetAxis("Vertical");
            float hor = Input.GetAxis("Horizontal");



            if (ver != 0 || hor != 0)
            {

                animator.SetBool("isMove", true);


                Vector3 moveVector = new Vector3();

                moveVector.z = ver * moveSpeed * Time.deltaTime;
                moveVector.x = hor * moveSpeed * Time.deltaTime;

                transform.position = transform.position + moveVector;

                Vector3 checkVector = new Vector3();
                if (moveVector != checkVector)
                {
                    transform.forward = moveVector;
                }

                //    transform.position += moveVector;
            }
            else
            {
                animator.SetBool("isMove", false);
            }
        }







    }

    void Attack()
    {

        StartCoroutine(CoAttack());

        
    }

    IEnumerator CoAttack()
    {

        if (state == State.Idle || state == State.Move)
        {
            bool att = Input.GetButtonDown("Fire1");

            if (state != State.Attack)
            {
                if (att == true)
                {
                    state = State.Attack;

                    animator.SetBool("isMove", false);

                    //            GetComponent<Animator>().SetTrigger("isAttack");
                    animator.SetTrigger("isAttack");
                    // �����ð����� ����Ѵ�.
                    yield return new WaitForSeconds(attackSeconds);
                    // yield return () null > 1 ������ ����Ѵ�
                    // new WaitForSeconds Ư�� �ʸ� ����Ѵ�.

                    state = State.Idle;
                }
            }
        }
               
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ZombieHand")
        {
            Hit();
        }
    }


    private void Hit()
    {
        StartCoroutine(CoHit());
    }

    IEnumerator CoHit()
    {

        if(state != State.Hit || state != State.Death)  // check this 
        {
            state = State.Hit;

            // HP�� ���δ�. hp = hp - ������ ���ݷ�
            HP = HP - 10;
            // �ִϸ��̼� ��� (�ǰ�)

            // ���� HP�� 0 ���϶�� �׾���Ѵ�.
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
        StartCoroutine(CoDeath());

    }

    IEnumerator CoDeath()
    {
        state = State.Death;
        animator.SetBool("isDeath", true);

        // display game over after certain time
        yield return new WaitForSeconds(2);

        // playerObject = GameObject.FindGameObjectWithTag("Player");
        manager.GetComponent<GUIManager>().ShowGameOver();
    }
}
