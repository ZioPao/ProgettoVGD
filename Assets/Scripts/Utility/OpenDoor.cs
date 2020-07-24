﻿using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    private bool isOpening;
    private bool forceActivate = false;

    private Quaternion correctRotation;

    void Start()
    {
        isOpening = false;
        correctRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -90f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Values.GetIsUsingDoor() && Values.GetHasKey())
        {
            isOpening = true;
            Values.SetIsUsingDoor(false);
            Values.SetHasKey(false);
            this.tag = "InteractableOver";        //To disable the "interact with e" message
        }

        if (isOpening)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, correctRotation, Time.deltaTime * 10f);
        }
        
    }

    public void ForceActivation()
    {
        transform.rotation = correctRotation;
    }
}