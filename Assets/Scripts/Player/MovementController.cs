using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class MovementController : MonoBehaviour
    {

        private Vector3 movementVec;

        private float forwardMovement, rightMovement;
        
        public void SetupMovement()
        {
            float movementSpeedMod = Values.GetMovementSpeed();
            float slopeSpeedMultiplier = 1 - (GetSlopeAngle() / 90);

            /*In water*/
            
            if (Values.GetIsInWater())
            {
                //todo i broke something 
                movementSpeedMod *= 0.5f; //Decrease
                //rb.mass = rigidBodyDefaultMass + 15f;
            }

            /*Get movement*/
            
            float axisMovementVertical = Input.GetAxis("Vertical");
            float axisMovementHorizontal = Input.GetAxis("Horizontal");

            /*Boost*/
            
            //todo da rifare con nuovo movement controller
            bool shouldBeBoosting;
            if (Input.GetKey(KeyCode.LeftShift) && !Values.GetIsTouchingWall() && !Values.GetIsTouchingWallWithHead() &&
                (Values.GetRigidbody().velocity.magnitude > 0) && (axisMovementVertical > 0) && Values.GetStamina() >= 10)
            {
                movementSpeedMod = Values.GetMovementSpeed() * Values.GetBoostSpeed();
                shouldBeBoosting = true;
            }
            else
            {
                shouldBeBoosting = false;
            }
            
            
            forwardMovement = axisMovementVertical * movementSpeedMod;
            rightMovement = axisMovementHorizontal * movementSpeedMod;
            
            
            /*Set Movement Variables*/
            if (forwardMovement != 0 || rightMovement != 0)
            {
                Values.SetIsMoving(true);
                Values.SetIsRunning(shouldBeBoosting);
            }
            else
            {
                Values.SetIsMoving(false);
                Values.SetIsRunning(false);
            }

            /*Setup vectors*/
            Vector3 forwardVec = new Vector3(0,0,0);

            if (axisMovementVertical != 0)
            {
                forwardVec = transform.forward * (forwardMovement * slopeSpeedMultiplier);
            }
    
      

            Vector3 rightVec = new Vector3(0,0,0);
            if (axisMovementHorizontal != 0)
            {
                rightVec = transform.right * (rightMovement * slopeSpeedMultiplier);

            }
          
            movementVec = (forwardVec + rightVec);

        }

        public void MakeMovement()
        {

            // if (Values.GetIsTouchingWallWithHead())
            // {
            //     if (Values.GetRigidbody().position.y < Values.GetLastGoodYPosition())
            //     {
            //         //lo rende talmente lento da farlo diventare un non problema
            //         Values.GetRigidbody().AddForce(transform.position + (movementVec / 4) * Time.fixedDeltaTime);
            //     }
            //     else
            //     {
            //         Values.GetRigidbody().AddForce(transform.position + movementVec * Time.fixedDeltaTime);
            //     }
            //     
            // }
            // else
            // {
      
            Values.GetRigidbody().AddForce(movementVec, ForceMode.VelocityChange);
     
            // }

            CapSpeed();
        }

        public void CapSpeed()
        {
            {
                if (Values.GetRigidbody().velocity.magnitude > Values.GetMaxSpeed())
                {
                    Values.GetRigidbody().velocity = Values.GetRigidbody().velocity.normalized * Values.GetMaxSpeed();
                }
            }
        }

        public void Jump()
        {
            SetDrag();
            
            if (Input.GetKey("space") && Values.GetStamina() >= 5 && (Values.GetIsGrounded() || Values.GetIsInWater()) && !Values.GetIsTouchingWallWithHead())
            {
                //Continue going towards that way
                float jumpForceMod = Values.GetJumpForce();
                
                //Decrease Stamina
                Values.DecreaseStamina(1);

                if (Values.GetIsInWater())
                {
                    jumpForceMod /= 5;
                }

                Vector3 tmp = (transform.up * jumpForceMod);
                Values.GetRigidbody().AddForce(tmp, ForceMode.Force);
            }

            
        }

        private void SetDrag()
        {
            Values.GetRigidbody().drag = Values.GetIsGrounded() ? Values.GetNormalDrag() : Values.GetJumpDrag();
        }

        public float GetSlopeAngle()
        {
            float slopeAngle = 0;    //base value

            if (Physics.Raycast(Values.GetRigidbody().transform.position + new Vector3(Values.GetRaycastSpread(), 0, 0), Vector3.down, out RaycastHit raySlope1, Values.GetRaycastLength()))
            {
                if (Physics.Raycast(Values.GetRigidbody().transform.position - new Vector3(Values.GetRaycastSpread(), 0, 0), Vector3.down, out RaycastHit raySlope2, Values.GetRaycastLength()))
                {
                    slopeAngle = Mathf.Atan2(raySlope1.point.y - raySlope2.point.y, raySlope1.point.x - raySlope2.point.x) * 180 / Mathf.PI;
                }
            }
            return slopeAngle;
        }
        
    }
    
}