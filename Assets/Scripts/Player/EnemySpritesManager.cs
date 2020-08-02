using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Player
{
    public class EnemySpritesManager : MonoBehaviour
    {
        /*Enemy viewing stuff*/

        private List<GameObject> enemyList;
        private Dictionary<GameObject, SpriteRenderer> enemyRenderers;

        private Dictionary<GameObject, Animator> enemyAnimators;

        //private Dictionary<GameObject, Transform> enemyTextureTransforms;
        private Dictionary<GameObject, Transform> enemyViewChecks;

        private string levelName;

        private const int layerTmpEnemy = 13;
        private const int layerViewChecksDefault = 14;

        private string pathCommonEnemy, pathBoss;

        public void Awake()
        {
            //la lista è completa SOLO all'inizio. Poi diventa outdated e il sistema si rompe
            //enemyList = enemyBase.GetAllEnemies(); //todo gestire nel caso volessimo aggiungere nemici
            enemyList = new List<GameObject>();
            enemyRenderers = new Dictionary<GameObject, SpriteRenderer>();
            enemyAnimators = new Dictionary<GameObject, Animator>();
            enemyViewChecks = new Dictionary<GameObject, Transform>();
            
            //Determina il nome del livello per determinare la cartella
            levelName = "Level" + Values.GetCurrentLevel();
            pathCommonEnemy = "Enemies/" + levelName + "/";
            pathBoss = "Enemies/" + levelName + "/Boss/";

            //Update iniziale della enemyList
            foreach (GameObject enemy in enemyList)
            {
                enemyRenderers.Add(enemy, enemy.GetComponentInChildren<SpriteRenderer>());
                enemyAnimators.Add(enemy, enemy.transform.Find("Texture").GetComponent<Animator>());
                //enemyTextureTransforms.Add(enemy, enemy.transform.Find("Texture"));
                enemyViewChecks.Add(enemy, enemy.transform.Find("ViewCheck"));
            }
        }

        private void FixedUpdate()
        {
            if (enemyList.Count == 0) return;

            foreach (GameObject enemy in enemyList)
            {
                //Check esistenza nemico
                if (enemy)
                {
                    var enemyViewCheck = enemyViewChecks[enemy];
                    SetChildLayers(enemyViewCheck, layerTmpEnemy);

                    if (Physics.Linecast(transform.position, enemy.transform.position, out RaycastHit rayEnemySprite,
                        LayerMask.GetMask("tmpEnemy")))
                    {
                        SetChildLayers(enemyViewCheck, layerViewChecksDefault); //reset a ViewCheckDefault
                        SpriteRenderer enemyRenderer = enemyRenderers[enemy];
                        Animator enemyAnimator = enemyAnimators[enemy];
                        ManageSprites(enemyRenderer, enemyAnimator,
                            enemy.Equals(Values.GetCurrentBoss()) ? pathBoss : pathCommonEnemy, rayEnemySprite);
                    }
                }
            }
        }


        public void AddEnemyToEnemyList(GameObject enemy)
        {
            enemyList.Add(enemy);
            enemyRenderers.Add(enemy, enemy.GetComponentInChildren<SpriteRenderer>());
            enemyAnimators.Add(enemy, enemy.transform.Find("Texture").GetComponent<Animator>());
            enemyViewChecks.Add(enemy, enemy.transform.Find("ViewCheck"));
        }

        private void SetChildLayers(Transform t, int layer)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Transform child = t.GetChild(i);
                child.gameObject.layer = layer;
            }
        }

        private void ManageSprites(SpriteRenderer enemyRenderer, Animator enemyAnimator, string path, RaycastHit ray)
        {
            RuntimeAnimatorController tmp = null;

            enemyRenderer.flipX = !(ray.collider.name.Equals("Right") ||
                                    ray.collider.name.Equals("DiagFrontRight") ||
                                    ray.collider.name.Equals("DiagBackRight"));

            switch (ray.collider.name)
            {
                case "Front":
                case "Left":
                case "Back":
                case "DiagFrontLeft":
                case "DiagBackLeft":
                    tmp = Resources.Load(path + ray.collider.name + "_AnimController") as
                        RuntimeAnimatorController;
                    break;

                case "Right":
                    tmp = Resources.Load(path + "Left_AnimController") as RuntimeAnimatorController;
                    break;
                case "DiagFrontRight":
                    tmp = Resources.Load(
                        path + "DiagFrontLeft_AnimController") as RuntimeAnimatorController;
                    break;
                case "DiagBackRight":
                    tmp = Resources.Load(path + "DiagBackLeft_AnimController") as RuntimeAnimatorController;
                    break;
                default:
                    tmp = Resources.Load(path + "Front") as RuntimeAnimatorController;
                    break;
            }


            //FIX PER NEMICI CON SINGOLO SPRITE
            // //todo in questi casi rinomiamo i vari view checks in front e basta, è un po monco sto sistema
            // if (tmp == null)
            // {
            //     print("tmp è null");
            //     tmp = Resources.Load(path + "Front") as RuntimeAnimatorController;
            // }

            enemyAnimator.runtimeAnimatorController = tmp;
        }
    }
}