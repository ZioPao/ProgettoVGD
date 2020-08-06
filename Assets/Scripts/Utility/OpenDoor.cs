using Player;
using UnityEngine;

namespace Utility
{
    public class OpenDoor : MonoBehaviour, IInteractableMidGame
    {
        private bool isOpening;
        private Quaternion correctRotation;

        void Awake()
        {
            isOpening = false;
            correctRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -90f);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            InteractableBehaviour();
        }

        public void InteractableBehaviour()
        {
            if (Values.GetIsUsingDoor() && Values.GetHasKey())
            {
                //Play Sound
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.DoorOpen);

                isOpening = true;
                Values.SetIsUsingDoor(false);
                Values.SetHasKey(false);
                tag = Values.interactableOverTag;     //To disable the "interact with e" message
            }

            if (Values.GetIsUsingDoor() && !Values.GetHasKey())
            {
                //Play Sound
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.DoorLocked);
                Values.SetIsUsingDoor(false);
            }

            if (isOpening)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, correctRotation, Time.deltaTime * 10f);

                if (Quaternion.Angle(transform.rotation, correctRotation) <= 0.05f)
                {
                    this.enabled = false;
                }
            }

        }

        public void ForceActivation()
        {
            transform.rotation = correctRotation;
            tag = Values.interactableOverTag;     //To disable the "interact with e" message
            enabled = false;
        }

        public bool GetIsEnabled()
        {
            return enabled;
        }
    }
}