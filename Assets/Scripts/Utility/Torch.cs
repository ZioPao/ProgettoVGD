using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private Light torchLight;

    private float zCenter, yAxisLock;
    private float maxMovement = 0.15f;

    private Color firstColor = new Color(82,38,24);
    private Color secondColor = new Color(82, 52, 24);
    void Awake()
    {
        torchLight = GetComponent<Light>();
        zCenter = transform.position.z;
        yAxisLock = transform.position.y;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Values.GetIsLoadingSave())
        {
            float t = Mathf.PingPong(Time.time, 0.2f);
            torchLight.color = Color.Lerp(firstColor, secondColor, t);

            //print(transform.right);
            transform.position = new Vector3(transform.position.x, transform.position.y, zCenter + Mathf.PingPong(Time.time * 1.5f, maxMovement) - maxMovement/2f);//move on z axis only
            //transform.forward = new Vector3(transform.forward.x, transform.forward.y,
            //   zCenter + Mathf.PingPong(Time.time * 1.5f, maxMovement) - maxMovement / 2f);



            if (Values.GetCurrentLevel() != 3)
            {
                Vector3 playerPosition = Values.GetPlayerTransform().position;
                transform.LookAt(new Vector3(playerPosition.x,
                    transform.position.y,
                    playerPosition.z));
                //transform.LookAt(Values.GetPlayerTransform());

            }
        }
     
         
    }
}
