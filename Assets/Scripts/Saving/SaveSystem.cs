using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Enemies;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using Object = UnityEngine.Object;

namespace Saving
{
    public class SaveSystem : MonoBehaviour
    {
        public SaveSystem()
        {
            save = new Save();
            canLoad = false;
        }


        private Save save;
        private GameObject currentLevel;
        private bool canLoad;

        public void Save()
        {
            //Player stats
            Transform player = Values.GetPlayerTransform();
            save.playerPosition = player.position;
            save.playerRotation = player.rotation;

            save.health = Values.GetHealth();
            save.stamina = Values.GetStamina();

            Dictionary<Values.WeaponEnum, int> weaponsCurrentAmmo = new Dictionary<Values.WeaponEnum, int>();
            foreach (Values.WeaponEnum weaponEnum in Enum.GetValues(typeof(Values.WeaponEnum)))
            {
                weaponsCurrentAmmo.Add(weaponEnum, Values.GetCurrentAmmo()[weaponEnum]);
            }

            save.weaponsCurrentAmmo = weaponsCurrentAmmo;
            
            Dictionary<Values.WeaponEnum, int> weaponsReserveAmmo = new Dictionary<Values.WeaponEnum, int>();
            foreach (Values.WeaponEnum weaponEnum in Enum.GetValues(typeof(Values.WeaponEnum)))
            {
                weaponsReserveAmmo.Add(weaponEnum, Values.GetAmmoReserve()[weaponEnum]);
            }

            save.weaponsCurrentReserve = weaponsReserveAmmo;

                
            //Get the level manager
            GameObject level = GameObject.FindWithTag("Level");

            save.levelName = level.name;
            LevelManager manager = level.GetComponent<LevelManager>();

            //Pickups
            try
            {
                save.pickupStatus = manager.GetPickups();
            }
            catch (NullReferenceException)
            {
                ;
            }

            //Spawners
            try
            {
                save.enemySpawnerStatus = manager.GetSpawnerStatus();
            }
            catch (NullReferenceException)
            {
                ;
            }
            //Triggers
            //save.triggersStatus = manager.GetTriggerStatus();

            //Levers and doors
            save.interactableStatus = manager.GetInteractables();

            //Enemies status saving
            manager.UpdateEnemiesStatus();
            save.enemiesStatus = manager.enemiesStatus;

            //Projectiles

            save.projectileStatus = manager.GetProjectileStatus();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, save);
            file.Close();

            UnityEngine.Debug.Log("Saved");
        }

