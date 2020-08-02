using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Boss;
using Enemies;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
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
        }


        private Save save;
        private GameObject currentLevel;
        private Canvas canvas;

        public void Save()
        {

            StartCoroutine(ShowSaveTip());
            
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
            save.pickups = manager.GetCurrentPickups();


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
            save.triggers = manager.GetCurrentTriggers();
           
            
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
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);

            save = (Save) bf.Deserialize(file);
            file.Close();
            StartCoroutine(LoadLevel(save.levelName));
        }


        IEnumerator LoadLevel(string levelName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Scenes/" + levelName, LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;
            GetComponentInChildren<Canvas>().enabled = true;

            while (!asyncOperation.isDone)
            {
                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;

                    
                    //Necessario un minimo d'attesa per determinare quando ha effettivamente caricato TUTTO.
                    yield return new WaitForSeconds(0.2f);        //non ho idea di come fare il check al momento

                    Values.SetCanSave(false);
                    Values.SetCanPause(false);
                    Values.SetIsLoadingSave(true);

                    //todo probably totally useless
                    //just in case?
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);

                    save = (Save) bf.Deserialize(file);
                    file.Close();

                    GameObject newPlayer = null;
                    Transform newPlayerT = null;
                    bool isPlayerLoaded = false;
                    
                    while (!isPlayerLoaded)
                    {
                        newPlayer = GameObject.FindWithTag("Player");
                        if (newPlayer == null)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        else
                        {
                            newPlayerT = newPlayer.transform;
                            isPlayerLoaded = true;
                        }

                    }

                  
                    

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

                    // //Deletes all the enemies todo should be useless now
                    // foreach (var enemy in GameObject.FindGameObjectsWithTag("enemy"))
                    // {
                    //     Object.Destroy(enemy);
                    // }


                    //Creates them again
                    GameObject enemyPrefab =
                        Resources.Load<GameObject>("Prefabs/Enemies/" + save.levelName); //Level name = enemy type

                    EnemySpritesManager spritesManager = Values.GetEnemySpritesManager();
                    foreach (var element in save.enemiesStatus)
                    {
                        GameObject tmpEnemy = Instantiate(enemyPrefab, element.Value.GetPosition(), element.Value.GetRotation(), 
                            GameObject.Find("Enemies").transform );
                        
                        //todo non rimette il nome corretto 
                        //todo ci dev'essere una maniera meno orribile per reiniziallizare un nemico
                        tmpEnemy.GetComponent<EnemyBase>().Reload(element.Value);
                        tmpEnemy.GetComponent<EnemyMovement>().Reload();
                        tmpEnemy.GetComponent<EnemyIntelligence>().Awake();
                        tmpEnemy.GetComponent<EnemyShooting>().Awake();


                        spritesManager.AddEnemyToEnemyList(tmpEnemy);
                    }

                    //Projectiles

                    //Destroy all the old projectiles todo should be useless now
                    foreach (var p in GameObject.FindGameObjectsWithTag("Projectile"))
                    {
                        Object.Destroy(p);
                    }

                    GameObject projPrefab =
                        Resources.Load<GameObject>("Prefabs/Projectiles/" +
                                                   save.levelName); //Level name = projectile type
                    foreach (var pStatus in save.projectileStatus)
                    {
                        GameObject tmpProj = Instantiate(projPrefab);

                        ProjectileScript tmpScript = tmpProj.GetComponent<ProjectileScript>();

                        tmpScript.SetTransform(pStatus.GetPosition(), pStatus.GetRotation());
                        tmpScript.Reload(pStatus);
                    }

                    //Spawners 
                    currentLevel = GameObject.FindWithTag(Values.levelTag);
                    foreach (var spawnerStatus in save.enemySpawnerStatus)
                    {
                        var spawnerObject = currentLevel.transform.Find("Spawners/" + spawnerStatus.Key);

                        //todo aggiungi caso specialep re bossSpawner
                        if (!spawnerObject.CompareTag(Values.bossSpawnerTag))
                        {
                            var tmpSpawner = spawnerObject.GetComponent<EnemySpawner>();
                            StartCoroutine(WaitForComponentStartup<EnemySpawner>
                                (tmpSpawner, spawnerStatus, enemyPrefab));
                        }
                    }

                    //Interactables
                    foreach (var interactable in save.interactableStatus)
                    {
                        var interactableObject = currentLevel.transform.Find("InteractableObjects/" + interactable.Key);

                        interactableObject.GetComponent<IInteractableMidGame>().ForceActivation();

                        // //Check aggiuntivo per capire se stiamo prendendo l'object igusto o meno
                        //
                        // switch (interactableObject.name)
                        // {
                        //     
                        //     case "LeverBoss":
                        //         //Destroy(interactableObject);
                        //         //StartCoroutine(InstantiatePrefab("Prefabs/Levels/Generic/Prefabs/LeverBoss"));
                        //
                        //         //interactableObject = GameObject.Find(interactable.Key);
                        //         var lever = interactableObject.GetComponent<LeverScript>();
                        //
                        //         if (!interactable.Value)
                        //         {
                        //             lever.ForceActivation();
                        //         }
                        //
                        //         break;
                        //
                        //     case "DoorPassageOptional":
                        //         var door = interactableObject.GetComponentInChildren<OpenDoor>();
                        //
                        //         if (!interactable.Value)
                        //         {
                        //             door.ForceActivation();
                        //         }
                        //
                        //         break;
                        //     default:
                        //         break;
                        // }
                    }
                    
                    var tmpLevelManager = currentLevel.GetComponent<LevelManager>();
                    
                    //Triggers
                    var oldTriggers = tmpLevelManager.GetOriginalTriggers();
                    foreach (var oldT in oldTriggers)
                    {
                        if (!save.triggers.Contains(oldT))
                        { 
                            GameObject.Find(oldT).GetComponent<ITriggerMidGame>().RunTrigger();
                            
                        }
                    }

                    //Pickups

                    var oldPickups = tmpLevelManager.GetOriginalPickups();
                    foreach (var oldP in oldPickups)
                    {
                        if (!save.pickups.Contains(oldP))
                        {
                            //cerca e distrugge il pickup in questione in quanto è già stato preso
                            var tmpPickup = GameObject.Find(oldP);
                            Destroy(tmpPickup);
                        }
                    }
                    Values.SetIsLoadingSave(false);        //gli enemy spawner torneranno a funzionare
                    GameObject.Find(Values.loadingCanvasName).GetComponent<Canvas>().enabled = false;
                    print("Caricato");

                    yield return new WaitForSeconds(2);
                    Values.SetCanPause(true);

                }
                else
                {
                    yield return null;

                }
            }
        }

        public IEnumerator WaitForComponentStartup<T>(EnemySpawner x, KeyValuePair<String, EnemySpawnerStatus> status,
            GameObject prefab)
        {
            x.SetStatus(status.Value);
            x.SetEnemyPrefab(prefab);

            yield return new WaitForEndOfFrame();
        }

        private IEnumerator ShowSaveTip()
        {

            var canvas = GameObject.Find("SavingCanvas").GetComponent<Canvas>();
            canvas.enabled = true;

            yield return new WaitForSecondsRealtime(3);
            
            canvas.enabled = false;


        }
    }
}