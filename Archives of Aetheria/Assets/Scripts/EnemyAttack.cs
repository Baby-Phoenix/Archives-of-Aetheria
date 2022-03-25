using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttack : MonoBehaviour
{
   public Animator anim;
   public Slider playerHealth;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Spider _ Attack"))
            {
                //playerHealth.value = other.gameObject.GetComponent<PlayerMovement>().health--;
                Debug.Log("Player hit");
            }
            
        }
    }

}
