using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PathUnlocker : MonoBehaviour
{
    [SerializeField] private GameObject objectToMove;


    void Start()
    {
        if (objectToMove == null)
            enabled = false;        //no need to unlock a path
    }
    void FixedUpdate()
    {

        try
        {
            var x = Values.GetCurrentBoss().transform.position;

        }
        catch (Exception)
        {
            objectToMove.transform.position = new Vector3(-1000, -1000, -1000); //lo sposta in un punto lontano
            enabled = false;
        }

        
    }
}