using System.Collections;
using System.Collections.Generic;
using Logic.Enemy;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public LineRenderer laserLineRenderer;
    private EnemyBehaviour enemyBehaviour;
    private Transform playerCamera; 
    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;

    private float timerShooting = 1f;       //
    private float timerShootingLeft = 1f;
    
    void Start()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.useWorldSpace = false;        //local to the transform

        enemyBehaviour = transform.GetComponentInParent<EnemyBehaviour>();
        playerCamera = GameObject.Find("Camera_Main").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyBehaviour.GetIsPlayerInView())
        {
            //print(timerShootingLeft);
            timerShootingLeft -= Time.deltaTime;        //Decrease timer. Shoot only once every sec

            
            if (Mathf.RoundToInt(timerShootingLeft) < 0)
            {
                //Spara e disattiva
                laserLineRenderer.SetPositions(new Vector3[2] { transform.forward, transform.right });
                if (Physics.Linecast(transform.position, playerCamera.position))
                {
                    laserLineRenderer.SetPosition(0, new Vector3(0, 0, 0)); ;
                    laserLineRenderer.SetPosition(1, playerCamera.position);
                    laserLineRenderer.enabled = true;
                }

                timerShootingLeft = timerShooting;      //Reset timer

            }
            else
            {
                laserLineRenderer.enabled = false;
            }
           
        }
        else
        {
            laserLineRenderer.enabled = false;
            timerShootingLeft = timerShooting;      //Reset timer

        }
    }
}
