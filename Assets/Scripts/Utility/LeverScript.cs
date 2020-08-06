using Player;
using UnityEngine;

namespace Utility
{
    public class LeverScript : MonoBehaviour, IInteractableMidGame
    {
        [SerializeField] private string objectName;
        [SerializeField] private bool disableLinkedObject = false;

        [SerializeField] private bool forcePosition = false;

        [SerializeField] private float zRotation;


        private bool isMoving;

        //private LeverStatus status;
        private Transform obj;
        private Transform movingPiece;

        private Quaternion correctRotation;
        private Quaternion objRotation;

        public void Awake()
        {
            //print("Instanziato lever!");
            isMoving = false;
            movingPiece = transform.Find("movingPiecePivot");


            correctRotation = Quaternion.Euler(0,0, zRotation);
 
            // correctRotation = Quaternion.Euler(movingPiece.eulerAngles.x, movingPiece.eulerAngles.y, -40f);
            // correctPosition = new Vector3(movingPiece.position.x + 0.261f, movingPiece.position.y - 0.058f,
            //     movingPiece.position.z);

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

            //InteractableBehaviour();


            if (isMoving)
            {
                MoveLever();
            }
        }

        public void InteractableBehaviour()
        {
            //Play Sound
            Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.LeverActivate);
            isMoving = true;
            //Values.SetIsUsingLever(false);
            tag = Values.InteractableOverTag; //To disable the "interact with e" message
        }


        public void ForceActivation()
        {
            Awake();
            //obj indica la porta\cosa che si deve attivare
            obj = GameObject.Find(objectName).transform;
            movingPiece.rotation = correctRotation;
            obj.rotation =
                Quaternion.Euler(obj.eulerAngles.x, obj.eulerAngles.y, -90f); //todo maybe broken with other objects
            tag = Values.InteractableOverTag; //To disable the "interact with e" message, again just in case
        }

        public void MoveLever()
        {
            movingPiece.rotation = Quaternion.Slerp(movingPiece.rotation, correctRotation, Time.deltaTime * 10f);

            if (movingPiece.rotation.z - correctRotation.z <= 0.01f)
            {
                ActivateLinkedObject();
                enabled = false; //end the script

            }

        }

        private void ActivateLinkedObject()
        {
            obj = GameObject.Find(objectName).transform;

            if (disableLinkedObject)
            {
                obj.gameObject.SetActive(false);
            }
            else
            {
                objRotation = Quaternion.Euler(obj.eulerAngles.x, obj.eulerAngles.y, -90f);
                obj.rotation = objRotation; 
            }


        }

        public bool GetIsEnabled()
        {
            return enabled;
        }
    }
}