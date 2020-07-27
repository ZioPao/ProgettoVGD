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
        private Material knifeIdleMat, knifeHittingMat;
        
        
        void Start()
        {
            anim = GetComponent<Animator>();

            pistolRenderer = null;        //init sempre a null
            smgRenderer = null;
            
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
                case(Values.WeaponEnum.Pistol):
                    SetupPistolAnimations();
                    break;
                case(Values.WeaponEnum.SMG):
                    SetupSmgAnimations();
                    break;
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
