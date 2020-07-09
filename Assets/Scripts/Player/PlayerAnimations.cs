﻿using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Player
{
    public class PlayerAnimations : MonoBehaviour
    {

        private Animator anim;
        
        

        //IDs
        private static readonly int IsRunningAnim = Animator.StringToHash("isRunning");
        private static readonly int IsShootingAnim = Animator.StringToHash("isShooting");
    
    
        //Textures

        private Sprite pistolBaseSprite, pistolShootingSprite;
        private Texture2D knifeBase, knifeAttack;
        
        
        //renderer
        private SpriteRenderer pistolRenderer = null;
        private SpriteRenderer knifeRenderer = null;

        void Start()
        {
            anim = GetComponent<Animator>();

            //Pistol

            pistolBaseSprite = Resources.Load<Sprite>("PlayerWeapons/pistol");
            pistolShootingSprite = Resources.Load<Sprite>("PlayerWeapons/pistolShooting");
            
        }

        // Update is called once per frame
        void Update()
        {
            SetAnimations();
        }

        private void SetAnimations()
        { 
            anim.SetBool(IsRunningAnim, Values.GetIsRunning());
            anim.SetBool(IsShootingAnim, Values.GetIsAttacking()[Values.GetCurrentWeapon()]);

            if (Values.GetCurrentWeapon().Equals(Values.WeaponEnum.Pistol))
            {
                SetupPistolAnimations();
            }


        }


        private void SetupPistolAnimations()
        {
        
            //PlayerPistol
            
            //should be run only one time


            if (pistolRenderer == null)
            {
                pistolRenderer = Values.GetWeaponObjects()[Values.WeaponEnum.Pistol].GetComponent<SpriteRenderer>();
            }
            
            int currentBullets = Values.GetCurrentAmmo()[Values.WeaponEnum.Pistol];
            if ((Utility.TimerController.GetCurrentTime()[TimerController.PISTOLATTACK_K] > 0)  && currentBullets >= 0)
            {
                pistolRenderer.sprite = pistolShootingSprite;        //Change texture
            }
            else 
            {
                 pistolRenderer.sprite = pistolBaseSprite;
            }

        }
    
    }
}
