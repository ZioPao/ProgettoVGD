using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ReflectionProbeRealTimeManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private int time;
    [SerializeField] private ReflectionProbe probe;
    private bool isDoneUpdating;


    void Start()
    {
        //isDoneUpdating = false;
        StartCoroutine(UpdateProbe());
    }

    // private void FixedUpdate()
    // {
    //     if (isDoneUpdating)
    //     {
    //         isDoneUpdating = false;        //Reset
    //         StartCoroutine(UpdateProbe());
    //     }
    // }


    private IEnumerator UpdateProbe()
    {
        while (true)
        {
            for (int i = 0; i < time; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            probe.RenderProbe();
        }
    }
}