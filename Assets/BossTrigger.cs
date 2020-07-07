using System;
using System.Collections;
using System.Collections.Generic;
using Boss;
using Enemies;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private GameObject rockBlocker;
    [SerializeField] private GameObject boss;


    
    private void OnTriggerEnter(Collider other)
    {
        
        //riattiva la roccia e fa un rumorone
        rockBlocker.GetComponent<MeshCollider>().enabled = true;
        rockBlocker.GetComponent<MeshRenderer>().enabled = true;

        //Attiva il boss
        boss.GetComponent<EnemyBase>().enabled = true;
        boss.GetComponent<BossOne>().enabled = true;
        boss.GetComponent<EnemyShooting>().enabled = true;
        boss.GetComponent<EnemyIntelligence>().enabled = true;
        boss.GetComponent<EnemyMovement>().enabled = true;

        //todo play sound
        
        this.enabled = false;
    }
}
