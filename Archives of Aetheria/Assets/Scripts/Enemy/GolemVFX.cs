using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GolemVFX : MonoBehaviour
{
    [SerializeField] GameObject specialVFX;
    [SerializeField] Transform firePoint;
    GameObject projectileObj;

    void InstantiateSlam()
    {
        projectileObj = Instantiate(specialVFX, firePoint.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<ParticleSystem>().Play();
    }

    void ResetVFX()
    {
        Destroy(projectileObj, 5);
    }

    private void GolemSwing()
    {
        FindObjectOfType<AudioManager>().Play("GolemSwing");
    }

    private void GolemSlam()
    {
        FindObjectOfType<AudioManager>().Play("GolemSlam");
    }
}
