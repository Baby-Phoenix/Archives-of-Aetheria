using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyHand"|| collision.gameObject.tag == "Enemy")
        {
            if (gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                Debug.Log("Enemy hit within if");
                collision.gameObject.GetComponent<EnemyAI_newTrial>().health--;
            }
            Debug.Log("Enemy hit");
        }
    }
}
