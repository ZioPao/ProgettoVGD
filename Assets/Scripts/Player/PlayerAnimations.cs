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
        
        //SMG
        private Sprite smgIdleSprite, smgShootingSprite;
        private Material smgIdleMat, smgShootingMat;
        
        //Knife
        private Sprite knifeIdleSprite, knifeHittingSprite;
        private Material knifeIdleMat, knifeHittingMat;

        //renderer
        private SpriteRenderer pistolRenderer;

        void Start()
        {
            anim = GetComponent<Animator>();

            pistolRenderer = null;        //init sempre a null

            ///PISTOL

            pistolIdleSprite = Resources.Load<Sprite>("PlayerWeapons/pistol/Sprites/" + IDLE_WEAPON);
            pistolShootingSprite = Resources.Load<Sprite>("PlayerWeapons/pistol/Sprites/" + SHOOTING_WEAPON);

            pistolIdleMat = Resources.Load<Material>("PlayerWeapons/pistol/Mats/" + IDLE_WEAPON);
            pistolShootingMat = Resources.Load<Material>("PlayerWeapons/pistol/Mats/" + SHOOTING_WEAPON);
            
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
    
    }
}
