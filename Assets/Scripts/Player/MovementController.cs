using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class MovementController : MonoBehaviour
    {

        private Vector3 movementVec;

        public void SetupMovement()
        {
            float forwardMovement, rightMovement;
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
            
            /*Fix diagonal speed and check if player's boosting*/
            if (forwardMovement != 0 && rightMovement != 0)
            {
                forwardMovement /= 1.42f; //todo determinare se è sempre questo valore
                rightMovement /= 1.42f;
            }

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
            Vector3 forwardVec = transform.forward * (forwardMovement * slopeSpeedMultiplier);
            Vector3 rightVec = transform.right * (rightMovement * slopeSpeedMultiplier);
            movementVec = (forwardVec + rightVec);

        }

        public void MakeMovement()
        {

            if (Values.GetIsTouchingWallWithHead())
            {
                if (Values.GetRigidbody().position.y < Values.GetLastGoodYPosition())
                {
                    //lo rende talmente lento da farlo diventare un non problema
                    Values.GetRigidbody().MovePosition(transform.position + (movementVec / 4) * Time.fixedDeltaTime);
                }
                else
                {
                    Values.GetRigidbody().MovePosition(transform.position + movementVec * Time.fixedDeltaTime);
                }
                
            }
            else
            {
                float slopeAngleTmp = GetSlopeAngle();

                //ignore check if player is in water
                if (slopeAngleTmp > -50 && slopeAngleTmp <= 35 || Values.GetIsInWater())
                {
                    Values.GetRigidbody().MovePosition(transform.position + movementVec * Time.fixedDeltaTime);
                }
                else
                {
                    //Fa scendere forzatamente il giocatore
                    print("stuck boy");
                    Values.GetRigidbody().MovePosition(transform.position + new Vector3(0, -9.81f, 0) * Time.deltaTime);
                }
            }

        }

        public void Jump()
        {
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