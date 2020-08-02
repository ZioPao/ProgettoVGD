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
        
        //TODO QUESTO MOTORE DI MERDA ESEGUE LE COSE QUANDO GLI PARE 
        if (objectToMove == null)
            enabled = false; //no need to unlock a path
    }

    void FixedUpdate()
    {
        
        
        
        try
        {
            var x = Values.GetCurrentBoss().transform.position;
        }
        catch (Exception)
        {

            //todo così rompe gli altri livelli
            if (objectToMove)
            {
                objectToMove.transform.position = new Vector3(-1000, -1000, -1000); //lo sposta in un punto lontano
                Values.SetCanSave(true); //permette di salvare nuovamente
                gameObject.SetActive(false); 
            }
            else
            {
                enabled = false;
            }

        }
    }

    public void RunTrigger()
    {
        objectToMove.transform.position = new Vector3(-1000, -1000, -1000); //lo sposta in un punto lontano
        Values.SetCanSave(true); //permette di salvare nuovamente
        gameObject.SetActive(false);
    }
}