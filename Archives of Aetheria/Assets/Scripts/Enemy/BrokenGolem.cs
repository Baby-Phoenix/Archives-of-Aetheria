using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGolem : MonoBehaviour
{
    [SerializeField] GameObject golemModel;

    private void InstantiateGolem()
    {
        Instantiate(golemModel, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
