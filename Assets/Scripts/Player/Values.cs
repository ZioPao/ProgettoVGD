using UnityEngine;

namespace Player
{
    public static class Values
    {
        
        //Hard Values (May be modified by upgrades)
        
        private static float boostSpeed = 2f;
        private static float jumpForce = 500f;
        private static float movementSpeed = 10f;
        
        private static float maxHealth = 100f;
        private static float maxStamina = 100f;
        private static float maxOxygen = 100f;
        
        private static float rigidBodyDefaultMass = 2.45f;
        
        private static float projectileDistance = 100f;    //Maybe change into weapon attribute
        private static float interactionDistance = 5f;
        private static float pickupDistance = 5f;
        
        //Player Stats
        private static float health;
        private static float stamina;
        private static float oxygen;
        private static float currentAmmo;
        private static float ammoReserve;
        
        //State Definers
        //Collisions
        private static bool isGrounded;
        private static bool isTouchingWall;
        private static bool isTouchingWallWithHead = false;
        private static float lastGoodYPosition;
        private static bool isInWater = false;
        //Actions
        //private static bool isMoving = false; //Da implementare
        private static bool isMoving = false;
        private static bool isRunning = false;
        private static bool isShooting = false;
        private static bool isReadingSign = false;
        private static bool isInteracting = false;
        private static bool isNearInteractable = false;
        private static bool isNearPickup = false;

        //Raycasting
        private static float raycastLength = 5f;
        private static float raycastSpread = 0.08f;
        
        //Rigidbody
        private static Rigidbody rb;
        
        
        
        /*GETTERS*/
        
        //Hard Values

        public static float GetBoostSpeed()
        {
            return boostSpeed;
        }
        public static float GetJumpForce()
        {
            return jumpForce;
        }
        public static float GetMovementSpeed()
        {
            return movementSpeed;
        }

        public static float GetMaxHealth()
        {
            return maxHealth;
        }
        public static float GetMaxStamina()
        {
            return maxStamina;
        }
        public static float GetMaxOxygen()
        {
            return maxOxygen;
        }

        public static float GetRigidBodyDefaultMass()
        {
            return rigidBodyDefaultMass;
        }

        public static float GetProjectileDistance()
        {
            return projectileDistance;
        }
        public static float GetInteractionDistance()
        {
            return interactionDistance;
        }
        public static float GetPickupDistance()
        {
            return pickupDistance;
        }
        
        //Player Stats

        public static float GetHealth()
        {
            return health;
        }
        public static float GetStamina()
        {
            return stamina;
        }
        public static float GetOxygen()
        {
            return oxygen;
        }
        public static float GetCurrentAmmo()
        {
            return currentAmmo;
        }
        public static float GetAmmoReserve()
        {
            return ammoReserve;
        }
        
        //State Definers
        //Collisions
        public static bool GetIsGrounded()
        {
            return isGrounded;
        }
        public static bool GetIsTouchingWall()
        {
            return isTouchingWall;
        }
        public static bool GetIsTouchingWallWithHead()
        {
            return isTouchingWallWithHead;
        }
        public static float GetLastGoodYPosition()
        {
            return lastGoodYPosition;
        }
        public static bool GetIsInWater()
        {
            return isInWater;
        }
        //Actions
        public static bool GetIsMoving()
        {
            return isMoving;
        }
        public static bool GetIsRunning()
        {
            return isRunning;
        }
        public static bool GetIsShooting()
        {
            return isShooting;
        }
        public static bool GetIsReadingSign()
        {
            return isReadingSign;
        }
        public static bool GetIsInteracting()
        {
            return isInteracting;
        }
        public static bool GetIsNearInteractable()
        {
            return isNearInteractable;
        }
        public static bool GetIsNearPickup()
        {
            return isNearPickup;
        }

        //Raycasting
        public static float GetRaycastLength()
        {
            return raycastLength;
        }
        public static float GetRaycastSpread()
        {
            return raycastSpread;
        }
        
