using System;
using System.Collections;
using System.Collections.Generic;
using Boss;
using Enemies;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private GameObject rockBlocker;
    [SerializeField] private GameObject bossSpawner;
    [SerializeField] private GameObject sun;


    
    private void OnTriggerEnter(Collider other)
    {
        
        //riattiva la roccia e fa un rumorone
        rockBlocker.GetComponent<MeshCollider>().enabled = true;
        rockBlocker.GetComponent<MeshRenderer>().enabled = true;

        //Attiva il boss
        bossSpawner.SetActive(true);

        //todo play sound

        //Diminuisce la luce

        sun.GetComponent<Light>().intensity = 0;
        this.enabled = false;
    }
}
