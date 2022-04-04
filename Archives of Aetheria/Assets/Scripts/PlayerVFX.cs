using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] GameObject slashVFX_1;
    [SerializeField] GameObject slashVFX_2;
    [SerializeField] GameObject slashVFX_3;

    private void Start()
    {
        slashVFX_1.SetActive(false);
        slashVFX_2.SetActive(false);
        slashVFX_3.SetActive(false);
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

    private void ResetVFX()
    {
        slashVFX_1.SetActive(false);
        slashVFX_2.SetActive(false);
        slashVFX_3.SetActive(false);
    }

}
