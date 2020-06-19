using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private float mouseSensitivity = 100;
    [SerializeField] private Transform player = null;      //Sarebbe il player controller

    private PlayerController pc;
    private PostProcessVolume post;

    private float maxY;

    private bool isCameraInWater;
    private ColorGrading colorGrading;

    void Start()
    {

        pc = GetComponentInParent<PlayerController>();
        post = GetComponent<PostProcessVolume>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CameraRotation();

        /*Adds effects based on some bools*/
        post.profile.TryGetSettings(out colorGrading);
        colorGrading.enabled.value = isCameraInWater;

   
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
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Water"))
            isCameraInWater = true;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Water"))
            isCameraInWater = false;
    }

}

