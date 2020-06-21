using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    [SerializeField] private float mouseSensitivity = 100;
    [SerializeField] private Transform player = null;      //Sarebbe il player controller


    private float maxY;

    /*Enemy viewing stuff*/
    private EnemyBase enemyBase;
    private GameObject[] enemyList;
    /*Graphical stuff*/
    private bool isCameraInWater;
    private PostProcessVolume post;
    private ColorGrading colorGrading;
    private LensDistortion lensDistortion;

    void Start()
    {
        enemyBase = GameObject.Find("Enemies").GetComponent<EnemyBase>();
        enemyList = enemyBase.GetAllEnemies();      //todo gestire nel caso volessimo aggiungere nemici


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
        //print(maxY);

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
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;        //La blocca
        transform.eulerAngles = eulerRotation;  //Setta la rotazione del player
    }
    
    
    /* Raycasting for enemy sprites*/
    private void ManageSpriteViewing()
    {

        //todo dovrebbe sapere da prima che nemici sono
        RaycastHit rayEnemySprite = new RaycastHit();

        //Prende una lista di tutti i nemici. Seleziona solo quelli che sono distanti meno di tot metri
        if (enemyList.Length != 0)
        {
            //determina che nemici sono a seconda del livello
            //todo deve gestire anche animazioni
            Texture[] enemyTexture = Resources.LoadAll<Texture>("Assets/Textures/Enemies/Level1");
            foreach (GameObject enemy in enemyList)
            {
                //Debug.DrawLine(transform.position, enemy.transform.position);

                if (Physics.Linecast(transform.position, enemy.transform.position, out rayEnemySprite, LayerMask.GetMask("Enemy")))
                {
                    //print(rayEnemySprite.collider.name);
                    Renderer enemyRenderer = enemy.GetComponentInChildren<MeshRenderer>();
                    switch (rayEnemySprite.collider.name)
                    {
                        case "Front":
                            enemyRenderer.material.mainTexture = Resources.Load<Texture2D>("Enemies/Level1/enemy_idle_front");
                            break;
                        case "Left":
                            enemyRenderer.material.mainTexture = Resources.Load<Texture>("Enemies/Level1/enemy_idle_left");
                            break;
                        case "Right":
                            enemyRenderer.material.mainTexture = Resources.Load<Texture>("Enemies/Level1/enemy_idle_right");
                            
                            break;

                    }

                }


            }

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

