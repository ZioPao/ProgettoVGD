using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    //[SerializeField] private string mouseX, mouseY;
    [SerializeField] private float mouseSensitivity;
    // Start is called before the first frame update
    [SerializeField] private float maxY = 0;
    [SerializeField] private Transform player;      //Sarebbe il player controller
   

    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CameraRotation(); 
    }

    private void CameraRotation()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        //Check clamping
        maxY += mouseY;
        print(maxY);

        if (maxY > 90.0f)
        {
            maxY = 90.0f;
            mouseY = 0.0f;
            setLockCamera(270.0f);
        }
        else if (maxY < -90.0f)
        {
            maxY = -90.0f;
            mouseY = 0.0f;
            setLockCamera(90.0f);
        }


        //Spostamento effettivo
        transform.Rotate(Vector3.left * mouseY);

        //Rotazione X

        player.Rotate(Vector3.up * mouseX);

    }

    private void setLockCamera(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;        //La blocca
        transform.eulerAngles = eulerRotation;  //Setta la rotazione del player
    }
}