        public void Load()
        {
            Values.SetIsLoadingSave(true);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            
            save = (Save) bf.Deserialize(file);
            file.Close();

            //Load correct scene
            StartCoroutine(LoadLevel(save.levelName));
            
            //Setup player
            
            GameObject newPlayer = GameObject.Find("Player");
            Transform newPlayerT = newPlayer.transform;

            newPlayerT.position = save.playerPosition;
            newPlayerT.rotation = save.playerRotation;
            
            Values.SetPlayerTransform(newPlayer.transform);
            Values.SetHealth(save.health);
            Values.SetStamina(save.stamina);
            
            //Weapons
            foreach (var w in save.weaponsCurrentReserve)
            {
                Values.SetAmmoReserve(w.Key, w.Value);
            }
            
            foreach (var w in save.weaponsCurrentAmmo)
            {
                Values.SetCurrentAmmo(w.Key, w.Value);
            }
            
            //Deletes all the enemies todo should be useless now
            foreach (var enemy in GameObject.FindGameObjectsWithTag("enemy"))
            {
                Object.Destroy(enemy);
            }
            
            //Creates them again
            GameObject enemyPrefab =
                Resources.Load<GameObject>("Prefabs/Enemies/" + save.levelName); //Level name = enemy type
            
            EnemySpritesManager spritesManager = Values.GetEnemySpritesManager();
            foreach (var element in save.enemiesStatus)
            {
                GameObject tmpEnemy = PrefabUtility.InstantiatePrefab(enemyPrefab) as GameObject;
                tmpEnemy.transform.SetParent(GameObject.Find("Enemies").transform);
            
                tmpEnemy.GetComponent<EnemyBase>().Reload(element.Value);
            
                tmpEnemy.transform.position = element.Value.GetPosition();
                tmpEnemy.transform.rotation = element.Value.GetRotation();
                tmpEnemy.GetComponent<EnemyMovement>().Reload();
            
                tmpEnemy.GetComponent<EnemyIntelligence>().Start();
                tmpEnemy.GetComponent<EnemyShooting>().Start();
            
            
                spritesManager.AddEnemyToEnemyList(tmpEnemy);
            }

            //Projectiles
            
            //Destroy all the old projectiles todo should be useless now
            foreach (var p in GameObject.FindGameObjectsWithTag("Projectile"))
            {
                Object.Destroy(p);
            }
            
            GameObject projPrefab =
                Resources.Load<GameObject>("Prefabs/Projectiles/" + save.levelName); //Level name = projectile type
            foreach (var pStatus in save.projectileStatus)
            {
                GameObject tmpProj = PrefabUtility.InstantiatePrefab(projPrefab) as GameObject;
            
                ProjectileScript tmpScript = tmpProj.GetComponent<ProjectileScript>();
            
                tmpScript.SetTransform(pStatus.GetPosition(), pStatus.GetRotation());
                tmpScript.Reload(pStatus);
            }
            
            //Spawners 
            currentLevel = GameObject.FindWithTag("Level");
            foreach (var spawnerStatus in save.enemySpawnerStatus)
            {
                var spawnerObject = currentLevel.transform.Find("Spawners/" + spawnerStatus.Key);

                //todo aggiungi caso specialep re bossSpawner
                if (spawnerObject.name != "BossSpawner")
                {
                    var tmpSpawner = spawnerObject.GetComponent<EnemySpawner>();
                    StartCoroutine(WaitForComponentStartup<EnemySpawner>(tmpSpawner, spawnerStatus, enemyPrefab));
                }
                
            }
            
            //Interactables
            foreach (var interactable in save.interactableStatus)
            {
                var interactableObject = currentLevel.transform.Find("InteractableObjects/" + interactable.Key);
            
                //Check aggiuntivo per capire se stiamo prendendo l'object igusto o meno
            
                switch (interactableObject.name)
                {
                    case "LeverBoss":
                        //Destroy(interactableObject);
                        //StartCoroutine(InstantiatePrefab("Prefabs/Levels/Generic/Prefabs/LeverBoss"));
            
                        //interactableObject = GameObject.Find(interactable.Key);
                        var lever = interactableObject.GetComponent<LeverScript>();
            
                        if (!interactable.Value)
                        {
                            lever.ForceActivation();
                        }
            
                        break;
            
                    case "DoorPassageOptional":
                        var door = interactableObject.GetComponentInChildren<OpenDoor>();
            
                        if (!interactable.Value)
                        {
                            door.ForceActivation();
                        }
            
                        break;
                    default:
                        break;
                }
            }

            Values.SetIsLoadingSave(false); //Finished loading
            print("Caricato");
        }

        IEnumerator LoadLevel(string levelName)
        {
            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Scenes/" + levelName, LoadSceneMode.Single);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                    canLoad = true;
                    //StopCoroutine(LoadNewGame());
                }


                yield return null;
            }
            //Clean the level and set it up 
            //Object.Destroy(GameObject.FindWithTag("Level"));
            //GameObject newLevel = Resources.Load<GameObject>("Prefabs/Levels/" + save.levelName);


            // PrefabUtility.UnpackPrefabInstance(currentLevel = PrefabUtility.InstantiatePrefab(newLevel) as GameObject
            //     , PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

        }

        IEnumerator InstantiatePrefab(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);

            PrefabUtility.UnpackPrefabInstance(PrefabUtility.InstantiatePrefab(prefab) as GameObject
                , PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);


            yield return new WaitForEndOfFrame();
        }


        public IEnumerator WaitForComponentStartup<T>(EnemySpawner x, KeyValuePair<String, EnemySpawnerStatus> status,
            GameObject prefab)
        {
            
            x.SetStatus(status.Value);
            x.SetEnemyPrefab(prefab);

            yield return new WaitForEndOfFrame();
        }
    }
}