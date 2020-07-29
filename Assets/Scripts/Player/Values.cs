using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public static class Values
    {
        
        //Hard Values (May be modified by upgrades)
        
        private static float boostSpeed = 3f;
        private static float jumpForce = 40f;
        private static float movementSpeed = 2f;
        private static float normalMass = 2.45f;
        private static float jumpMass = 10f;
        private static float normalDrag = 8f;
        private static float jumpDrag = 0f;
        
        private static float maxHealth = 100f;
        private static float maxStamina = 100f;
        private static float maxOxygen = 100f;
        
        private static float rigidBodyDefaultMass = 2.45f;
        
        private static float projectileDistance = 100f;
        private static float meleeDistance = 40f;
        private static float interactionDistance = 5f;
        private static float pickupDistance = 5f;
        
        //Player Stats
        private static float health;
        private static float stamina;
        private static float oxygen;
        private static float maxSpeed = 10f;
        
        //State Definers
        //Collisions
        private static bool isGrounded;
        private static bool isTouchingWall;
        private static bool isTouchingWallWithHead = false;
        private static float lastGoodYPosition;
        private static bool isInWater = false;
        //Actions
        //private static bool isMoving = false; //Da implementare
        private static bool isFrozen = false;
        private static bool isMoving = false;
        private static bool isRunning = false;
        private static bool isReloading = false;
        private static bool isReadingSign = false;
        private static bool isInteracting = false;
        private static bool isNearInteractable = false;
        private static bool isNearPickup = false;
        private static bool isUsingDoor = false;
        private static bool isUsingLever = false;
        private static bool hasKey = false;

        private static bool hasInteractedWithWinObject = false;

        //Raycasting
        private static float raycastLength = 5f;
        private static float raycastSpread = 2f;//0.08f;
        
        //Transform
        private static Transform _playerTransform;
        
        
        //Enemy Sprite manager
        private static EnemySpritesManager _enemySpritesManager;
        
        //Rigidbody
        private static Rigidbody rb;
        
        //Collider
        private static CapsuleCollider collider;
        
        //Saving 
        private static bool isLoadingSave;
        
        //Weapon Types
        public enum WeaponEnum
        {
            Knife,
            Pistol,
            SMG,
        }
        
        //Weapon Properties
        private static Dictionary<WeaponEnum, GameObject> weaponObjects;
        private static Dictionary<WeaponEnum, WeaponBehaviour> weaponBehaviours;
        private static Dictionary<WeaponEnum, bool> heldWeapons;
        private static Dictionary<WeaponEnum, int> currentAmmo;
        private static Dictionary<WeaponEnum, int> ammoReserve;
        private static Dictionary<WeaponEnum, int> reloadAmount;
        private static Dictionary<WeaponEnum, bool> isAttacking;
        private static WeaponEnum currentWeapon;
        
        //Timers
        private static bool isTimerLoaded = false;
        
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

        public static float GetJumpDrag()
        {
            return jumpDrag;
        }

        public static float GetNormaMass()
        {
            return normalMass;
        }

        public static float GetJumpMass()
        {
            return jumpMass;
        }

        public static float GetNormalDrag()
        {
            return normalDrag;
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
        public static float GetMeleeDistance()
        {
            return meleeDistance;
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

        public static float GetMaxSpeed()
        {
            return maxSpeed;
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
        public static bool GetIsFrozen()
        {
            return isFrozen;
        }
        public static bool GetIsMoving()
        {
            return isMoving;
        }
        public static bool GetIsRunning()
        {
            return isRunning;
        }
        public static bool GetIsReloading()
        {
            return isReloading;
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

        public static bool GetIsUsingDoor()
        {
            return isUsingDoor;
        }

        public static bool GetIsUsingLever()
        {
            return isUsingLever;
        }
        public static bool GetHasKey()
        {
            return hasKey;
        }

        public static bool GetHasInteractedWithWinObject()
        {
            return hasInteractedWithWinObject;
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

        public static Transform GetPlayerTransform()
        {
            return _playerTransform;
        }
        //Enemy Sprites Manager
        public static EnemySpritesManager GetEnemySpritesManager()
        {
            return _enemySpritesManager;
        }

        //Collider
        public static CapsuleCollider GetCollider()
        {
            return collider;
        }
        
        //Loading

        public static bool GetIsLoadingSave()
        {
            return isLoadingSave;
        }
        
        //Weapon Properties
        public static Dictionary<WeaponEnum, GameObject> GetWeaponObjects()
        {
            return weaponObjects;
        }
        public static Dictionary<WeaponEnum, WeaponBehaviour> GetWeaponBehaviours()
        {
            return weaponBehaviours;
        }
        public static Dictionary<WeaponEnum, bool> GetHeldWeapons()
        {
            return heldWeapons;
        }
        public static Dictionary<WeaponEnum, int> GetCurrentAmmo()
        {
            return currentAmmo;
        }
        public static Dictionary<WeaponEnum, int> GetAmmoReserve()
        {
            return ammoReserve;
        }
        public static Dictionary<WeaponEnum, int> GetReloadAmount()
        {
            return reloadAmount;
        }

        public static Dictionary<WeaponEnum, bool> GetIsAttacking()
        {
            return isAttacking;
        }
        public static WeaponEnum GetCurrentWeapon()
        {
            return currentWeapon;
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
        public static void SetMeleeDistance(float value)
        {
            meleeDistance = value;
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

        public static void AddHealth(float value)
        {

            if (health + value > 100)
                health = 100;
            else
                health += value;
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
        public static void SetIsFrozen(bool value)
        {
            isFrozen = value;
        }
        public static void SetIsMoving(bool value)
        {
            isMoving = value;
        }
        public static void SetIsRunning(bool value)
        {
            isRunning = value;
        }
        public static void SetIsReloading(bool value)
        {
            isReloading = value;
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

        public static void SetIsUsingDoor(bool value)
        {
            isUsingDoor = value;
        }

        public static void SetIsUsingLever(bool value)
        {
            isUsingLever = value;
        }
        public static void SetHasKey(bool value)
        {
            hasKey = value;
        }
        
        public static void SetHasInteractedWithWinObject(bool value)
        {
            hasInteractedWithWinObject = value;
        }
        
        //Rigidbody
        public static void SetRigidbody(Rigidbody value)
        {
            rb = value;
        }
        
        //Collider
        public static void SetCollider(CapsuleCollider value)
        {
            collider = value;
        }

        public static void SetPlayerTransform(Transform value)
        {
            _playerTransform = value;
        }
        public static void SetEnemySpritesManager(EnemySpritesManager value)
        {
            _enemySpritesManager = value;
        }
        
        //Saving
        public static void SetIsLoadingSave(bool value)
        {
            isLoadingSave = value;
        }
        //Weapon Properties
        public static void SetCurrentWeapon(WeaponEnum value)
        {
            currentWeapon = value;
        }
        
        
        
        /*Utility Methods*/

        //Stat Modifiers
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
        
        //Weapon Managers
        public static void InitializeWeaponObjects()
        {
            weaponObjects = new Dictionary<WeaponEnum, GameObject>();
        }
        public static void InitializeWeaponBehaviours()
        {
            weaponBehaviours = new Dictionary<WeaponEnum, WeaponBehaviour>();
        }
        public static void InitializeHeldWeapons()
        {
            heldWeapons = new Dictionary<WeaponEnum, bool>();
        }
        public static void InitializeCurrentAmmo()
        {
            currentAmmo = new Dictionary<WeaponEnum, int>();
        }
        public static void InitializeAmmoReserve()
        {
            ammoReserve = new Dictionary<WeaponEnum, int>();
        }
        public static void InitializeReloadAmount()
        {
            reloadAmount = new Dictionary<WeaponEnum, int>();
        }
        public static void InitializeIsAttacking()
        {
            isAttacking = new Dictionary<WeaponEnum, bool>();
        }
        
        public static void AddWeaponObject(WeaponEnum key, GameObject value)
        {
            weaponObjects.Add(key,value);
        }
        public static void AddWeaponBehaviour(WeaponEnum key, WeaponBehaviour value)
        {
            weaponBehaviours.Add(key, value);
        }
        public static void AddHeldWeapon(WeaponEnum key, bool value)
        {
            heldWeapons.Add(key, value);
        }
        public static void AddCurrentAmmo(WeaponEnum key, int value)
        {
            currentAmmo.Add(key, value);
        }
        public static void AddAmmoReserve(WeaponEnum key, int value)
        {
            ammoReserve.Add(key, value);
        }
        public static void AddReloadAmount(WeaponEnum key, int value)
        {
            reloadAmount.Add(key, value);
        }
        public static void AddIsAttacking(WeaponEnum key, bool value)
        {
            isAttacking.Add(key, value);
        }

        public static void IncrementCurrentAmmo(WeaponEnum key, int value)
        {
            currentAmmo[key] += value;
        }
        public static void DecrementCurrentAmmo(WeaponEnum key, int value)
        {
            currentAmmo[key] -= value;
        }
        public static void IncrementAmmoReserve(WeaponEnum key, int value)
        {
            ammoReserve[key] += value;
        }
        public static void DecrementAmmoReserve(WeaponEnum key, int value)
        {
            ammoReserve[key] -= value;
        }
        public static void SetIsAttacking(WeaponEnum key, bool value)
        {
            isAttacking[key] = value;
        }
        
        
        
        
        // Timer stuff
        
        public static void SetIsTimerLoaded(bool value)
        {
            isTimerLoaded = value;
        }

        public static IEnumerator WaitForTimer()
        {
            yield return new WaitUntil(() => isTimerLoaded);
        }

    }
}
