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
        private Dictionary<GameObject, Transform> enemyTextureTransforms;

        private void Start()
        {
            
            //la lista è completa SOLO all'inizio. Poi diventa outdated e il sistema si rompe
            //enemyList = enemyBase.GetAllEnemies(); //todo gestire nel caso volessimo aggiungere nemici
            enemyList = new List<GameObject>();
            enemyRenderers = new Dictionary<GameObject, SpriteRenderer>();
            enemyTextureTransforms = new Dictionary<GameObject, Transform>();
            foreach (GameObject enemy in enemyList)
            {
                enemyRenderers.Add(enemy, enemy.GetComponentInChildren<SpriteRenderer>());
                enemyTextureTransforms.Add(enemy, enemy.transform.Find("Texture"));
            }

        }

        // Update is called once per frame
        private void FixedUpdate()
        {
         if (enemyList.Count == 0) return;

            /*todo è estremamente WIP. Da inserire gestione animazioni, un sistema più decente per il caricamento delle texture, e potenzialmente una marea di altra roba che ora non mi viene in mente*/

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
                            
                            switch (rayEnemySprite.collider.name)
                            {
                                case "Front":

                                    enemyRenderer.sprite =
                                        Resources.Load<Sprite>("Enemies/Level1/enemy_idle_front");
                                    break;
                                case "Left":
                                    enemyRenderer.sprite =
                                        Resources.Load<Sprite>("Enemies/Level1/enemy_idle_left");
                                    break;
                                case "Right":
                                    enemyRenderer.sprite =
                                        Resources.Load<Sprite>("Enemies/Level1/enemy_idle_right");
                                    break;
                                case "DiagFrontRight":

                                    enemyRenderer.sprite =
                                        Resources.Load<Sprite>("Enemies/Level1/enemy_idle_diag_front_right");
                                    break;
                                case "DiagFrontLeft":

                                    enemyRenderer.sprite =
                                        Resources.Load<Sprite>("Enemies/Level1/enemy_idle_diag_front_left");
                                    break;
                                case "DiagBackRight":

                                    enemyRenderer.sprite =
                                        Resources.Load<Sprite>("Enemies/Level1/enemy_idle_diag_back_right");
                                    break;
                                case "DiagBackLeft":

                                    enemyRenderer.sprite =
                                        Resources.Load<Sprite>("Enemies/Level1/enemy_idle_diag_back_left");
                                    break;
                                case "Back":

                                    enemyRenderer.sprite =
                                        Resources.Load<Sprite>("Enemies/Level1/enemy_idle_back");
                                    break;
                            }
                        }
                    }
                }

            }
        }
        
        public void AddEnemyToEnemyList(GameObject enemy)
        {
            enemyList.Add(enemy);
            enemyRenderers.Add(enemy, enemy.GetComponentInChildren<SpriteRenderer>());
            enemyTextureTransforms.Add(enemy, enemy.transform.Find("Texture"));
        }
        
    }
}
