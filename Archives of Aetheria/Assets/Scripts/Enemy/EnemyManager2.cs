using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager2 : MonoBehaviour
{
    EnemyLocomotionManager2 enemyLocomotionManager2;
    EnemyAnimatorManager2 enemyAnimatorManager2;
    public bool isPerformingAction;
    
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;

    [Header("A.I Settings")]
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;

    public float currentRecoveryTime = 0;

    private void Awake()
    {
        enemyLocomotionManager2 = GetComponent<EnemyLocomotionManager2>();
        enemyAnimatorManager2 = GetComponent<EnemyAnimatorManager2>();
    }
    void Update()
    {
        HandleRecoveryTimer();
    }

    private void FixedUpdate()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction()
    {
        if (enemyLocomotionManager2.currentTarget != null)
        {
            enemyLocomotionManager2.distanceFromTarget = Vector3.Distance(enemyLocomotionManager2.currentTarget.transform.position, transform.position);

        }

        if (enemyLocomotionManager2.currentTarget == null)
        {
            enemyLocomotionManager2.HandleDetection();
        }
        else if (enemyLocomotionManager2.distanceFromTarget > enemyLocomotionManager2.stoppingDistance)
        {
            enemyLocomotionManager2.HandleMoveToTarget();
        }
        else if (enemyLocomotionManager2.distanceFromTarget <= enemyLocomotionManager2.stoppingDistance)
        {
            if (!isPerformingAction)
            {
                Vector3 direction = (enemyLocomotionManager2.currentTarget.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
            }

            enemyAnimatorManager2.anim.SetBool("isRunning", false);
            AttackTarget();
        }
    }

    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }

    private void AttackTarget()
    {
        if (isPerformingAction)
            return;

        if (currentAttack == null)
        {
            GetNewAttack();
        }
        else
        {
            isPerformingAction = true;
            currentRecoveryTime = currentAttack.recoveryTime;
            enemyAnimatorManager2.PlayTargetAnimation(currentAttack.actionAnimation, true);
            currentAttack = null;
        }
    }

    private void GetNewAttack()
    {
        Vector3 targetsDirection = enemyLocomotionManager2.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
        enemyLocomotionManager2.distanceFromTarget = Vector3.Distance(enemyLocomotionManager2.currentTarget.transform.position, transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemyLocomotionManager2.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                   && enemyLocomotionManager2.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }

        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemyLocomotionManager2.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                   && enemyLocomotionManager2.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (currentAttack != null)
                        return;

                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        currentAttack = enemyAttackAction;
                    }
                }
            }
        }
    }
}
