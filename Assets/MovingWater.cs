using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWater : MonoBehaviour
{

    [SerializeField] public float countdownMovingWater = 1.0f;

    private float countdownMovingWaterCurrent;
    // Start is called before the first frame update
    bool isMovingUp = true;

    void Start()
    {
        countdownMovingWaterCurrent = countdownMovingWater;
    }

    private void FixedUpdate()
    {
        if (isMovingUp)
        {
            countdownMovingWaterCurrent -= Time.deltaTime;
            transform.position += new Vector3(0, 0.001f, 0);

            if (countdownMovingWaterCurrent <= 0)
                isMovingUp = false;
        }
        else
        {
            countdownMovingWaterCurrent += Time.deltaTime;
            transform.position -= new Vector3(0, 0.001f, 0);

            if (countdownMovingWaterCurrent >= countdownMovingWater)
                isMovingUp = true;
        }


    }
}
