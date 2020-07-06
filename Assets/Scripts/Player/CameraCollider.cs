using UnityEngine;

namespace Player
{
    public class CameraCollider : MonoBehaviour
    {
        private CameraScript cs;

        private const string WaterTag = "Water";

        private void Start()
        {
            cs = GetComponentInParent<CameraScript>();
        }

 

        private void OnTriggerEnter(Collider c)
        {
            if (c.gameObject.CompareTag(WaterTag))
                cs.SetCameraStatus(true);
        }

        private void OnTriggerExit(Collider c)
        {
            if (c.gameObject.CompareTag(WaterTag))
                cs.SetCameraStatus(false);
        }


    }
}
