using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Logic.Player
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 100;
        [SerializeField] private Transform player = null; //Sarebbe il player controller


        private float maxY;

        /*Enemy viewing stuff*/
        private EnemyBase enemyBase;
        private GameObject[] enemyList;

        private List<MeshRenderer> enemyRendererList;
        private List<Transform> enemyTextureTransformList;


        /*Graphical stuff*/
        private bool isCameraInWater;
        private PostProcessVolume post;
        private ColorGrading colorGrading;
        private LensDistortion lensDistortion;

        private void Start()
        {
            enemyBase = GameObject.Find("Enemies").GetComponent<EnemyBase>();
            enemyList = enemyBase.GetAllEnemies(); //todo gestire nel caso volessimo aggiungere nemici

            enemyRendererList = new List<MeshRenderer>();
            enemyTextureTransformList = new List<Transform>();

            foreach (GameObject enemy in enemyList)
            {
                enemyTextureTransformList.Add(enemy.transform.Find("Texture"));
                enemyRendererList.Add(enemy.GetComponentInChildren<MeshRenderer>());
            }


            post = GetComponent<PostProcessVolume>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            CameraRotation();
            ManageSpriteViewing();

            /*Adds effects based on some bools*/
            post.profile.TryGetSettings(out colorGrading);
            post.profile.TryGetSettings(out lensDistortion);

            colorGrading.enabled.value = isCameraInWater;
            lensDistortion.enabled.value = isCameraInWater;
        }

        private void CameraRotation()
        {
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

            //Check clamping
            maxY += mouseY;

            if (maxY > 90.0f)
            {
                maxY = 90.0f;
                mouseY = 0.0f;
                SetLockCamera(270.0f);
            }
            else if (maxY < -90.0f)
            {
                maxY = -90.0f;
                mouseY = 0.0f;
                SetLockCamera(90.0f);
            }


            //Spostamento effettivo
            transform.Rotate(Vector3.left * mouseY);

            //Rotazione X

            player.Rotate(Vector3.up * mouseX);
        }

        private void SetLockCamera(float value)
        {
            var transformCopy = transform;

            Vector3 eulerRotation = transformCopy.eulerAngles;
            eulerRotation.x = value; //La blocca
            transformCopy.eulerAngles = eulerRotation; //Setta la rotazione del player
        }


        /* Raycasting for enemy sprites*/
        private void ManageSpriteViewing()
        {
            if (enemyList.Length == 0) return;

            /*todo è estremamente WIP. Da inserire gestione animazioni, un sistema più decente per il caricamento delle texture, e potenzialmente una marea di altra roba che ora non mi viene in mente*/

            //Texture[] enemyTexture = Resources.LoadAll<Texture>("Assets/Textures/Enemies/Level1");

            int counter = 0;
            foreach (GameObject enemy in enemyList)
            {
                Debug.DrawLine(transform.position, enemy.transform.position);
                //Check esistenza nemico
                if (enemy)
                {
                    if (Physics.Linecast(transform.position, enemy.transform.position, out RaycastHit rayEnemySprite,
                        LayerMask.GetMask("Enemy")))
                    {
                        //Ci si assicura che sia il nemico che stiamo osservando quello che ci interessa
                        if (enemy.name.Equals(rayEnemySprite.transform.parent.parent.name))
                        {
                            Renderer enemyRenderer = enemyRendererList[counter];
                            Transform enemyTextureTransform = enemyTextureTransformList[counter];


                            switch (rayEnemySprite.collider.name)
                            {
                                case "Front":
                                    enemyTextureTransform.localScale = new Vector3(0.4f, 1, 1);

                                    enemyRenderer.material.mainTexture =
                                        Resources.Load<Texture2D>("Enemies/Level1/enemy_idle_front");
                                    break;
                                case "Left":
                                    enemyTextureTransform.localScale = new Vector3(0.6f, 1, 1);
                                    enemyRenderer.material.mainTexture =
                                        Resources.Load<Texture>("Enemies/Level1/enemy_idle_left");
                                    break;
                                case "Right":
                                    enemyTextureTransform.localScale = new Vector3(0.6f, 1, 1);
                                    enemyRenderer.material.mainTexture =
                                        Resources.Load<Texture>("Enemies/Level1/enemy_idle_right");
                                    break;
                                case "DiagFrontRight":
                                    enemyTextureTransform.localScale = new Vector3(0.5f, 1, 1);

                                    enemyRenderer.material.mainTexture =
                                        Resources.Load<Texture>("Enemies/Level1/enemy_idle_diag_front_right");
                                    break;
                                case "DiagFrontLeft":
                                    enemyTextureTransform.localScale = new Vector3(0.5f, 1, 1);

                                    enemyRenderer.material.mainTexture =
                                        Resources.Load<Texture>("Enemies/Level1/enemy_idle_diag_front_left");
                                    break;
                                case "DiagBackRight":
                                    enemyTextureTransform.localScale = new Vector3(0.5f, 1, 1);

                                    enemyRenderer.material.mainTexture =
                                        Resources.Load<Texture>("Enemies/Level1/enemy_idle_diag_back_right");
                                    break;
                                case "DiagBackLeft":
                                    enemyTextureTransform.localScale = new Vector3(0.5f, 1, 1);

                                    enemyRenderer.material.mainTexture =
                                        Resources.Load<Texture>("Enemies/Level1/enemy_idle_diag_back_left");
                                    break;
                                case "Back":
                                    enemyTextureTransform.localScale = new Vector3(0.4f, 1, 1);

                                    enemyRenderer.material.mainTexture =
                                        Resources.Load<Texture>("Enemies/Level1/enemy_idle_back");
                                    break;
                            }
                        }
                    }
                }

                counter++;
            }
        }


        //Getters

        public bool IsCameraUnderWater()
        {
            return isCameraInWater;
        }

        //Setters
        public void SetCameraStatus(bool isUnderWater)
        {
            isCameraInWater = isUnderWater;
        }
    }
}