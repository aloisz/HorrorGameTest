using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOfIce : MonoBehaviour
{
    
    
    void Update()
    {
        transform.Rotate(new Vector3(50,50,50) * (Time.deltaTime * 2));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
