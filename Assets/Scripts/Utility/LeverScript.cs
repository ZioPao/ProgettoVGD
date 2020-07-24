using Player;
using UnityEngine;

namespace Utility
{
    public class LeverScript : MonoBehaviour
    {
        [SerializeField] private string doorName;

        private bool isMoving;

        //private LeverStatus status;
        private Transform door;
        private Transform movingPiece;

        private Quaternion correctRotation;
        private Vector3 correctPosition;
        private Quaternion doorRotation;
        
        public void Awake()
        {

            //print("Instanziato lever!");
            isMoving = false;
            movingPiece = transform.Find("movingPiece");

            correctRotation = Quaternion.Euler(movingPiece.eulerAngles.x, movingPiece.eulerAngles.y, -40f);
            correctPosition = new Vector3(movingPiece.position.x + 0.261f, movingPiece.position.y - 0.058f,
                movingPiece.position.z);
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


            if (Values.GetIsUsingLever())
            {
                isMoving = true;
                Values.SetIsUsingLever(false);
                this.tag = "InteractableOver"; //To disable the "interact with e" message
            }

            if (isMoving)
            {
                movingPiece.position = Vector3.Slerp(movingPiece.position, correctPosition, Time.deltaTime * 10f);
                movingPiece.rotation = Quaternion.Slerp(movingPiece.rotation, correctRotation, Time.deltaTime * 10f);

                float diff = Mathf.Abs(movingPiece.position.sqrMagnitude - correctPosition.sqrMagnitude);

                if (diff < 1)
                {
                    door = GameObject.Find(doorName).transform;
                    doorRotation = Quaternion.Euler(door.eulerAngles.x, door.eulerAngles.y, -90f);

                    door.rotation = doorRotation;
                    this.tag = "InteractableOver"; //To disable the "interact with e" message, again just in case

                    this.enabled = false; //end the script
                }
            }
        }

        public void ForceActivation()
        {
            Awake();        //Reinit just in case
            //print("forzato lever");
            door = GameObject.Find(doorName).transform;
            movingPiece.position = correctPosition;
            movingPiece.rotation = correctRotation;
            door.rotation = Quaternion.Euler(door.eulerAngles.x, door.eulerAngles.y, -90f);
            this.tag = "InteractableOver"; //To disable the "interact with e" message, again just in case
            

            //Destroy(this);        //Non so che altro penasre

        }
    }
}