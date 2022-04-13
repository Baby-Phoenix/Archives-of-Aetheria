using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotionManager2 : MonoBehaviour
{
    EnemyManager2 enemyManager2;
    EnemyAnimatorManager2 enemyAnimatorManager2;
    NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidBody;

    public CharacterStats currentTarget;
    public LayerMask detectionLayer;

    public float distanceFromTarget;
    public float stoppingDistance = 1f;

    public float rotationSpeed = 15;


    private void Awake()
    {
        enemyManager2 = GetComponent<EnemyManager2>();
        enemyAnimatorManager2 = GetComponent<EnemyAnimatorManager2>();
        navmeshAgent = GetComponent<NavMeshAgent>();
        enemyRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        navmeshAgent.enabled = false;
        enemyRigidBody.isKinematic = false;
    }

    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager2.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > enemyManager2.minimumDetectionAngle && viewableAngle < enemyManager2.maximumDetectionAngle)
                {
                    currentTarget = characterStats;
                }
            }
        }

    }

    public void HandleMoveToTarget()
    {
        //if (enemyManager.isPerformingAction)
        //    return;

        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        //If performing an action, stop movement
        if (enemyManager2.isPerformingAction)
        {
            enemyAnimatorManager2.anim.SetBool("isRunning", false);
            navmeshAgent.enabled = false;
        }
        else
        {
            if (distanceFromTarget > stoppingDistance)
            {
                enemyAnimatorManager2.anim.SetBool("isRunning", true);
                enemyRigidBody.isKinematic = false;
            }
            else if (distanceFromTarget <= stoppingDistance)
            {
                enemyRigidBody.isKinematic = true;
                enemyAnimatorManager2.anim.SetBool("isRunning", false);
            }
        }

        HandleRotateTowardsTarget();
        //navmeshAgent.transform.localPosition = Vector3.zero;
        //navmeshAgent.transform.localRotation = Quaternion.identity;
    }

    public void HandleRotateTowardsTarget()
    {
        //Rotate manually
        if (enemyManager2.isPerformingAction)
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            //Quaternion targetRotation = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
        }
        //Rotate with pathfinding (navmesh)
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(navmeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyRigidBody.velocity; //navmeshAgent.velocity;

            navmeshAgent.enabled = true;
            navmeshAgent.SetDestination(currentTarget.transform.position);
            enemyRigidBody.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, navmeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
        }
    }
}
