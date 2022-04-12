using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class EnemyAI_newTrial : MonoBehaviour
{
    public NavMeshAgent agent;

    Animator anim;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkpoint;
    public bool walkpointSet;
    public float walkPointRange;
    public HealthBar playerHealthSlider;

    public float health;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealthSlider = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        timeBetweenAttacks = .5f; //give it a 0.5sec delay between attacksa
        health = 5;
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
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
        {
            walkpointSet = true;
            anim.SetBool("isRunning", true);
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
        anim.SetBool("isRunning", true);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        anim.SetBool("isRunning", false);



        Vector3 distanceToWalkPoint = transform.position - player.position;

        if (!alreadyAttacked)
        {

            //Melee attack if player is in range of 2

            if (distanceToWalkPoint.magnitude < 5f)
                anim.SetTrigger("meleeTrigger");

            //the player is out of melee range
            else if (distanceToWalkPoint.magnitude >= 5f)
            {
                int attackType = Random.Range(0, 2);

                //the attack type is ranged shot
                if (attackType == 0 && distanceToWalkPoint.magnitude > 10f)
                    anim.SetTrigger("shootTrigger");

                //the attack type is the jump attack
                else if (attackType == 1 && distanceToWalkPoint.magnitude <= 10f)
                    anim.SetTrigger("jumpAttackTrigger");

            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        transform.LookAt(player);
    }

    
}
