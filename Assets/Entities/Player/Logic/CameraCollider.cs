using Logic.Player;
using UnityEngine;

namespace Entities.Player.Logic
{
    public class CameraCollider : MonoBehaviour
    {
        private CameraMovement cm;

        private const string WaterTag = "Water";

        private void Start()
        {
            cm = GetComponentInParent<CameraMovement>();
        }

 

        private void OnTriggerEnter(Collider c)
        {
            if (c.gameObject.CompareTag(WaterTag))
                cm.SetCameraStatus(true);
        }

        private void OnTriggerExit(Collider c)
        {
            if (c.gameObject.CompareTag(WaterTag))
                cm.SetCameraStatus(false);
        }


    }
}
