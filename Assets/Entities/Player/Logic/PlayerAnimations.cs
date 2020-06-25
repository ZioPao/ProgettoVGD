﻿using Logic.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Entities.Player.Logic
{
    public class PlayerAnimations : MonoBehaviour
    {
        private PlayerController playerController;
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
        
        //timers
        private float timerAnimationMax = 0.25f;
        private float timerAnimation;
        
        void Start()
        {
            anim = GetComponent<Animator>();
            playerController = GetComponentInParent<PlayerController>();
        
            //Pistol

            pistolBaseSprite = Resources.Load<Sprite>("PlayerWeapons/pistol");
            pistolShootingSprite = Resources.Load<Sprite>("PlayerWeapons/pistolShooting");

            
            //Timers
            timerAnimation = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            SetAnimations();
        }

        private void SetAnimations()
        { 
            anim.SetBool(IsRunningAnim, playerController.IsPlayerRunning());
            anim.SetBool(IsShootingAnim, playerController.IsPlayerShooting());

            if (playerController.IsPlayerShooting())
                timerAnimation = timerAnimationMax;        //start timer
        
            if (playerController.IsPistolInHand())
                SetupPistolAnimations();
            

        }


        private void SetupPistolAnimations()
        {
        
            //PlayerPistol
            
            //should be run only one time
            if (pistolRenderer == null)
                pistolRenderer = playerController.GetPistol().GetComponent<SpriteRenderer>();
            
            if (playerController.IsPlayerShooting() || timerAnimation > 0f)
            {
                pistolRenderer.sprite = pistolShootingSprite;        //Change texture
                timerAnimation -= Time.deltaTime;
            }else 
            {
                 pistolRenderer.sprite = pistolBaseSprite;
            }

        }
    
    }
}
