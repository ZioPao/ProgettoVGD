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
        private LevelManager levelManager;
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
            save.hasKey = Values.GetHasKey();

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

            //Dynamic pickups

            save.dynamicPickups = manager.GetDynamicPickups();

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
            save.triggers = Values.GetCompletedTriggers();


            //save.triggersStatus = manager.GetTriggerStatus();

            //Levers and doors
            save.interactableStatus = manager.GetInteractables();

            //Enemies status saving
            manager.UpdateEnemiesStatus();
            save.enemiesStatus = manager.enemiesStatus;

            //Projectiles
            save.projectileStatus = manager.GetProjectileStatus();

            //Currently held weapons
            save.heldWeapons = Values.GetHeldWeapons();

            save.currentWeapon = Values.GetCurrentWeapon();

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
            StartCoroutine(LoadSave(save.levelName));
        }


        IEnumerator LoadSave(string levelName)
        {
            Values.SetCanSave(false);
            Values.SetCanPause(false);
            Values.SetIsLoadingSave(true);

            if (Values.GetPlayerTransform() != null)
            {
                SceneManager.MoveGameObjectToScene(Values.GetPlayerTransform().gameObject,
                    SceneManager.GetActiveScene());
            }

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
                    yield return new WaitForSeconds(0.2f); //non ho idea di come fare il check al momento

                    GameObject newPlayer = null;
                    Transform newPlayerT = null;
                    bool isPlayerLoaded = false;

                    //Attende fino a che il LevelManager non ha spawnato il player, dopodiché edita
                    //quello che è da ripristinare

                    while (!GameObject.FindWithTag(Values.LevelTag))
                        yield return null;
                    currentLevel = GameObject.FindWithTag(Values.LevelTag);
                    levelManager = currentLevel.GetComponent<LevelManager>();

                    Debug.LogWarning("Caricato levelmanager");

                    Instantiate(Resources.Load("Prefabs/Player"));
                    newPlayer = GameObject.FindWithTag(Values.PlayerTag);
                    while (!isPlayerLoaded)
                    {
                        while (newPlayer.transform == null)
                            yield return new WaitForEndOfFrame();

                        isPlayerLoaded = true;
                        Debug.LogWarning("Trovato player");
                        Debug.LogWarning(newPlayer.transform);
                    }

                    yield return new WaitUntil(Values.GetIsWeaponControllerDoneLoading);
                    while (newPlayer.transform == null)
                    {
                        yield return new WaitForEndOfFrame();
                    }

                    newPlayerT = newPlayer.transform;
                    print("does this exist?" + newPlayerT);
                    newPlayerT.position = save.playerPosition;
                    newPlayerT.rotation = save.playerRotation;


                    Values.SetPlayerTransform(newPlayer.transform);
                    Values.SetHealth(save.health);
                    Values.SetStamina(save.stamina);
                    Values.SetHasKey(save.hasKey);

                    //Weapons
                    // while (!)
                    // {
                    //     yield return null;        //Wait until it's done
                    // }

                    foreach (var w in save.weaponsCurrentReserve)
                    {
                        Values.SetAmmoReserve(w.Key, w.Value);
                    }

                    foreach (var w in save.heldWeapons)
                    {
                        Values.AddHeldWeapon(w.Key,
                            w.Value); //Dovrebbe aver fatto la reinit del weapon controller... O almeno, non dovrebbe.. I guess? Fuck
                    }

                    foreach (var w in save.weaponsCurrentAmmo)
                    {
                        Values.SetCurrentAmmo(w.Key, w.Value);
                    }

                    Values.GetWeaponObjects()[save.currentWeapon].SetActive(true);
                    Values.SetCurrentWeapon(save.currentWeapon);

                    /* ENEMIES */

                    //Creates them again
                    foreach (var enemy in GameObject.FindGameObjectsWithTag(Values.EnemyTag))
                    {
                        Destroy(enemy);
                    }


                    GameObject enemyPrefab =
                        Resources.Load<GameObject>("Prefabs/Enemies/" + save.levelName); //Level name = enemy type

                    EnemySpritesManager spritesManager = Values.GetEnemySpritesManager();
                    foreach (var element in save.enemiesStatus)
                    {
                        GameObject tmpEnemy = Instantiate(enemyPrefab, element.Value.GetPosition(),
                            element.Value.GetRotation(),
                            GameObject.Find("Enemies").transform);

                        //todo non rimette il nome corretto 
                        //todo ci dev'essere una maniera meno orribile per reiniziallizare un nemico
                        tmpEnemy.GetComponent<EnemyBase>().Reload(element.Value);
                        tmpEnemy.GetComponent<EnemyMovement>().Reload();
                        tmpEnemy.GetComponent<EnemyIntelligence>().Start();
                        tmpEnemy.GetComponent<EnemyShooting>().Start();

                        tmpEnemy.name = element.Key; //Mette il nome corretto
                        spritesManager.AddEnemyToEnemyList(tmpEnemy);
                    }

                    //Projectiles

                    //Destroy all the old projectiles
                    foreach (var p in GameObject.FindGameObjectsWithTag(Values.ProjectileTag))
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


                    foreach (var spawnerStatus in save.enemySpawnerStatus)
                    {
                        var spawnerObject = currentLevel.transform.Find("Spawners/" + spawnerStatus.Key);

                        //todo aggiungi caso speciale per bossSpawner
                        if (!spawnerObject.CompareTag(Values.BossSpawnerTag))
                        {
                            var tmpSpawner = spawnerObject.GetComponent<EnemySpawner>();
                            tmpSpawner.SetStatus(spawnerStatus.Value);
                            tmpSpawner.SetEnemyPrefab(enemyPrefab);
                        }
                    }

                    //Interactables
                    foreach (var interactable in save.interactableStatus)
                    {
                        var interactableObject = currentLevel.transform.Find("InteractableObjects/" + interactable.Key);
                        if (!interactable.Value)
                        {
                            interactableObject.GetComponent<IInteractableMidGame>().ForceActivation();
                        }
                    }

                    //Triggers


                    foreach (var oldT in Values.GetOriginalTriggers())
                    {
                        //se contiene il trigger, significa che è già stato eseguito
                        if (save.triggers.Contains(oldT))
                        {
                            GameObject.Find(oldT).GetComponent<ITriggerMidGame>().RunTrigger();
                        }
                    }

                    //Pickups
                    foreach (var oldP in Values.GetOriginalPickups())
                    {
                        if (!save.pickups.Contains(oldP))
                        {
                            //cerca e distrugge il pickup in questione in quanto è già stato preso
                            var tmpPickup = GameObject.Find(oldP);
                            Destroy(tmpPickup);
                        }
                    }

                    //Dynamic pickups
                    foreach (var p in save.dynamicPickups)
                    {
                        GameObject tmpPickup = null;
                        switch (p.Item2)
                        {
                            case Values.PickupEnum.AmmoBox:

                                tmpPickup = Instantiate(Values.GetAmmoBoxPrefab(), p.Item3, Quaternion.identity);
                                break;
                            case Values.PickupEnum.HealthPack:
                                tmpPickup = Instantiate(Values.GetHealthPackPrefab(), p.Item3, Quaternion.identity);
                                break;
                        }

                        tmpPickup.name = p.Item1;
                    }

                    Values.SetIsLoadingSave(false); //gli enemy spawner torneranno a funzionare
                    GameObject.Find(Values.LoadingCanvasName).GetComponent<Canvas>().enabled = false;
                    print("Caricato");

                    yield return new WaitForSeconds(1);
                    Values.SetCanPause(true);
                    Values.SetCanSave(true);
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