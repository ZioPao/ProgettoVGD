using UnityEngine;
using UnityEngine.UI;

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
        
        //timers
        private float timerAnimationMax = 0.25f;
        private float timerAnimation;        //TODO da legare con lo shooting effettivo. Al momento è totalmente sganciato e causa solo problemi
        
        void Start()
        {
            anim = GetComponent<Animator>();

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
            anim.SetBool(IsRunningAnim, Values.GetIsRunning());
            anim.SetBool(IsShootingAnim, Values.GetIsShooting());

            if (Values.GetIsShooting())
                timerAnimation = timerAnimationMax;        //start timer
        
            if (Values.GetCurrentWeapon().Equals(Values.WeaponEnum.Pistol))
                SetupPistolAnimations();
            

        }


        private void SetupPistolAnimations()
        {
        
            //PlayerPistol
            
            //should be run only one time


            if (pistolRenderer == null)
            {
                pistolRenderer = Values.GetWeaponObjects()[Values.WeaponEnum.Pistol].GetComponent<SpriteRenderer>();
            }


            int currentBullets = Values.GetWeaponScripts()[Values.WeaponEnum.Pistol].GetCurrentBulletsInMag();
            print(currentBullets);
            if ((Values.GetIsShooting() || timerAnimation > 0f)  && currentBullets > 0)
            {
                pistolRenderer.sprite = pistolShootingSprite;        //Change texture
                timerAnimation -= Time.deltaTime;
            }
            else 
            {
                 pistolRenderer.sprite = pistolBaseSprite;
            }

        }
    
    }
}
