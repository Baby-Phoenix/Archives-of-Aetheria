using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager2 : AnimatorManager
{
    EnemyLocomotionManager2 enemyLocomotionManager2;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyLocomotionManager2 = GetComponent<EnemyLocomotionManager2>();
    }
    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyLocomotionManager2.enemyRigidBody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyLocomotionManager2.enemyRigidBody.velocity = velocity;
    }
    
    private void SetAttackToDone()
    {
        this.anim.SetBool("attackDone", true);
    }

    private void SetAttackToNotDone()
    {
        this.anim.SetBool("attackDone", false);
    }
}
