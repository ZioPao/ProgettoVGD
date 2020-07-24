using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField]
    private string doorName;

    private Transform door;
    private bool isMoving = false;
    private Transform movingPiece;
    
    private Quaternion correctRotation;
    private Vector3 correctPosition;


    void Start()
    {
        isMoving = false;

        door = GameObject.Find(doorName).transform;


        movingPiece = transform.Find("movingPiece");
        correctRotation = Quaternion.Euler(movingPiece.eulerAngles.x, movingPiece.eulerAngles.y, -40f);
        correctPosition = new Vector3(movingPiece.position.x + 0.261f, movingPiece.position.y - 0.058f, movingPiece.position.z);
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
            this.tag = "Untagged";        //To disable the "interact with e" message
        }

        if (isMoving)
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
