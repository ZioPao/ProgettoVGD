using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(0, 2, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        Values.AddHealth(25);
        Destroy(gameObject);
        //when players touch it automatically add tot health to him
    }
}    
