using System;
using Enemies;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Boss
{
    public class CheckBossSpawner : MonoBehaviour
    {
        private GameObject fakeBoss;
        private Vector3 fakeBossLastPosition;

        [SerializeField] private GameObject spawnPoint1, spawnPoint2;

        private bool isFakeBossDead;

        private GameObject sign;

        void Start()
        {
            
            //It runs AFTER the instantiation of the fakeboss, so it won't get shitted on later
            fakeBoss = GameObject.Find("FakeBossLevel3");
            isFakeBossDead = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!isFakeBossDead)
            {
                try
                {
                    fakeBossLastPosition = fakeBoss.transform.position;
                    //it'll fail when the fake boss is dead
                }
                catch (Exception)
                {
                    //Spawn the object that'll start the real boss battle
                    GameObject signPrefab = Resources.Load<GameObject>("Prefabs/Levels/Generic/FakeSign");

                    sign = PrefabUtility.InstantiatePrefab(signPrefab) as GameObject;
                    sign.transform.name = "Sign";        //per far funzionare correttamente l'interactioncontroller
                    sign.transform.parent = GameObject.Find("InteractableObjects").transform;
                    sign.transform.position = fakeBossLastPosition;
                    isFakeBossDead = true;

                }
            }
            else
            {
                if (Values.GetHasInteractedWithWinObject())
                {

                    Vector3 chosenPoint;
                    var signPosition = sign.transform.position - new Vector3(0,0.05f,0);
                    Destroy(sign);

                    if (Vector3.Distance(Values.GetPlayerTransform().position, spawnPoint1.transform.position) < 10f)
                    {
                        chosenPoint = spawnPoint2.transform.position;
                    }
                    else
                    {
                        chosenPoint = spawnPoint1.transform.position;
                    }
                    
                    GameObject bossPrefab = Resources.Load<GameObject>("Prefabs/Enemies/BossLevel3");
                    var boss = PrefabUtility.InstantiatePrefab(bossPrefab) as GameObject;


                    var newBossPosition = chosenPoint;
                    boss.transform.position = newBossPosition;
                    boss.transform.rotation = Random.rotation;
                    boss.GetComponent<NavMeshAgent>().Warp(chosenPoint);
                    boss.GetComponent<EnemyBase>().GetStatus().SetIsPlayerInView(true);        //subito aggrato
                    enabled = false;        //disables the checker
                }
            }
        }
    }
}