using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Player
{
    public class PlayerAnimations : MonoBehaviour
    {

        private Animator anim;
        private const string IDLE_WEAPON = "idle";
        private const string SHOOTING_WEAPON = "shooting";
        
        

        //IDs
        private static readonly int IsRunningAnim = Animator.StringToHash("isRunning");
        private static readonly int IsShootingAnim = Animator.StringToHash("isShooting");
    
    
        //Pistol
        private Sprite pistolIdleSprite, pistolShootingSprite;
        private Material pistolIdleMat, pistolShootingMat;
        private SpriteRenderer pistolRenderer;
        
        //SMG
        private Sprite smgIdleSprite, smgShootingSprite;
        private Material smgIdleMat, smgShootingMat;
        private SpriteRenderer smgRenderer;

        //Knife
        private Sprite knifeIdleSprite, knifeHittingSprite;
        private RuntimeAnimatorController knifeAnimation;
        private Material knifeIdleMat, knifeHittingMat;
        private SpriteRenderer knifeRenderer;
        private Animator knifeAnimator;
        private bool attackingWithKnife;
        
        
        void Start()
        {
            anim = GetComponent<Animator>();

            pistolRenderer = null;        //init sempre a null
            smgRenderer = null;
            knifeRenderer = null;

            attackingWithKnife = false;
            
            
            //KNIFE
            knifeIdleSprite = Resources.Load<Sprite>("PlayerWeapons/knife/Sprites/" + IDLE_WEAPON);
            knifeHittingSprite = Resources.Load<Sprite>("PlayerWeapons/knife/Sprites/" + SHOOTING_WEAPON);

            knifeIdleMat = Resources.Load<Material>("PlayerWeapons/knife/Mats/" + IDLE_WEAPON);
            knifeHittingMat = Resources.Load<Material>("PlayerWeapons/knife/Mats/" + SHOOTING_WEAPON);
            
            ///PISTOL

            pistolIdleSprite = Resources.Load<Sprite>("PlayerWeapons/pistol/Sprites/" + IDLE_WEAPON);
            pistolShootingSprite = Resources.Load<Sprite>("PlayerWeapons/pistol/Sprites/" + SHOOTING_WEAPON);

            pistolIdleMat = Resources.Load<Material>("PlayerWeapons/pistol/Mats/" + IDLE_WEAPON);
            pistolShootingMat = Resources.Load<Material>("PlayerWeapons/pistol/Mats/" + SHOOTING_WEAPON);
            
            //SMG
            
            smgIdleSprite = Resources.Load<Sprite>("PlayerWeapons/smg/Sprites/" + IDLE_WEAPON);
            smgShootingSprite = Resources.Load<Sprite>("PlayerWeapons/smg/Sprites/" + SHOOTING_WEAPON);

            smgIdleMat = Resources.Load<Material>("PlayerWeapons/smg/Mats/" + IDLE_WEAPON);
            smgShootingMat = Resources.Load<Material>("PlayerWeapons/smg/Mats/" + SHOOTING_WEAPON);
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

            switch (Values.GetCurrentWeapon())
            {
                case(Values.WeaponEnum.Knife):
                    SetupKnifeAnimations();
                    break;
                case(Values.WeaponEnum.Pistol):
                    SetupPistolAnimations();
                    break;
                case(Values.WeaponEnum.SMG):
                    SetupSmgAnimations();
                    break;
            }
      


        }


        private void SetupKnifeAnimations()
        {
            if (knifeRenderer == null)
                knifeRenderer = Values.GetWeaponObjects()[Values.WeaponEnum.Knife].GetComponent<SpriteRenderer>();
            if (knifeAnimator == null)
            {
                knifeAnimator = Values.GetWeaponObjects()[Values.WeaponEnum.Knife].GetComponent<Animator>();
                knifeAnimation = Resources.Load("PlayerWeapons/knife/Sprites/shooting_AnimController") as RuntimeAnimatorController;

            }
            if (TimerController.GetCurrentTime()[TimerController.KNIFEATTACK_K] > 0 && !attackingWithKnife)
            {
                //knifeRenderer.material = knifeHittingMat;
                knifeRenderer.sprite = knifeHittingSprite;
                knifeAnimator.runtimeAnimatorController = knifeAnimation;                                 
                attackingWithKnife = true;
            }
            else if (attackingWithKnife)

                if (TimerController.GetCurrentTime()[TimerController.KNIFEATTACK_K] == 0)
                    attackingWithKnife = false;
                
           
            if (!attackingWithKnife)
            {
                //knifeRenderer.material = knifeIdleMat;
                knifeRenderer.sprite = knifeIdleSprite;
                knifeAnimator.runtimeAnimatorController = null;
                attackingWithKnife = false;
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
            if ((TimerController.GetCurrentTime()[TimerController.PISTOLATTACK_K] > 0)  && currentBullets >= 0)
            {
                pistolRenderer.material = pistolShootingMat;        //Change mat
                pistolRenderer.sprite = pistolShootingSprite;        //Changes sprite
            }
            else 
            {
                pistolRenderer.material = pistolIdleMat;
                pistolRenderer.sprite = pistolIdleSprite;        //Changes sprite

            }

        }

        private void SetupSmgAnimations()
        {
            if (smgRenderer == null)
                smgRenderer = Values.GetWeaponObjects()[Values.WeaponEnum.SMG].GetComponent<SpriteRenderer>();

            int currentBullets = Values.GetCurrentAmmo()[Values.WeaponEnum.SMG];

            if ((TimerController.GetCurrentTime()[TimerController.SMGATTACK_K] > 0) && currentBullets >= 0)
            {
                smgRenderer.material = smgShootingMat;
                smgRenderer.sprite = smgShootingSprite;
            }
            else
            {
                smgRenderer.material = smgIdleMat;
                smgRenderer.sprite = smgIdleSprite;
            }
        }
    }
}
