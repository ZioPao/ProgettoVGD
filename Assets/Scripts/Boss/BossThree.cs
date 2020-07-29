﻿using System.Collections;
using System.Collections.Generic;
using Enemies;
using Player;
using UnityEditor;
using UnityEngine;
using Utility;


namespace Boss
{
    public class BossThree : MonoBehaviour
    {
        //todo add status

        //todo spawna un nemico che poi alla sua morte viene spawnato un interactable per spawnare il boss vero
        //todo il boss sparisce per un tot e poi riappare
        //todo ha un aura che fa danno se a contatto oppure un'ombra

        [SerializeField] private int bossHealth = 150;
        [SerializeField] private float timerPhase = 15f;
        [SerializeField] private float projRateDefense = 5f;
        [SerializeField] private int projSpeedDefense = 3;

        [SerializeField] private float projRateAttack = 0.2f;
        [SerializeField] private int projSpeedAttack = 10;
        private EnemyBase boss;
        private EnemyShooting bossShooting;
        private EnemySpawner enemySpawner;


        private bool isBossInProgress;
        //Fake boss stuff
        private GameObject fakeBoss;
        private bool isFakeBossDead;
        
        //private SpriteRenderer spriteRenderer;

        private bool isAttacking;


        /*  Inizialmente identico a Nemico 1, quando subisce una certa quantità di danni cambia fase diventando molto più
            aggressivo (Attacca sparando un maggior numero di proiettili)
        */
        private void Start()
        {
            //Il boss viene spawnato ma viene disattivato e nascosto

            boss = GetComponent<EnemyBase>();
            bossShooting = GetComponent<EnemyShooting>();
            //Set boss health
            boss.GetStatus().SetHealth(bossHealth);

            //Il boss spawna un altro nemico che quando viene ucciso spawna un'altra cosa interagibile
            // fakeBoss = PrefabUtility.InstantiatePrefab(new GameObject()) as GameObject;
            // isFakeBossDead = false;

            //isBossInProgress = false;

            //Init timer
            // TimerController.AddTimer(TimerController.BOSSTWOPHASE, timerPhase);
            // TimerController.AddCurrentTime(TimerController.BOSSTWOPHASE, 0f);
        }

        private void FixedUpdate()
        {

            if (isBossInProgress)
            {
                //Boss 3 stuff
            }
            else if (Values.GetHasInteractedWithWinObject())
            {
                isBossInProgress = true;
            }
          
            
            
            
            
            
            
            
            
            // if (!isFakeBossDead)
            // {
            //     UpdateFakeBossStatus();
            //     
            //     
            //     //In case the boss is finally dead, init the "you win" thingy
            //     PrefabUtility.InstantiatePrefab(new GameObject());
            //
            //
            //
            // }
            // else
            // {
            //     
            // }
            //
            //
        }

        // private bool UpdateFakeBossStatus()
        // {
        //     if (fakeBoss == null)
        //         isFakeBossDead = true;
        //     else
        //         isFakeBossDead = false;
        // }

        private void ManageTimer()
        {
            TimerController.RunTimer(TimerController.BOSSTWOPHASE);


            if (TimerController.GetCurrentTime()[TimerController.BOSSTWOPHASE] <= 0)
            {
                //Change phase
                if (isAttacking)
                    isAttacking = false;
                else
                    isAttacking = true;

                TimerController.ResetTimer(TimerController.BOSSTWOPHASE);
            }
        }


        private void SetPhase()
        {
            if (isAttacking)
            {
                bossShooting.SetProjectileSpawnRate(projRateAttack);
                bossShooting.SetProjectileSpeed(projSpeedAttack);
            }
            else
            {
                bossShooting.SetProjectileSpawnRate(projRateDefense);
                bossShooting.SetProjectileSpeed(projSpeedDefense);
            }
        }
    }
}