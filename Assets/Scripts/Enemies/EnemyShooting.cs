﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Enemies
{
    [Serializable]

    public class EnemyShooting : MonoBehaviour
    {
        private Transform enemyTextureTransform;

        private List<Texture> projectilesSprites;
        

        [SerializeField] public GameObject projectilePrefab;
        [SerializeField] private float timerProjectileRate = 5f;        //How much does the enemy have to wait between 2 projectiles
        [SerializeField] private int projectileSpeed = 10;


        private float timerWaitingLeft;


        //Projectile values
        private GameObject projectile;
        private Vector3 direction;
        
        //bools

        private bool isEnemyShooting;

        public void Awake()
        {
            //Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
            //laserLineRenderer.SetPositions(initLaserPositions);
            //laserLineRenderer.startWidth = laserWidth;
            //laserLineRenderer.useWorldSpace = false;        //local to the transform

            timerWaitingLeft = timerProjectileRate;
            enemyTextureTransform = transform;
        }


        private void FixedUpdate()
        {

            if (timerWaitingLeft > 0 && isEnemyShooting)
            {
                timerWaitingLeft -= Time.deltaTime; //decrease costantly 
            }
        }

        public void Shoot()
        {

            if (timerWaitingLeft <= 0)
            {
                var projectile = Instantiate(projectilePrefab) as GameObject;
                projectile.name = "Projectile";
                
                ProjectileScript projScript = projectile.GetComponent<ProjectileScript>();
                projScript.SetSpeed(projectileSpeed);
                
                projectile.transform.position = enemyTextureTransform.position + new Vector3(0, 2.5f, 0);        //poco poco più in alto
                projectile.transform.rotation = enemyTextureTransform.rotation;
                    
                isEnemyShooting = true;
                timerWaitingLeft = timerProjectileRate;        //reset timer
            }
            else if (timerWaitingLeft > 0)
            {
                isEnemyShooting = false;
                timerWaitingLeft -= Time.deltaTime; //decrease
            }

            
        }

        public void SetProjectileSpeed(int speed)
        {
            projectileSpeed = speed;

        }

        public void SetProjectileSpawnRate(float rate)
        {
            timerProjectileRate = rate;
        }
        public bool IsEnemyShooting(){
            return isEnemyShooting;
        }
    }
    

}