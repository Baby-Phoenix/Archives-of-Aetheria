using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSphere : MonoBehaviour
{
    private float rot = 0;
    public float rotSpeed = 0.1f;

    private void Start()
    {
        
    }
    void Update()
    {
        transform.rotation.Set(0, 0, rot, 0);
        rot += rotSpeed;
    }
}
