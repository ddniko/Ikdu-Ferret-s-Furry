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

    private Animator Animator;
    public bool Walking, Sprinting, Jumping, Grounded, Dodging, Attacking; 

    public PlayerData PlayerData; //Reference to scriptable object


    void Start() //Setting objects from scene when starting
    {
        rb = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }


    void Update() //Good for input handeling
    {
        //Constantly updating timer, used to make cooldowns
        DodgeTimer += Time.deltaTime;
        AttackTimer += Time.deltaTime;
        SequenceTimer += Time.deltaTime;

        Horizontal = Input.GetAxis("Horizontal"); //Returns 1 or -1 when pressing a or d
        Vertical = Input.GetAxis("Vertical"); //Returns 1 or -1 when pressing w or s
        MoveDir = new Vector3(Horizontal, 0, Vertical); //Makes vector from imput

        if (MoveDir != Vector3.zero && !Dodging) //Makes the character rotate around the y-axis to look the direction it is walking.
        {
            transform.rotation = Quaternion.LookRotation(MoveDir, Vector3.up);
        }

        //Checks if you are making a movement input, this makes sure i don't walk and sprint at the same time
        Sprint = Input.GetKey(KeyCode.LeftShift) ? true : false; //These are simplified versions of if-statements where we have "Condition ? consequent : alternative" good for when a if-statement doesn't change much

        if (Input.GetKeyDown(KeyCode.Space) && DodgeTimer >= PlayerData.DodgeCD) //tilføj attack og sequence parameter og cooldown
        {
            StartCoroutine(DodgeRoll());
        }

        if (Input.GetMouseButtonDown(0)) //casts a ray when you L-click on screen space, and if the ray hits the ground, it makes a point, truning the character towards the point
        {
            Debug.Log("Click");
            Ray MouseInput = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(MouseInput, out RaycastHit hit))
            {
                Debug.Log("Hit");;
                if (hit.collider.gameObject.layer == 6)
                {
                    Debug.Log("Hit Ground");
                    transform.rotation = Quaternion.LookRotation(hit.point, Vector3.up);
                    StartCoroutine(Attack());
                }
            }
        }


        //Måske læg i script for sig selv
        if (rb.velocity.magnitude >= 0.1 && rb.velocity.magnitude < PlayerData.SprintLimit)
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
        if (!Dodging)
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
        yield return new WaitForSeconds(2);//set time to however long the animation takes
        Dodging = false;
        Animator.SetBool("Dodging", Dodging);
        Debug.Log("Rolling end");
    }

    private IEnumerator Attack()
    {
        Attacking = true;
        Debug.Log("Attack Start");
        Animator.SetBool("Attacking", Attacking);
        yield return new WaitForSeconds(1);
        if (Input.GetMouseButton(0)) //Mangler rigtig måde at få sequence attacks
        {
            Debug.Log("Attack2 start");
        }
        Attacking = false;
    }



    /*
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
}
