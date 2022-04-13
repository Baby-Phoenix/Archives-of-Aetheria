using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCharacterCollision : MonoBehaviour
{

    public CharacterController characterControllerCollider = null;
    public CapsuleCollider characterCollider;
    public CapsuleCollider characterBlockerCollider;



    // Start is called before the first frame update
    void Start()
    {
        if(characterControllerCollider != null)
            Physics.IgnoreCollision(characterControllerCollider, characterBlockerCollider, true);
        else
            Physics.IgnoreCollision(characterCollider, characterBlockerCollider, true);
    }


}
