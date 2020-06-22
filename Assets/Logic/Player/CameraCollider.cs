using UnityEngine;

namespace Logic.Player
{
    public class CameraCollider : MonoBehaviour
    {

        CameraMovement cm;

        void Start()
        {
            cm = GetComponentInParent<CameraMovement>();
        }

 

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("Water"))
                cm.SetCameraStatus(true);
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.CompareTag("Water"))
                cm.SetCameraStatus(false);
        }


    }
}
