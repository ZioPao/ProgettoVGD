using System;
using System.Collections;
using Enemies;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Boss
{
    public class FinalBossSpawner : MonoBehaviour
    {
        private GameObject fakeBoss;
        private Vector3 fakeBossLastPosition;

        [SerializeField] private GameObject spawnPoint1, spawnPoint2;

        private const string fakeBossName = "FakeBossLevel3";
        private bool isFakeBossDead;

        private GameObject sign;

        void Start()
        {
            //It runs AFTER the instantiation of the fakeboss, so it won't get shitted on later
            isFakeBossDead = false;
            StartCoroutine(WaitForFakeBossDeath());
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isFakeBossDead && Values.GetHasInteractedWithWinObject())
            {
                Vector3 chosenPoint;
                var signPosition = sign.transform.position - new Vector3(0, 0.05f, 0);
                Destroy(sign);

                //Play Laugh Sound
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.FinalBossLaugh);

                //Resume Boss Music
                Audio.SoundManager.PlaySoundtrack(Audio.SoundManager.SoundTracks.BossTrack);

                if (Vector3.Distance(Values.GetPlayerTransform().position, spawnPoint1.transform.position) < 10f)
                {
                    chosenPoint = spawnPoint2.transform.position;
                }
                else
                {
                    chosenPoint = spawnPoint1.transform.position;
                }

                GameObject bossPrefab = Resources.Load<GameObject>("Prefabs/Enemies/BossLevel3");
                var boss = Instantiate(bossPrefab, chosenPoint, Quaternion.identity);
                boss.transform.LookAt(Values.GetPlayerTransform());
                Values.GetEnemySpritesManager().AddEnemyToEnemyList(boss);

                enabled = false; //disables the spawner
            }
        }


        IEnumerator WaitForFakeBossDeath()
        {
            yield return new WaitForEndOfFrame(); //Attendi un frame per evitare falsi positivi
            
            fakeBoss = GameObject.Find(fakeBossName);

            while (fakeBoss)
            {
                fakeBossLastPosition = fakeBoss.transform.position;
                yield return null;
            }

            GameObject signPrefab = Resources.Load<GameObject>("Prefabs/Levels/Generic/FakeSign");
            sign = Instantiate(signPrefab, fakeBossLastPosition, Quaternion.identity, Values.GetCurrentSignController().transform);
            sign.transform.name = "Sign"; //per far funzionare correttamente l'interactioncontroller
            isFakeBossDead = true;

            //Play Regular Level Soundtrack
            Audio.SoundManager.PlaySoundtrack(Audio.SoundManager.SoundTracks.LevelTrack);
        }
    }
}