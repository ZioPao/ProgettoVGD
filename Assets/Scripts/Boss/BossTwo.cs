using System.Collections;
using System.Collections.Generic;
using Enemies;
using Player;
using UnityEngine;
using Utility;


namespace Boss
{

    public class BossTwo : MonoBehaviour
    {


        [SerializeField] private int bossHealth = 150;
        [SerializeField] private float timerPhase = 15f;
        [SerializeField] private float projRateDefense = 5f;
        [SerializeField] private int projSpeedDefense = 3;

        [SerializeField] private float projRateAttack = 0.2f;
        [SerializeField] private int projSpeedAttack = 10;
        private EnemyBase boss;
        private EnemyShooting bossShooting;
        private EnemySpawner enemySpawner;
        //private SpriteRenderer spriteRenderer;
        private Light aura;
        
        private bool isAttacking;
        
        
        
        
    /*  Inizialmente identico a Nemico 1, quando subisce una certa quantità di danni cambia fase diventando molto più
        aggressivo (Attacca sparando un maggior numero di proiettili)
    */
        private void Start()
        {

            boss = GetComponent<EnemyBase>();
            bossShooting = GetComponent<EnemyShooting>();
            aura = GetComponentInChildren<Light>();
            
            //Set boss health
            boss.GetStatus().SetHealth(bossHealth);

            
            
            //Init timer
            TimerController.AddTimer(TimerController.BOSSTWOPHASE, timerPhase);
            TimerController.AddCurrentTime(TimerController.BOSSTWOPHASE, 0f);
            
            

            
        }
        
        private void FixedUpdate()
        {
           ManageTimer();

           SetAura();
           SetPhase();

        }

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

        private void SetAura()
        {
            if (isAttacking)
                aura.color = Color.red;
            else
                aura.color = Color.blue;

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
