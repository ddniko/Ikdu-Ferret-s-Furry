using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{

    public NavMeshAgent Agent;

    public Transform Player;

    public LayerMask Ground, PlayerL;

    public Vector3 Centerpoint;
    private bool CenterpointSet;
    public float PatrolRange;
    private float Speed;

    public float AttackCD;
    private bool Attacked;

    public float SightRange, AttackRange;
    public bool PlayerInSight, PlayerInAttack;

    private Rigidbody RB;

    private Animator Animator;

    public Collider AttackZone;



    void Awake()
    {
        Player = GameObject.Find("Player").transform;
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        Animator.SetBool("Walking", true);
        RB = GetComponent<Rigidbody>();
        Speed = Agent.speed;

    }
    void Update()
    {
        PlayerInSight = Physics.CheckSphere(transform.position, SightRange, PlayerL);
        PlayerInAttack = Physics.CheckSphere(transform.position, AttackRange, PlayerL);

        if (!PlayerInSight && !PlayerInAttack) Patroling();
        if (PlayerInSight && !PlayerInAttack) ChasePlayer();
        else Agent.speed = Speed;
        if (PlayerInSight && PlayerInAttack) AttackPlayer();
        if (!Attacked) RB.velocity = Vector3.zero;
    }

    private void Patroling()
    {
        Animator.SetBool("Walking", true);
        Animator.SetBool("Attacking", false);
        Debug.Log("patroling");
        if (!CenterpointSet) SearchWalkPoints();

        if (CenterpointSet)
            Agent.SetDestination(Centerpoint);

        Vector3 DistanceToPoint = transform.position - Centerpoint;

        if (DistanceToPoint.magnitude < 1f)
            CenterpointSet = false;
    }

    private void SearchWalkPoints()
    {
        float RandomZ = Random.Range(-PatrolRange, PatrolRange);
        float RandomX = Random.Range(-PatrolRange, PatrolRange);

        Centerpoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        if (Physics.Raycast(Centerpoint, -transform.up, 10f, Ground))
        {
            CenterpointSet = true;
        }
    }

    private void ChasePlayer()
    {
        Animator.SetBool("Walking", true);
        Animator.SetBool("Attacking", false);
        Agent.SetDestination(Player.position);
        Agent.speed = Speed * 2;
    }

    private void AttackPlayer()
    {
        Animator.SetBool("Walking", true);
        Animator.SetBool("Attacking", false);
        Agent.SetDestination(transform.position);

        if (!Attacked)
        {
            Animator.SetBool("Walking", false);
            Animator.SetBool("Attacking", true);

            StartCoroutine(Attack());

            //Debug.Log("didattack");
            Attacked = true;
            Invoke(nameof(ResetAttack), AttackCD);
        }
    }

    private void ResetAttack()
    {
        Attacked = false; 
    }

    IEnumerator Attack()
    {
        transform.LookAt(Player);
        yield return new WaitForSeconds(.3f);
        Debug.Log("Attack");

        AttackZone.enabled = true;
        RB.AddForce(transform.forward * 20, ForceMode.Impulse);
        yield return new WaitForSeconds(1);
        AttackZone.enabled = false;
        RB.velocity = Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.DrawWireSphere(transform.position, SightRange);
        Gizmos.DrawWireSphere(transform.position, PatrolRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Centerpoint, 10);
    }

}
