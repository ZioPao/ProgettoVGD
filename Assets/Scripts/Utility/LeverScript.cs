using Player;
using UnityEngine;

namespace Utility
{
    public class Lever : MonoBehaviour
    {
        [SerializeField]
        private string doorName;

        private bool isReloading = false;

        private LeverStatus status;
        private Transform door;
        private Transform movingPiece;
    
        private Quaternion correctRotation;
        private Vector3 correctPosition;


        void Awake()
        {

            if (!isReloading)
            {
                status = new LeverStatus();
                status.SetIsMoving(false);
                status.SetDoorName(doorName);
                status.SetMovingPiece(transform.Find("movingPiece"));
            
            }

            movingPiece = status.GetMovingPiece();
            correctRotation = Quaternion.Euler(movingPiece.eulerAngles.x, movingPiece.eulerAngles.y, -40f);
            correctPosition = new Vector3(movingPiece.position.x + 0.261f, movingPiece.position.y - 0.058f, movingPiece.position.z);
            door = GameObject.Find(status.GetDoorName()).transform;
            
         
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
                status.SetIsMoving(true);
                Values.SetIsUsingLever(false);
                this.tag = "Untagged";        //To disable the "interact with e" message
            }

            if (status.GetIsMoving())
            {
                movingPiece.position = Vector3.Slerp(movingPiece.position, correctPosition, Time.deltaTime * 10f);
                movingPiece.rotation = Quaternion.Slerp(movingPiece.rotation, correctRotation, Time.deltaTime * 10f);

                float diff = Mathf.Abs(movingPiece.position.sqrMagnitude - correctPosition.sqrMagnitude);
            
                if (diff < 1)
                {
                    var doorRotation = Quaternion.Euler(door.eulerAngles.x, door.eulerAngles.y, -90f);
                    door.rotation = doorRotation;

                    this.enabled = false;        //end the script

                }

            }
        }
    }
}
