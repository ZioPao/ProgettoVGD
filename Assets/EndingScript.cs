using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class EndingScript : MonoBehaviour
{
    // Start is called before the first frame update

    private void FixedUpdate()
    {
        try
        {
            var x = Values.GetCurrentBoss().transform.position;
        }
        catch (Exception)
        {
            RunTrigger();
        }
    }

    private void RunTrigger()
    {
        print("gioco finito");
        enabled = false;
    }

    
}
