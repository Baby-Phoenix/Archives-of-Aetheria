using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] GameObject slashVFX_1;

    
    private void SlashAbilityVFX_1()
    {
        slashVFX_1.SetActive(true);
    }

    private void SlashAbilityVFX_2()
    {

    }

    private void SlashAbilityVFX_3()
    {

    }

    private void ResetVFX()
    {
        slashVFX_1.SetActive(false);
    }

}
