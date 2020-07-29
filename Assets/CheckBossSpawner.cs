using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utility;

public class CheckBossSpawner : MonoBehaviour
{
    private GameObject fakeBoss;
    
    void Awake()
    {
        fakeBoss = GameObject.Find("FakeBossLevel3");
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        try
        {
            var tmp = fakeBoss.transform.position.x;
            //it'll fail when the fake boss is dead
        }
        catch (Exception)
        {
            //Spawn the object that'll start the real boss battle
            PrefabUtility.InstantiatePrefab(new GameObject());
            
            
            //gameObject.GetComponent<BossSpawner>().enabled = true;        
            enabled = false;        //Disable checkbossspawner
        }
    }
}
