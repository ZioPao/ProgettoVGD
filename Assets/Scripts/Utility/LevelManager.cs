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
        [SerializeField] private Vector3 spawnPoint;
        [SerializeField] private Vector3 spawnPointRotation;
        [SerializeField] private int levelId;


        //Enemy stuff
        public Dictionary<String, EnemyStatus> enemiesStatus;

        public void Awake()
        {
            
            
            //E' AWAKE PER FARLO GIRARE PRIMA DEL LOAD SAVE, COSI CHE POSSA RECUPERARE I TRIGGERS ORIGINALI
            
            //Ma se è awake non funziona il load game mid 
            Values.SetCurrentLevel(levelId);
            Values.SetHasInteractedWithWinObject(false); //per evitare problemi dopo aver finito il gioco
            GameObject player;

            //Initializes Audio Players
            Audio.SoundManager.InitializeSoundPlayer();
            Audio.SoundManager.InitializeMusicPlayer();

            Audio.SoundManager.PlaySoundtrack(Audio.SoundManager.SoundTracks.LevelTrack);
            Values.SetCurrentSignController(GameObject.Find(Values.SignsParentName).GetComponent<SignController>());


            //Is changing scene è relativo al cambio di scena da livello a livello 2\3, non c'entra coi cambi di scena
            //al cambio del salvataggio.
            
            
            //Ergo, new game teoricamente

            if (Values.GetIsChangingScene())
            {
                player = GameObject.Find(Values.OldPlayerName);
                player.name = Values.PlayerName; //reset name

                player.GetComponent<InteractionController>().Awake(); //upates it
                player.GetComponentInChildren<EnemySpritesManager>().Awake();
                player.transform.position = spawnPoint;
                player.transform.rotation = Quaternion.Euler(spawnPointRotation);
                Values.SetIsChangingScene(false);        //Finito di caricare nuova scena dall'endtrigger
            }
            else if (!Values.GetIsChangingScene() && !Values.GetIsLoadingSave())
            {
                //Spawn normale
                player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
                Values.InitializeCompletedTriggers();            
                player.transform.position = spawnPoint;
                player.transform.rotation = Quaternion.Euler(spawnPointRotation);


            }
            else
            {
                player = null;        //default case, non dovrebbe maia rrivarci
            }

            enemiesStatus = new Dictionary<string, EnemyStatus>();

            //All'init di un livello resetta il current Boss presente su Values

            Values.SetCurrentBoss(null);

            //Per deterimnare che triggers\pickups resettare quando si carica una partita
            var tmpTriggers = GameObject.FindGameObjectsWithTag(Values.TriggerTag);
            var triggersStart = new List<string>();
            foreach (var t in tmpTriggers)
            {
                triggersStart.Add(t.name);
            }

            Values.SetOriginalTriggers(triggersStart);


            var tmpPickups = GameObject.FindGameObjectsWithTag(Values.PickupTag);
            var pickupsStart = new List<string>();
            foreach (var p in tmpPickups)
            {
                pickupsStart.Add(p.name);
            }

            Values.SetOriginalPickups(pickupsStart);


            GetComponentInChildren<Terrain>().detailObjectDistance = 10000; //todo rendi piu carino
            //allo spawn del player si attivano gli spawner
            StartCoroutine(WaitAndInitComponents());
        }


        public List<string> GetCurrentTriggers()
        {
            var triggers = GameObject.FindGameObjectsWithTag(Values.TriggerTag);

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

        // public List<string> GetOriginalTriggers()
        // {
        //     return triggersStart;
        // }

        public List<string> GetCurrentPickups()
        {
            var pickups = GameObject.FindGameObjectsWithTag(Values.PickupTag);

            List<string> list = new List<string>();
            foreach (var x in pickups)
            {
                list.Add(x.name);
            }

            return list;
        }

        // public List<string> GetOriginalPickups()
        // {
        //     return pickupsStart;
        // }

        public Dictionary<String, EnemySpawnerStatus> GetSpawnerStatus()
        {
            GameObject[] spawners = GameObject.FindGameObjectsWithTag(Values.SpawnerTag);

            Dictionary<String, EnemySpawnerStatus> spawnerStatus = new Dictionary<string, EnemySpawnerStatus>();

            foreach (var spawner in spawners)
            {
                spawnerStatus.Add(spawner.gameObject.name, spawner.GetComponent<EnemySpawner>().GetStatus());
            }

            return spawnerStatus;
        }

        public void UpdateEnemiesStatus()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Values.EnemyTag);

            //todo non dovrebbe dare problemi ma tienilo d'occhio
            if (enemiesStatus == null)
                enemiesStatus = new Dictionary<string, EnemyStatus>();
            
            
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
            var projectiles = GameObject.FindGameObjectsWithTag(Values.ProjectileTag).ToList();
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
            var pickups = GameObject.FindGameObjectsWithTag(Values.PickupTag);
            return pickups;
            //todo non serve se sono sullo stesso livello i guess. Ma credo sia rotto con gli altri
        }

        public Dictionary<string, bool> GetInteractables()
        {
            Dictionary<string, bool> interactablesDictionary = new Dictionary<string, bool>();
            GameObject[] interactables = GameObject.FindGameObjectsWithTag(Values.InteractableTag);

            foreach (var x in interactables)
            {
                //I sign son indicati sempre come Top, todo temporaneo forse?

                interactablesDictionary.Add(x.name, x.GetComponent<IInteractableMidGame>().GetIsEnabled());
            }

            foreach (var x in GameObject.FindGameObjectsWithTag(Values.InteractableOverTag))
            {
                interactablesDictionary.Add(x.name, x.GetComponent<IInteractableMidGame>().GetIsEnabled());
            }

            return interactablesDictionary;
        }

        IEnumerator WaitAndInitComponents()
        {
            //Aspetta che la navmesh sia pronta
            //todo determinare come capire se la navmesh è pronta
            yield return new WaitForSeconds(1);

            foreach (var x in GameObject.FindGameObjectsWithTag(Values.SpawnerTag))
            {
                x.GetComponent<EnemySpawner>().enabled = true;
            }

            SignController tmp = GameObject.Find(Values.SignsParentName).GetComponent<SignController>();
            Values.SetCurrentSignController(tmp);
        }
    }
}