using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Player;
using Saving;
using UnityEditor;
using UnityEngine;

namespace Utility
{
    public class LevelManager : MonoBehaviour
    {
        private const string EnemyTag = "enemy";
        private const string PickupTag = "Pickup";
        private const string InteractableTag = "Interactable";
        private const string InteractableOverTag = "InteractableOver";
        private const string TriggerTag = "Trigger";
        private const string SpawnerTag = "Spawner";
        private const string ProjectileTag = "Projectile";
        private const string LeverBossString = "LeverBoss";

        [SerializeField] private Vector3 spawnPoint;
        [SerializeField] private Vector3 spawnPointRotation;
        [SerializeField] private int levelId;


        //Enemy stuff
        public Dictionary<String, EnemyStatus> enemiesStatus;

        //To save status at the start of the level. Necessary for saving
        public List<string> triggersStart, pickupsStart;


        private void Awake()
        {
            Values.SetCurrentLevel(levelId);

            GameObject player;
            //print("game over: " + Values.GetIsGameOver());

            //Initializes Audio Players
            Audio.SoundManager.InitializeSoundPlayer();
            Audio.SoundManager.InitializeMusicPlayer();

            Audio.SoundManager.PlaySoundtrack(Audio.SoundManager.SoundTracks.LevelTrack);


            //Is changing scene è relativo al cambio di scena da livello a livello 2, non c'entra coi cambi di scena
            //al cambio del salvataggio.
            if (!Values.GetIsChangingScene())
            {
                player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
                if (Values.GetIsLoadingSave())
                {
                    return; //Stops everything else, it should be ok.
                }
            }
            else
            {
                player = GameObject.Find("oldPlayer");
                player.name = "Player"; //reset name

                player.GetComponent<InteractionController>().Awake(); //upates it
                player.GetComponentInChildren<EnemySpritesManager>().Awake();
            }

            player.transform.position = spawnPoint;
            player.transform.rotation = Quaternion.Euler(spawnPointRotation);

            enemiesStatus = new Dictionary<string, EnemyStatus>();

            //All'init di un livello resetta il current Boss presente su Values

            Values.SetCurrentBoss(null);

            //Per deterimnare che triggers\pickups resettare quando si carica una partita
            var tmpTriggers = GameObject.FindGameObjectsWithTag("Trigger");
            triggersStart = new List<string>();
            foreach (var t in tmpTriggers)
            {
                triggersStart.Add(t.name);
            }

            var tmpPickups = GameObject.FindGameObjectsWithTag("Pickup");
            pickupsStart = new List<string>();
            foreach (var p in tmpPickups)
            {
                pickupsStart.Add(p.name);
            }


            GetComponentInChildren<Terrain>().detailObjectDistance = 10000;        //todo rendi piu carino
            //allo spawn del player si attivano gli spawner
            StartCoroutine(WaitAndInitComponents());
        }


        public List<string> GetCurrentTriggers()
        {
            var triggers = GameObject.FindGameObjectsWithTag(TriggerTag);

            List<string> list = new List<string>();
            foreach (var x in triggers)
            {
                list.Add(x.name);
            }

            return list;

            // //get if they're active or not
            // GameObject[] triggers = GameObject.FindGameObjectsWithTag(TriggerTag);
            // Dictionary<String, bool> triggerStatus = new Dictionary<string, bool>();
            //
            // foreach (var trigger in triggers)
            // {
            //     triggerStatus.Add(trigger.name, trigger.activeSelf);
            // }
            //
            // return triggerStatus;
        }

        public List<string> GetOriginalTriggers()
        {
            return triggersStart;
        }

        public List<string> GetCurrentPickups()
        {
            var pickups = GameObject.FindGameObjectsWithTag(PickupTag);

            List<string> list = new List<string>();
            foreach (var x in pickups)
            {
                list.Add(x.name);
            }

            return list;
        }

        public List<string> GetOriginalPickups()
        {
            return pickupsStart;
        }

        public Dictionary<String, EnemySpawnerStatus> GetSpawnerStatus()
        {
            GameObject[] spawners = GameObject.FindGameObjectsWithTag(SpawnerTag);

            Dictionary<String, EnemySpawnerStatus> spawnerStatus = new Dictionary<string, EnemySpawnerStatus>();

            foreach (var spawner in spawners)
            {
                spawnerStatus.Add(spawner.gameObject.name, spawner.GetComponent<EnemySpawner>().GetStatus());
            }

            return spawnerStatus;
        }

        public void UpdateEnemiesStatus()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);

            foreach (var enemy in enemies)
            {
                EnemyStatus status = enemy.GetComponent<EnemyBase>().GetStatus();
                try
                {
                    enemiesStatus.Add(enemy.name, status);
                }
                catch (ArgumentException)
                {
                    enemiesStatus.Remove(enemy.name);
                    enemiesStatus.Add(enemy.name, status);
                }
            }
        }

        public List<ProjectileStatus> GetProjectileStatus()
        {
            var projectiles = GameObject.FindGameObjectsWithTag(ProjectileTag).ToList();
            List<ProjectileStatus> projectilesStatus = new List<ProjectileStatus>();

            foreach (var x in projectiles)
            {
                projectilesStatus.Add(x.GetComponent<ProjectileScript>().GetStatus());
            }

            return projectilesStatus;
        }

        public GameObject[] GetPickups()
        {
            //Prende i pickups che sono presenti al momento del salvataggio
            var pickups = GameObject.FindGameObjectsWithTag(PickupTag);
            return pickups;
            //todo non serve se sono sullo stesso livello i guess. Ma credo sia rotto con gli altri
        }

        public Dictionary<string, bool> GetInteractables()
        {
            Dictionary<string, bool> interactablesDictionary = new Dictionary<string, bool>();
            GameObject[] interactables = GameObject.FindGameObjectsWithTag(InteractableTag);

            foreach (var x in interactables)
            {
                switch (x.name)
                {
                    case LeverBossString:
                        LeverScript tmpLever = x.GetComponent<LeverScript>();
                        interactablesDictionary.Add(x.name, tmpLever.enabled);
                        break;
                    case "DoorOptional":
                        OpenDoor tmpDoor = x.GetComponent<OpenDoor>();
                        interactablesDictionary.Add(x.transform.parent.name,
                            tmpDoor.enabled); //Parent per via di come è strutturato il prefab
                        break;
                }
            }

            foreach (var x in GameObject.FindGameObjectsWithTag(InteractableOverTag))
            {
                switch (x.name)
                {
                    case LeverBossString:
                        LeverScript tmpLever = x.GetComponent<LeverScript>();
                        interactablesDictionary.Add(x.name, false);
                        break;
                    case "DoorOptional":
                        OpenDoor tmpDoor = x.GetComponent<OpenDoor>();
                        interactablesDictionary.Add(x.transform.parent.name, false);
                        break;
                }
            }

            return interactablesDictionary;
        }
        IEnumerator WaitAndInitComponents()
        {
            //Aspetta che la navmesh sia pronta
            //todo determinare come capire se la navmesh è pronta
            yield return new WaitForSeconds(1);
            
            foreach (var x in GameObject.FindGameObjectsWithTag(SpawnerTag))
            {
                x.GetComponent<EnemySpawner>().enabled = true;
            }

            SignController tmp = GameObject.Find("InteractableObjects").GetComponent<SignController>();
            Values.SetCurrentSignController(tmp);

        }
    }
}