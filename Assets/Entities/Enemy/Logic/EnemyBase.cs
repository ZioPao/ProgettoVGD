﻿using UnityEngine;

namespace Entities.Enemy.Logic
{
    public class EnemyBase : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public GameObject[] GetAllEnemies()
        {
            return GameObject.FindGameObjectsWithTag("enemy");
        }
    }
}
