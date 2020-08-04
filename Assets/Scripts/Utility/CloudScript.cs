using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    private Transform parent;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(Values.GetPlayerTransform());
        transform.RotateAround(transform.parent.position, Vector3.up, 0.2f * Time.deltaTime);
        //transform.position += transform.right * (Time.deltaTime * 2f);

    }
}
