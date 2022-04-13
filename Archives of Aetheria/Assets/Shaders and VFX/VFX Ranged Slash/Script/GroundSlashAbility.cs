using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlashAbility : MonoBehaviour
{
    public float speed = 30;
    public float slowDownRate = 0.01f;
    public float detectingDistance = 1f;
    public float destroyDelay = 5;
    public LayerMask detectionLayer;

    private Rigidbody rb;
    private bool stopped;
    private bool isAlreadyHit = false;

    private Transform firePoint;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(SlowDown());
        }
        else
            Debug.Log("No Rigidbody");

        Destroy(gameObject, destroyDelay);
    }

    public void SetFirepoint(Transform fp)
    {
        firePoint = fp;
    }

    private void FixedUpdate()
    {
        if (!stopped)
        {
            RaycastHit hit;
            Vector3 distance = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Raycast(distance, transform.TransformDirection(-Vector3.up), out hit, detectingDistance))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, firePoint.position.y, transform.position.z);
            }
            Debug.DrawRay(distance, transform.TransformDirection(-Vector3.up * detectingDistance), Color.red);
        }
    }

    private void Update()
    {
        if(!isAlreadyHit)
            DamageEnemy(-30);
    }

    IEnumerator SlowDown()
    {
        float t = 1;
        while (t > 0)
        {
            rb.velocity = Vector3.Lerp(Vector3.zero, rb.velocity, t);
            t -= slowDownRate;
            yield return new WaitForSeconds(0.1f);
        }

        stopped = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4);
    }

    public void DamageEnemy(float damageAmount)
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, 5, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > -360 && viewableAngle < 360)
                {
                    isAlreadyHit = true;
                    EnemyManager2 enemy = colliders[i].gameObject.GetComponent<EnemyManager2>();
                    enemy.healthbar.UpdateValue(damageAmount);
                }
            }
        }
    }
}
