using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody rb;
    private float Horizontal;
    private float Vertical;
    private Vector3 MoveDir;

    private bool Sprint;
    public bool Grounded;

    // Public bools to make animation parameters
    private Animator Animator;
    //public AnimatorController controller;
    public bool Walking; 
    public bool Sprinting;
    public bool Jumping;


    public PlayerData PlayerData; // Reference to scriptable object


    void Start() //Setting objects from scene when starting
    {
        rb = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }


    void Update() //Good for input handeling
    {
        Horizontal = Input.GetAxis("Horizontal"); //Returns 1 or -1 when pressing a or d
        Vertical = Input.GetAxis("Vertical"); //Returns 1 or -1 when pressing w or s
        MoveDir = new Vector3(Horizontal, 0, Vertical); //Makes vector from imput


        // Checks if you are making a movement input, this makes sure i don't walk and sprint at the same time
        Sprint = Input.GetKey(KeyCode.LeftShift) ? true : false; // These are simplified versions of if-statements where we have "Condition ? consequent : alternative" good for when a if-statement doesn't change much


        if (Input.GetKeyDown(KeyCode.Space) && Grounded)
        {
            Jumping = true;
            Jump();
        }



        // måske læg i script for sig selv
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
        if (!Jumping)
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

    private void OnCollisionEnter(Collision collision) // Skal måske ændres til Stay
    {
        if (collision.gameObject.tag == "Ground")
        {
            Grounded = true;
            if (Jumping)
            {
                Jumping = false;
            }
        }
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
}
