using UnityEngine;

namespace Player
{
    public class WeaponBehaviour : MonoBehaviour
    {


        [SerializeField] private bool isMelee = false;
        [SerializeField] private int damagePerShot;
        [SerializeField] private float shotSpread;
        
        private GameObject cameraMain;

        void Start()
        {
            cameraMain = GameObject.Find("Camera_Main");
        }


        public void WeaponAttack()
        {
            switch (gameObject.name)
            {
                case "PlayerKnife":
                    KnifeAttack();
                    break;
                case "PlayerPistol":
                    PistolAttack();
                    break;
                case "PlayerSMG":
                    SMGAttack();
                    break;
                default:
                    break;
            }
        }

        private void KnifeAttack()
        {
            
        }

        private void PistolAttack()
        {
            if (Input.GetMouseButtonDown(0) && !Values.GetIsRunning() && (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.PistolAttack] == 0))
            {
                Values.SetIsShooting(true);
                Utility.TimerController.ResetTimer(Utility.TimerController.TimerEnum.PistolAttack);

                ShootProjectile();
            }
            else
            {
                Values.SetIsShooting(false);
                
            }
            
            Utility.TimerController.RunTimer(Utility.TimerController.TimerEnum.PistolAttack);
            
        }

        private void SMGAttack()
        {
            
        }
        

        
        public void ShootProjectile()
        {
            if (Values.GetCurrentAmmo()[Values.GetCurrentWeapon()] > 0)
            {
                Values.DecrementCurrentAmmo(Values.GetCurrentWeapon(), 1);
                
                if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit projectile, Values.GetProjectileDistance(), LayerMask.GetMask("EnemyHitbox")))
                {
                    Destroy(projectile.transform.parent.gameObject);
                }
            }
        }

        private void Reload()
        {
            print("chick chick clack ricaricata");
        }

        public bool GetIsMelee()
        {
            return isMelee;
        }
        
    }
}
