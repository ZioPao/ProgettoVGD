using Player;
using UnityEngine;

namespace Utility
{
    public class LeverScript : MonoBehaviour, IInteractableMidGame
    {
        [SerializeField] private string objectName;
        [SerializeField] private bool forcePosition = false;

        [SerializeField] private float xValue, yValue, zValue;

        private bool isMoving;

        //private LeverStatus status;
        private Transform obj;
        private Transform movingPiece;

        private Quaternion correctRotation;
        private Vector3 correctPosition;
        private Quaternion doorRotation;

        public void Awake()
        {
            //print("Instanziato lever!");
            isMoving = false;
            movingPiece = transform.Find("movingPiece");

            if (forcePosition)
            {
                correctPosition = new Vector3(xValue, yValue, zValue);
            }
            else
            {
                correctRotation = Quaternion.Euler(movingPiece.eulerAngles.x, movingPiece.eulerAngles.y, -40f);
                correctPosition = new Vector3(movingPiece.position.x + 0.261f, movingPiece.position.y - 0.058f,
                    movingPiece.position.z);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // -0.176;
            // 0.542;
            // -0.019;
            //
            // 0.137;
            // 0.484;
            // -0.021;
            //
            // +261;
            // +58;

            InteractableBehaviour();
        }

        public void InteractableBehaviour()
        {
            if (Values.GetIsUsingLever())
            {
                //Play Sound
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.LeverActivate);

                isMoving = true;
                Values.SetIsUsingLever(false);
                this.tag = Values.interactableOverTag; //To disable the "interact with e" message
            }

            if (isMoving)
            {
                movingPiece.position = Vector3.Slerp(movingPiece.position, correctPosition, Time.deltaTime * 10f);
                movingPiece.rotation = Quaternion.Slerp(movingPiece.rotation, correctRotation, Time.deltaTime * 10f);

                float diff = Mathf.Abs(movingPiece.position.sqrMagnitude - correctPosition.sqrMagnitude);

                if (diff < 1)
                {
                    obj = GameObject.Find(objectName).transform;
                    doorRotation = Quaternion.Euler(obj.eulerAngles.x, obj.eulerAngles.y, -90f);

                    obj.rotation = doorRotation;
                    //todo credo sia inutile
                    //this.tag = "InteractableOver"; //To disable the "interact with e" message, again just in case

                    this.enabled = false; //end the script
                }
            }
        }

        public void ForceActivation()
        {
            Awake();

            obj = GameObject.Find(objectName).transform;
            movingPiece.position = correctPosition;
            movingPiece.rotation = correctRotation;
            obj.rotation =
                Quaternion.Euler(obj.eulerAngles.x, obj.eulerAngles.y, -90f); //todo maybe broken with other objects
            this.tag = Values.interactableOverTag; //To disable the "interact with e" message, again just in case
        }
    }
}