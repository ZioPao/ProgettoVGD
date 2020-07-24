using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private Light light;
    
    private float zCenter;
    private float maxMovement = 0.15f;

    private Color firstColor = new Color(82,38,24);
    private Color secondColor = new Color(82, 52, 24);
    void Awake()
    {
        light = GetComponent<Light>();
        zCenter = transform.position.z;


    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float t = Mathf.PingPong(Time.time, 0.2f);
        light.color = Color.Lerp(firstColor, secondColor, t);

        //print(transform.right);
        transform.position = new Vector3(transform.position.x, transform.position.y, zCenter + Mathf.PingPong(Time.time * 1.5f, maxMovement) - maxMovement/2f);//move on z axis only
        //transform.forward = new Vector3(transform.forward.x, transform.forward.y,
         //   zCenter + Mathf.PingPong(Time.time * 1.5f, maxMovement) - maxMovement / 2f);

         
    }
}
