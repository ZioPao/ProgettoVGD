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
        private Dictionary<GameObject, Transform> enemyTextureTransforms;

        private string levelName;

        public void Awake()
        {
            //la lista è completa SOLO all'inizio. Poi diventa outdated e il sistema si rompe
            //enemyList = enemyBase.GetAllEnemies(); //todo gestire nel caso volessimo aggiungere nemici
            enemyList = new List<GameObject>();
            enemyRenderers = new Dictionary<GameObject, SpriteRenderer>();
            enemyAnimators = new Dictionary<GameObject, Animator>();

            enemyTextureTransforms = new Dictionary<GameObject, Transform>();

            //Determina il nome del livello per determinare la cartella
            //todo da inserire
            levelName = "Level" + Values.GetCurrentLevel();

            foreach (GameObject enemy in enemyList)
            {
                enemyRenderers.Add(enemy, enemy.GetComponentInChildren<SpriteRenderer>());
                enemyAnimators.Add(enemy, enemy.transform.Find("Texture").GetComponent<Animator>());

                enemyTextureTransforms.Add(enemy, enemy.transform.Find("Texture"));
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (enemyList.Count == 0) return;
            
            //Texture[] enemyTexture = Resources.LoadAll<Texture>("Assets/Textures/Enemies/Level1");


            foreach (GameObject enemy in enemyList)
            {
                //Check esistenza nemico
                if (enemy)
                {
                    if (Physics.Linecast(transform.position, enemy.transform.position, out RaycastHit rayEnemySprite,
                        LayerMask.GetMask("Enemy")))
                    {
                        //Per evitare di fare controlli pesanti, facciamo un check al nome per determinare se stiamo controllando il nemico corretto
                        //!!!!!!!!!!!!! TUTTI I NEMICI DEVONO AVERE NOMI DIVERSI !!!!!!!
                        if (enemy.name.Equals(rayEnemySprite.transform.parent.parent.name))
                        {
                            SpriteRenderer enemyRenderer = enemyRenderers[enemy];
                            Animator enemyAnimator = enemyAnimators[enemy];
                            //float animationTimer = enemy.GetComponent<EnemyBase>().GetAnimationTimer();    //todo rendi piu carino

                            string path = null;


                            if (enemy.Equals(Values.GetCurrentBoss()))
                            {
                                
                                //eccezione per boss 1 
                                path = "Enemies/" + levelName + "/Boss/";

                            }
                            else
                                path = "Enemies/" + levelName + "/";


                            enemyRenderer.flipX = !(rayEnemySprite.collider.name.Equals("Right") ||
                                                   rayEnemySprite.collider.name.Equals("DiagFrontRight") ||
                                                   rayEnemySprite.collider.name.Equals("DiagBackRight"));

                            RuntimeAnimatorController tmp = null;
                            
                            switch (rayEnemySprite.collider.name)
                            {
                                case "Front":
                                case "Left":
                                case "Back":
                                case "DiagFrontLeft":
                                case "DiagBackLeft":
                                    tmp = Resources.Load(path + rayEnemySprite.collider.name + "_AnimController") as
                                        RuntimeAnimatorController;
                                    break;

                                case "Right":
                                    tmp = Resources.Load(path + "Left_AnimController") as RuntimeAnimatorController;
                                    break;
                                case "DiagFrontRight":
                                    tmp = Resources.Load(path + "DiagFrontLeft_AnimController") as RuntimeAnimatorController;
                                    break;
                                case "DiagBackRight":
                                    tmp = Resources.Load(path + "DiagBackLeft_AnimController") as RuntimeAnimatorController;
                                    break;
                                default:
                                    tmp = Resources.Load(path + "Front") as RuntimeAnimatorController;
                                    break;
                            }


                            if (tmp == null)
                            {
                                print("tmp è null");
                                tmp = Resources.Load(path + "Front") as RuntimeAnimatorController;
                                

                            }

                            enemyAnimator.runtimeAnimatorController = tmp;
                        }
                    }
                }
            }
        }


        public void AddEnemyToEnemyList(GameObject enemy)
        {
            enemyList.Add(enemy);
            enemyRenderers.Add(enemy, enemy.GetComponentInChildren<SpriteRenderer>());
            enemyAnimators.Add(enemy, enemy.transform.Find("Texture").GetComponent<Animator>());
        }
    }
}