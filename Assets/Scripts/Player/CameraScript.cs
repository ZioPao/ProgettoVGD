using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Player
{
    public class CameraScript : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 500;
        [SerializeField] private Transform player = null; //Sarebbe il player controller


        private float maxY;

        /*Graphical stuff*/
        private bool isCameraInWater;
        private PostProcessVolume post;
        private ColorGrading colorGrading;
        private LensDistortion lensDistortion;

        private void Start()
        {

            post = GetComponent<PostProcessVolume>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            if (!Values.GetIsFrozen())
            {
                CameraRotation();
            }
            
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