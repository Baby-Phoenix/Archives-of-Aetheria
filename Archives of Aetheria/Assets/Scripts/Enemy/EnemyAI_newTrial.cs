using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class EnemyAI_newTrial : MonoBehaviour
{
    public NavMeshAgent agent;
    public StatusBarManager healthBar;

    Animator anim;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkpoint;
    public bool walkpointSet;
    public float walkPointRange;
    

    public float health;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public Transform hand;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthBar = gameObject.GetComponentInChildren<StatusBarManager>();
        healthBar.SetMaxValue(10f);
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        timeBetweenAttacks = .5f; //give it a 0.5sec delay between attacksa
        health = 5;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hand.position, attackRange);
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

    }

    private void Patrolling()
    {
        if (!walkpointSet) SearchWalkPoint();

        if (walkpointSet) agent.SetDestination(walkpoint);

        Vector3 distanceToWalkPoint = transform.position - walkpoint;

        if (distanceToWalkPoint.magnitude < 2.5f) walkpointSet = false;

        anim.SetBool("Attack", true);
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
        {
            walkpointSet = true;
            
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
        anim.SetBool("Attack", false);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 distanceToWalkPoint = transform.position - player.position;

        if (!alreadyAttacked)
        {
            //attack code here
            anim.SetBool("Attack", true);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        transform.LookAt(player);
    }

    private void CheckHit()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(hand.position, attackRange, whatIsPlayer);

        foreach(Collider player in hitPlayer)
        {
            player.gameObject.GetComponent<Player>().healthBar.unit--;
            player.gameObject.GetComponent<Player>().healthBar.UpdateValue();
        }
    }

    
}
