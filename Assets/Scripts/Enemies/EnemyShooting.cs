using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Enemies
{
    public class EnemyShooting : MonoBehaviour
    {
        private Transform enemyTextureTransform;

        private List<Texture> projectilesSprites;


        //Old stuff
        private EnemyBehaviour enemyBehaviour;

        [SerializeField] public GameObject projectilePrefab;
        [SerializeField] private float timerWaitingMax = 5f;        //How much does the enemy have to wait between 2 projectiles
        [SerializeField] private int projectileRateMax = 100;
        [SerializeField] private int projectileSpeedMax = 50;


        private float timerWaitingLeft = 5f;


        //Projectile values
        private GameObject projectile;
        private MeshRenderer projectileMeshRenderer;
        private Vector3 direction;
        
        //bools

        private bool isEnemyShooting;

        public void Start()
        {
            //Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
            //laserLineRenderer.SetPositions(initLaserPositions);
            //laserLineRenderer.startWidth = laserWidth;
            //laserLineRenderer.useWorldSpace = false;        //local to the transform

            timerWaitingLeft = timerWaitingMax;
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
                var projectile = PrefabUtility.InstantiatePrefab(projectilePrefab) as GameObject;
                projectile.transform.position = enemyTextureTransform.position + new Vector3(0, 2.5f, 0);        //poco poco più in alto
                projectile.transform.rotation = enemyTextureTransform.rotation;

                isEnemyShooting = true;
                timerWaitingLeft = timerWaitingMax;        //reset timer
            }
            else if (timerWaitingLeft > 0)
            {
                isEnemyShooting = false;
                timerWaitingLeft -= Time.deltaTime; //decrease
            }

            
        }

        public void SetProjectileSpeed(int speed)
        {
            
            //Do something
        }

        public void SetProjectileSpawnRate(int rate)
        {
            //do something
        }
        public bool IsEnemyShooting(){
            return isEnemyShooting;
        }
    }
    

}