        //Rigidbody
        public static Rigidbody GetRigidbody()
        {
            return rb;
        }

        
        
        /*SETTER*/
        
        //Hard Values

        public static void SetBoostSpeed(float value)
        {
            boostSpeed = value;
        }
        public static void SetJumpForce(float value)
        {
            jumpForce = value;
        }
        public static void SetMovementSpeed(float value)
        {
            movementSpeed = value;
        }

        public static void SetMaxHealth(float value)
        {
            maxHealth = value;
        }
        public static void SetMaxStamina(float value)
        {
            maxStamina = value;
        }
        public static void SetMaxOxygen(float value)
        {
            maxOxygen = value;
        }
        
        public static void SetProjectileDistance(float value)
        {
           projectileDistance = value;
        }
        public static void SetInteractionDistance(float value)
        {
            interactionDistance = value;
        }
        public static void SetPickupDistance(float value)
        {
            pickupDistance = value;
        }
        
        //Player Stats

        public static void SetHealth(float value)
        {
            if ((value <= maxHealth) && (value >= 0))
            {
                health = value;
            }
        }
        public static void SetStamina(float value)
        {
            if ((value <= maxStamina) && (value >= 0))
            {
                stamina = value;
            }
        }
        public static void SetOxygen(float value)
        {
            if ((value <= maxOxygen) && (value >= 0))
            {
                oxygen = value;
            }
        }
        public static void SetCurrentAmmo(float value)
        {
            currentAmmo = value;
        }
        public static void SetAmmoReserve(float value)
        {
            ammoReserve = value;
        }
        
        //State Definers
        //Collisions
        public static void SetIsGrounded(bool value)
        {
            isGrounded = value;
        }
        public static void SetIsTouchingWall(bool value)
        {
            isTouchingWall = value;
        }
        public static void SetIsTouchingWallWithHead(bool value)
        {
            isTouchingWallWithHead = value;
        }
        public static void SetLastGoodYPosition(float value)
        {
            lastGoodYPosition = value;
        }
        public static void SetIsInWater(bool value)
        {
            isInWater = value;
        }
        //Actions
        public static void SetIsMoving(bool value)
        {
            isMoving = value;
        }
        public static void SetIsRunning(bool value)
        {
            isRunning = value;
        }
        public static void SetIsShooting(bool value)
        {
            isShooting = value;
        }
        public static void SetIsReadingSign(bool value)
        {
            isReadingSign = value;
        }
        public static void SetIsInteracting(bool value)
        {
            isInteracting = value;
        }
        public static void SetIsNearInteractable(bool value)
        {
            isNearInteractable = value;
        }
        public static void SetIsNearPickup(bool value)
        {
            isNearPickup = value;
        }
        
        //Rigidbody
        public static void SetRigidbody(Rigidbody value)
        {
            rb = value;
        }
        
        
        /*Utility Methods*/

        public static void IncreaseHealth(float increment)
        {
            if ((health + increment) > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health += increment;
            }
        }
        public static void DecreaseHealth(float decrement)
        {
            if ((health - decrement) < 0)
            {
                health = 0;
            }
            else
            {
                health -= decrement;
            }
        }
        public static void IncreaseStamina(float increment)
        {
            if ((stamina + increment) > maxStamina)
            {
                stamina = maxStamina;
            }
            else
            {
                stamina += increment;
            }
        }
        public static void DecreaseStamina(float decrement)
        {
            if ((stamina - decrement) < 0)
            {
                stamina = 0;
            }
            else
            {
                stamina -= decrement;
            }
        }
        public static void IncreaseOxygen(float increment)
        {
            if ((oxygen + increment) > maxOxygen)
            {
                oxygen = maxOxygen;
            }
            else
            {
                oxygen += increment;
            }
        }
        public static void DecreaseOxygen(float decrement)
        {
            if ((oxygen - decrement) < 0)
            {
                oxygen = 0;
            }
            else
            {
                oxygen -= decrement;
            }
        }

    }
}
