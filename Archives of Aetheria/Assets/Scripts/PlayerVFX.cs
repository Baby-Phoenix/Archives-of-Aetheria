using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] Camera cam;

    //Melee
    [SerializeField] GameObject meleeVFX_1;
    [SerializeField] GameObject meleeVFX_2;
    [SerializeField] GameObject meleeVFX_3;

    //Ability
    [SerializeField] GameObject slashVFX_1;
    [SerializeField] GameObject slashVFX_2;
    [SerializeField] GameObject slashVFX_3;

    //Special Ability
    [SerializeField] GameObject specialVFX;
    [SerializeField] Transform firePoint;
    private GroundSlashAbility groundSlashScript;


    private void Start()
    {
        meleeVFX_1.SetActive(false);
        meleeVFX_2.SetActive(false);
        meleeVFX_3.SetActive(false);

        slashVFX_1.SetActive(false);
        slashVFX_2.SetActive(false);
        slashVFX_3.SetActive(false);
    }

    private void MeleeSlashVFX_1()
    {
        meleeVFX_1.SetActive(true);
    }

    private void MeleeSlashVFX_2()
    {
        meleeVFX_2.SetActive(true);
    }

    private void MeleeSlashVFX_3()
    {
        meleeVFX_3.SetActive(true);
    }

    private void SlashAbilityVFX_1()
    {
        slashVFX_1.SetActive(true);
    }

    private void SlashAbilityVFX_2()
    {
        slashVFX_2.SetActive(true);
    }

    private void SlashAbilityVFX_3()
    {
        slashVFX_3.SetActive(true);
    }

    private void SpecialAbilityVFX()
    {
        ShootProjectile();
    }

    void ShootProjectile()
    {
        if (cam != null)
        {
            InstantiateProjectile();
        }
    }

    void InstantiateProjectile()
    {
        var projectileObj = Instantiate(specialVFX, firePoint.position, Quaternion.identity) as GameObject;

        groundSlashScript = projectileObj.GetComponent<GroundSlashAbility>();
        RotateToDestination(projectileObj, true);
        projectileObj.GetComponent<Rigidbody>().velocity = transform.forward * groundSlashScript.speed;

    }

    void RotateToDestination(GameObject obj, bool onlyY)
    {
        var rotation = Quaternion.LookRotation(this.transform.forward);

        if (onlyY)
        {
            rotation.x = 0;
            rotation.z = 0;
        }

        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }

    private void ResetVFX()
    {
        meleeVFX_1.SetActive(false);
        meleeVFX_2.SetActive(false);
        meleeVFX_3.SetActive(false);

        slashVFX_1.SetActive(false);
        slashVFX_2.SetActive(false);
        slashVFX_3.SetActive(false);
    }

}
