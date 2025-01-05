using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody rb;
    private float Horizontal;
    private float Vertical;
    private Vector3 MoveDir;
    private bool Sprint;

    private float AttackTimer;
    private float SequenceTimer;
    private float DodgeTimer;
    private float ComboBuffer;

    private Animator Animator;
    public bool Walking, Sprinting, Jumping, Grounded, Dodging, Attacking, Sequence; 

    public PlayerData PlayerData; //Reference to scriptable object

    private int AttackNum = 1;

    private RaycastHit Point;

    public GameObject Sword, Staff;

    public LayerMask Ground;
    private Collider SwordCol;


    void Start() //Setting objects from scene when starting
    {
        rb = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        SwordCol = Sword.GetComponent<Collider>();
    }


    void Update() //Good for input handeling
    {
        //Constantly updating timer, used to make cooldowns
        DodgeTimer += Time.deltaTime;
        AttackTimer += Time.deltaTime;
        SequenceTimer += Time.deltaTime;
        ComboBuffer += Time.deltaTime;

        Horizontal = Input.GetAxis("Horizontal"); //Returns 1 or -1 when pressing a or d
        Vertical = Input.GetAxis("Vertical"); //Returns 1 or -1 when pressing w or s
        MoveDir = new Vector3(Horizontal, 0, Vertical); //Makes vector from imput

        if (MoveDir != Vector3.zero && !Dodging && !Attacking) //Makes the character rotate around the y-axis to look the direction it is walking.
        {
            transform.rotation = Quaternion.LookRotation(MoveDir, Vector3.up);
        }

        //Checks if you are making a movement input, this makes sure i don't walk and sprint at the same time
        Sprint = Input.GetKey(KeyCode.LeftShift) ? true : false; //These are simplified versions of if-statements where we have "Condition ? consequent : alternative" good for when a if-statement doesn't change much

        if (Input.GetKeyDown(KeyCode.Space) && DodgeTimer >= PlayerData.DodgeCD && !Attacking) //tilføj attack og sequence parameter og cooldown
        {
            StartCoroutine(DodgeRoll());
        }

        if (Input.GetMouseButtonDown(0) && !Dodging) //casts a ray when you L-click on screen space, and if the ray hits the ground, it makes a point, truning the character towards the point
        {
            //Debug.Log("Click");
            Ray MouseInput = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(MouseInput, out RaycastHit hit, Mathf.Infinity, Ground,QueryTriggerInteraction.Collide))
            {
                //Debug.Log("Hit Ground");
                SequenceTimer = 0;
                //transform.rotation = Quaternion.LookRotation(hit.point, Vector3.up); // Endnu en parameter
                Point = hit;
                if (AttackTimer >= PlayerData.AttackCD)
                {
                    Attack();
                    AttackTimer = 0;
                }
            }
        }

        if (SequenceTimer >= PlayerData.AttackSequence && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95 || Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95 && Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            Animator.SetBool("Attacking", false);
            Attacking = false;
            SwordCol.enabled = false;
        }


        //Måske læg i script for sig selv
        if (rb.velocity.magnitude >= 0.2 && rb.velocity.magnitude < PlayerData.SprintLimit)
        {
            Walking = true;
            Sprinting = false;
            Animator.SetBool("Walking", Walking);
            Animator.SetBool("Running", Sprinting);
        }
        else if (rb.velocity.magnitude >= PlayerData.SprintLimit)
        {
            Sprinting = true;
            Walking = false;
            Animator.SetBool("Walking", Walking);
            Animator.SetBool("Running", Sprinting);
        }
        else
        {
            Walking = false;
            Sprinting = false;
            Animator.SetBool("Walking", Walking);
            Animator.SetBool("Running", Sprinting);
        }
    }
    
    private void FixedUpdate() //Everything doing something to a rigidbody, make do it in fixed
    {
        if (!Dodging && !Attacking)
        {
            if (Sprint) //Gives rigidbody velocity in vector direction and some speed multiplier, different when walking or sprinting
            {
                rb.velocity = MoveDir.normalized * PlayerData.SprintSpeed;
            }
            else
            {
                rb.velocity = MoveDir.normalized * PlayerData.WalkSpeed;
            }
        }
    }

    private IEnumerator DodgeRoll() //CHANGE WHEN ANIMATION IS READY
    {
        DodgeTimer = 0;
        rb.velocity = Vector3.zero; //Stops momentum
        Dodging = true;
        Animator.SetBool("Dodging", Dodging);
        Debug.Log("Rolling start");

        //something dont take damage

        rb.AddForce(transform.forward * PlayerData.DodgeForce, ForceMode.Impulse); //Pushes the character TO BE REMADE WHEN ANIMATION IS DONE
        yield return new WaitForSeconds(1.458f);//set time to however long the animation takes
        Dodging = false;
        Animator.SetBool("Dodging", Dodging);
        Debug.Log("Rolling end");
    }
    private void Attack()
    {
        transform.rotation = Quaternion.LookRotation(Point.point-transform.position, Vector3.up);
        Animator.SetBool("Attacking", true);
        rb.velocity = Vector3.zero;
        Attacking = true;
        SwordCol.enabled = true;
    }









    /*
    private IEnumerator Attack1()
    {
        Attacking = true;
        Debug.Log("Attack Start");
        Animator.SetBool("Attacking", Attacking);


        //StopCoroutine("Attack1");
        Debug.Log("Didn't stop");
        //Åben hit area

        yield return new WaitForSeconds(1);

        //Luk hit area

        if (SequenceTimer < PlayerData.AttackSequence) //CHANGE  //Mangler rigtig måde at få sequence attacks
        {
            Debug.Log("Attack2 start");
            StartCoroutine(Attack2());
            StopCoroutine("Attack1");
        }
        Debug.Log("notattack2");
        Attacking = false;
        Animator.SetBool("Attacking", Attacking);
    }

    private IEnumerator Attack2()
    {

        //Åben hit area

        yield return new WaitForSeconds(2);

        if (SequenceTimer < PlayerData.AttackSequence) //Mangler rigtig måde at få sequence attacks
        {
            //Debug.Log("Attack2 start");
            StartCoroutine(Attack3());
            StopCoroutine(Attack2());
        }

        //Luk hit area
        Attacking = false;
        //Animator.SetBool("Attacking", Attacking);
    }

    private IEnumerator Attack3()
    {

        //Åben hit area

        yield return new WaitForSeconds(1);

        //Luk hit area


        Attacking = false;
        //Animator.SetBool("Attacking", Attacking);

    }

    private void Attack()
    {

    }



    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Grounded = false;
        }
    }

        public void Jump() //enten corutine eller method, så skal jumping være false, enten når corutine er færdig eller når jeg rammer jorden igen
    {
        rb.AddForce(Vector3.forward * PlayerData.JumpForce + Vector3.up * PlayerData.JumpUpForce);   //Tag vektor3.forward og gang med en force og tilføj en opadgående force
    }

            if (Input.GetKeyDown(KeyCode.Space) && Grounded)
        {
            Jumping = true;
            //Jump();
        }

        private void OnMouseDown()
    {
        Debug.Log("Click");
        Ray MouseInput = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(MouseInput, out RaycastHit hit))
        {
            Debug.Log("Hit");
            if (hit.collider.gameObject.layer == Ground)
            {
                Debug.Log("Hit Ground");
            }
        }
    }
    */

    /*
if (SequenceTimer < PlayerData.AttackSequence && WhatAttack(AttackNum) && !Dodging)
{
    if (AttackNum == 1)
    {
        Debug.Log("attack1");
        Animator.SetBool("Atk" + AttackNum, true);
    }
    else
    {
        Debug.Log("attack" + AttackNum);
        Animator.SetBool("Atk" + AttackNum, true);
        Animator.SetBool("Atk" + (AttackNum - 1), false);
    }
    AttackNum++;
    //AttackNum = Mathf.Clamp(AttackNum,1,3);
    Debug.Log(AttackNum);
}
else if (SequenceTimer > PlayerData.AttackSequence || AttackNum >= 4) //CHANGE den gør ikke attack 3
{
    AttackNum = 1;
    Animator.SetBool("Atk1",false);
    Animator.SetBool("Atk2", false);
    Animator.SetBool("Atk3", false);
}*/
    /*
    if (SequenceTimer <= 0 && !IsAttacking())
    {
        Attack();
    }

    if (IsAttacking() && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7)
    {
        ComboBuffer = -PlayerData.ComboBuffer;
    }

    if (!IsAttacking() && ComboBuffer <= 0)
    {
        Animator.SetBool("Atk" + AttackNum, false);
        AttackNum = 1;
    }*/

    /*
//attack animation logic
if (SequenceTimer <= 0 && !Dodging)
{
    Attack();
    Animator.SetBool("Atk1", true);
    Animator.SetBool("Atk2", false);
    Animator.SetBool("Atk3", false);
}
else if (SequenceTimer <= 0 && !Dodging && Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
{
    Attack();
    Animator.SetBool("Atk2", true);
    Animator.SetBool("Atk1", false);
    Animator.SetBool("Atk3", false);
}
else if (SequenceTimer <= 0 && !Dodging && Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
{
    Attack();
    Animator.SetBool("Atk3", true);
    Animator.SetBool("Atk2", false);
    Animator.SetBool("Atk1", false);
}*/
}
