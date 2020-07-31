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
                movementSpeedMod *= 0.85f; //Decrease
                //rb.mass = rigidBodyDefaultMass + 15f;
            }

            /*Get movement*/
            
            float axisMovementVertical = Input.GetAxis("Vertical");
            float axisMovementHorizontal = Input.GetAxis("Horizontal");

            /*Boost*/
            bool shouldBeBoosting;
            if (Input.GetKey(KeyCode.LeftShift) && Values.GetIsGrounded() && (Values.GetRigidbody().velocity.magnitude > 0) && (axisMovementVertical > 0))
            {
                shouldBeBoosting = true;

                if ( Values.GetStamina() > 0)
                    movementSpeedMod = Values.GetMovementSpeed() * Values.GetBoostSpeed();
 
            }
            else
            {
                shouldBeBoosting = false;
            }

            
            /*Set the forward and right movement*/
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

            /*Fix diagonal movement*/
            if (forwardMovement != 0 && rightMovement != 0)
            {
                forwardMovement /= 1.42f; 
                rightMovement /= 1.42f;
            }
                
            /*Setup vectors*/
            Vector3 forwardVec = new Vector3(0,0,0);
            Vector3 rightVec = new Vector3(0,0,0);
            Vector3 addedGravity = new Vector3(0, 0, 0);

            if (axisMovementVertical != 0)
            {
                forwardVec = transform.forward * (forwardMovement * slopeSpeedMultiplier);
            }
            if (axisMovementHorizontal != 0)
            {
                rightVec = transform.right * (rightMovement * slopeSpeedMultiplier);

            }
            
            if (!Values.GetIsGrounded() && Values.GetIsMoving())
            {
                addedGravity.y = -0.1f;
            }
            
            if (Values.GetIsTouchingWall())
            {
                
                addedGravity.y = -10f;        //per buttarlo giù

            }
          
            movementVec = (forwardVec + rightVec + addedGravity);

        }

        public void MakeMovement()
        {

            var tmpRigidbody = Values.GetRigidbody();    //todo recupera all'init una volta e basta
            tmpRigidbody.AddForce(movementVec, ForceMode.VelocityChange);
            
            //Caps the speed
            if (tmpRigidbody.velocity.magnitude > Values.GetMaxSpeed())
            {
                tmpRigidbody.velocity = Values.GetRigidbody().velocity.normalized * Values.GetMaxSpeed();
            }
        }
        
        public void Jump()
        {
            SetPhysicsValues();
            
            if (Input.GetKey("space") && Values.GetStamina() >= 5 && (Values.GetIsGrounded() || Values.GetIsInWater()) && !Values.GetIsTouchingWallWithHead())
            {
                //Continue going towards that way
                float jumpForceMod = Values.GetJumpForce();
                
                //Decrease Stamina
                Values.DecreaseStamina(3);

                if (Values.GetIsInWater())
                {
                    jumpForceMod /= 2;
                }

                // if (Values.GetRigidbody().velocity.magnitude != 0)
                // {
                //     jumpForceMod *= Values.GetRigidbody().velocity.magnitude;
                // }
                // print(jumpForceMod);

                if (Values.GetIsRunning())
                    jumpForceMod *= 3;
                    

                Vector3 tmp = (transform.up * jumpForceMod);
                Values.GetRigidbody().AddForce(tmp, ForceMode.Impulse);
            }

            
        }

        private void SetPhysicsValues()
        {
            Values.GetRigidbody().drag = Values.GetIsGrounded() ? Values.GetNormalDrag() : Values.GetJumpDrag();
            Values.GetRigidbody().mass = Values.GetIsGrounded() ? Values.GetNormaMass() : Values.GetJumpMass();
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