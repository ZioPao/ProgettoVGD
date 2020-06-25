using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Logic.Enemy
{
    public class EnemyShooting : MonoBehaviour
    {
        private const string CameraMainName = "Camera_Main";

        private Transform enemyTextureTransform;

        private List<Texture> projectilesSprites;


        //Old stuff
        private EnemyBehaviour enemyBehaviour;

        [SerializeField] public GameObject projectilePrefab;


        private float timerWaitingLeft = 5f;
        private float timerWaitingMax = 5f;


        //Projectile values
        private GameObject projectile;
        private Rigidbody projectileRigidbody;
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

            enemyBehaviour = transform.GetComponentInParent<EnemyBehaviour>();
            enemyTextureTransform = transform.parent;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (!enemyBehaviour.GetIsPlayerInView()) return;
            
            //Può sparare
            if (timerWaitingLeft <= 0)
            {
                //Spawna projectile
                //il nemico si ferma un secondo e poi continua a muoversi dopo aver sparato
                projectile = PrefabUtility.InstantiatePrefab(projectilePrefab) as GameObject;
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
        
            
        public bool IsEnemyShooting(){
            return isEnemyShooting;
        }
    }
    

}