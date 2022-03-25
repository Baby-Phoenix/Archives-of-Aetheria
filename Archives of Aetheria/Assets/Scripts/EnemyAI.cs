using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    Animator anim;

   public int Health = 5;

    private float _originalMaxSpeed = 0;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (agent)
            _originalMaxSpeed = agent.speed;
    }



    void Update()
    {
        int turnOnSpot;

        Vector3 cross = Vector3.Cross(transform.forward, agent.desiredVelocity.normalized);
        float horizontal = (cross.y < 0) ? -cross.magnitude : cross.magnitude;
        horizontal = Mathf.Clamp(horizontal * 2.32f, -2.32f, 2.32f);

        if (agent.desiredVelocity.magnitude < 1.0f && Vector3.Angle(transform.forward, agent.desiredVelocity) > 20.0f)
        {
            agent.speed = 0.01f;
            turnOnSpot = (int)Mathf.Sign(horizontal);
            anim.SetBool("isRunning", false);
        }
        else
        {
            agent.speed = _originalMaxSpeed;
            anim.SetBool("isRunning", true);
            turnOnSpot = 0;
        }

        //anim.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
        //anim.SetFloat("Vertical", agent.desiredVelocity.magnitude, 0.1f, Time.deltaTime);
        //anim.SetInteger("TurnOnSpot", turnOnSpot);

        float distance = Vector3.Distance(transform.position, target.position);

        if(distance > 3.76)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("isAttacking"))
            {
                agent.updatePosition = true;
                agent.SetDestination(target.position);
            }
            anim.SetBool("isAttacking", false);

        }
        else
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20);
            agent.updatePosition = false;
            anim.SetBool("isAttacking", true);
            anim.SetBool("isRunning", false);
        }
    }
}
