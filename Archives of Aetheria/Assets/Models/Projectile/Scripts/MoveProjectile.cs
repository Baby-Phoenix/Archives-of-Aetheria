using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    public float speed;
    public float fireRate;
    public GameObject muzzlePrefab;
    public GameObject impactPrefab;

    private void Start()
    {
        if (muzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;

            var psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
            if (psMuzzle != null)
            {
                Destroy(muzzleVFX, psMuzzle.main.duration);
            }
            else
            {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }
    }

    void Update()
    {
        if(speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("No Speed");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        speed = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (impactPrefab!= null)
        {
            var impactVFX = Instantiate(impactPrefab, pos, rotation);
                var psHit = impactVFX.GetComponent<ParticleSystem>();
                if (psHit != null)
                {
                    Destroy(impactVFX, psHit.main.duration);
                }
                else
                {
                    var psChild = impactVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(impactVFX, psChild.main.duration);
                }
        }
   
        Destroy(gameObject);
    }
}
