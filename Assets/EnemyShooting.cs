using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public LineRenderer laserLineRenderer;
    public SpottingScript spotting;
    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;

    private float timerShooting = 2f;       //
    private float timerShootingLeft = 2f;
    
    void Start()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.useWorldSpace = false;        //local to the transform
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spotting.isPlayerVisible)
        {
            timerShootingLeft -= Time.deltaTime;        //Decrease timer. Shoot only once every sec
            if (Mathf.RoundToInt(Mathf.Abs(timerShootingLeft)) % 2 == 0)
            {
                Ray ray = new Ray(transform.position, Vector3.forward);
                RaycastHit raycastHit;
                Vector3 endPosition = transform.position + (laserMaxLength * Vector3.forward);
                laserLineRenderer.SetPositions(new Vector3[2] { transform.forward, transform.right });


                if (Physics.Raycast(ray, out raycastHit, laserMaxLength))
                {
                    endPosition = raycastHit.point + new Vector3(0, 2, 0);
                }

                laserLineRenderer.SetPosition(0, new Vector3(0, 0, 0)); ;
                laserLineRenderer.SetPosition(1, endPosition);
                laserLineRenderer.enabled = true;
            }
            else
            {
                laserLineRenderer.enabled = false;      //turn it off for a sec
            }
        }
        else
        {
            laserLineRenderer.enabled = false;
            timerShootingLeft = timerShooting;      //Reset timer

        }
    }
}
