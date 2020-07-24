using UnityEngine;

namespace Utility
{
    public class LeverStatus
    {
        private bool isMoving;
        private string doorName;
        private Transform movingPiece;


        public void SetIsMoving(bool value)
        {
            isMoving = value;
        }

        public void SetDoorName(string name)
        {
            doorName = name;
        }

        public void SetMovingPiece(Transform t)
        {
            movingPiece = t;
        }
        public bool GetIsMoving()
        {
            return isMoving;
        }

        public string GetDoorName()
        {
            return doorName;
        }

        public Transform GetMovingPiece()
        {
            return movingPiece;
        }
    }
}
