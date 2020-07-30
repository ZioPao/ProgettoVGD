using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private int nextLevelId;
    [SerializeField] private GameObject currentLevel;


    private void OnTriggerEnter(Collider other)
    {
        var level = Resources.Load<GameObject>("Prefabs/Levels/Level" + nextLevelId);

        PrefabUtility.InstantiatePrefab(level);

        //player position 
        switch (nextLevelId)
        {
            case (2):
                player.transform.position = new Vector3(16.51f, 26.632f, 13.16f);
                break;
            case (3):
                break;
        }

        Destroy(currentLevel); //Destroy the old level at the end, killing the script    }
    }
}