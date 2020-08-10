using System.Collections;
using System.Collections.Generic;
using Enemies;
using Player;
using UnityEditor;
using UnityEngine;
using Utility;


namespace Boss
{
    public class BossThree : MonoBehaviour, IBoss
    {
        //todo add status

        //todo spawna un nemico che poi alla sua morte viene spawnato un interactable per spawnare il boss vero
        //todo il boss sparisce per un tot e poi riappare
        //todo ha un aura che fa danno se a contatto oppure un'ombra

        [SerializeField] private int bossHealth;
        [SerializeField] private float projRateAttack = 0.2f;
        [SerializeField] private int projSpeedAttack = 10;

        [SerializeField] private BossEndTrigger pathUnlocker;

        private EnemyBase boss;
        private EnemyStatus status;
        private EnemyShooting bossShooting;


        private bool isBossInProgress;

        //Fake boss stuff
        private GameObject fakeBoss;
        private bool isFakeBossDead;

        //private SpriteRenderer spriteRenderer;
        private bool isAttacking;


        /*  Inizialmente identico a Nemico 3, quando subisce una certa quantità di danni cambia fase diventando molto più
            aggressivo (Attacca sparando un maggior numero di proiettili)
        */
        private void Start()
        {
            
            boss = GetComponent<EnemyBase>();
            status = boss.GetStatus();
            bossShooting = GetComponent<EnemyShooting>();
            //Set boss health
            status.SetHealth(bossHealth);
            status.SetMaxHealth(bossHealth);

            Values.SetCurrentBoss(gameObject);
            FindPathUnlocker();
            ActivatePathUnlocker();

            //Aura che fa danno a distanza. un'ombra tipo?
        }

        private void FixedUpdate()
        {
            
            isBossInProgress = Values.GetHasInteractedWithWinObject();

            if (isBossInProgress)
            {
                bossShooting.SetProjectileSpawnRate(projRateAttack);
                bossShooting.SetProjectileSpeed(projSpeedAttack);
            }

        }



        public void SetPhase()
        {
            throw new System.NotImplementedException();

        }

        public void FindPathUnlocker()
        {
            pathUnlocker = GameObject.Find(Values.TriggersParentName).GetComponentInChildren<BossEndTrigger>();
        }

        public void ActivatePathUnlocker()
        {
            pathUnlocker.enabled = true;
        }
    }
}