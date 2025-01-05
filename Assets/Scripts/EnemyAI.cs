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

    public float AttackCD;
    private bool Attacked;

    public float SightRange, AttackRange;
    public bool PlayerInSight, PlayerInAttack;


    void Awake()
    {
        Player = GameObject.Find("Player").transform;
        Agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        PlayerInSight = Physics.CheckSphere(transform.position, SightRange, PlayerL);
        PlayerInAttack = Physics.CheckSphere(transform.position, AttackRange, PlayerL);

        if (!PlayerInSight && !PlayerInAttack) Patroling();
        if (PlayerInSight && !PlayerInAttack) ChasePlayer();
        if (PlayerInSight && PlayerInAttack) AttackPlayer();
    }

    private void Patroling()
    {
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
        else
        {
            //Debug.Log("cantfindground");
        }
    }

    private void ChasePlayer()
    {
        Agent.SetDestination(Player.position);
        //Debug.Log("chasing");
    }

    private void AttackPlayer()
    {
        Agent.SetDestination(transform.position);

        transform.LookAt(Player);
        //Debug.Log("attacking");
        if (!Attacked)
        {


            //kør attack metode her.


            //Debug.Log("didattack");
            Attacked = true;
            Invoke(nameof(ResetAttack), AttackCD);
        }
    }

    private void ResetAttack()
    {
        Attacked = false; 
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
