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


        public void Action()
        {
            switch (gameObject.name)
            {
                case "PlayerKnife":
                    KnifeAttack();
                    break;
                case "PlayerPistol":
                    PistolAttack();
                    Reload();
                    break;
                case "PlayerSMG":
                    SMGAttack();
                    Reload();
                    break;
                default:
                    break;
            }
        }

        private void KnifeAttack()
        {
            if (Input.GetMouseButtonDown(0) && !Values.GetIsRunning() && (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.KnifeAttack] == 0))
            {
                Values.SetIsShooting(true);
                Utility.TimerController.ResetTimer(Utility.TimerController.TimerEnum.KnifeAttack);

                MeleeHit();
            }
            else
            {
                Values.SetIsShooting(false);
            }
            Utility.TimerController.RunTimer(Utility.TimerController.TimerEnum.KnifeAttack);
        }

        private void PistolAttack()
        {
            if (Input.GetMouseButtonDown(0) && !Values.GetIsRunning() && !Values.GetIsReloading() && (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.PistolAttack] == 0))
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
            if (Input.GetMouseButton(0) && !Values.GetIsRunning() && !Values.GetIsReloading() && (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.SMGAttack] == 0))
            {
                Values.SetIsShooting(true);
                Utility.TimerController.ResetTimer(Utility.TimerController.TimerEnum.SMGAttack);

                ShootProjectile();
            }
            else
            {
                Values.SetIsShooting(false);
            }
            Utility.TimerController.RunTimer(Utility.TimerController.TimerEnum.SMGAttack);
        }

        
        public void MeleeHit()
        {
            if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit hit, Values.GetMeleeDistance(), LayerMask.GetMask("EnemyHitbox")))
            {
                Destroy(hit.transform.parent.gameObject);
            }
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
            if (Input.GetKeyDown("r") && !Values.GetIsRunning() &&!Values.GetIsShooting() && (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.ReloadTime] == 0))
            {
                Values.SetIsReloading(true);
                Utility.TimerController.ResetTimer(Utility.TimerController.TimerEnum.ReloadTime);

                if (Values.GetAmmoReserve()[Values.GetCurrentWeapon()] >= Values.GetReloadAmount()[Values.GetCurrentWeapon()])
                {
                    Values.DecrementAmmoReserve(Values.GetCurrentWeapon(), (Values.GetReloadAmount()[Values.GetCurrentWeapon()] - Values.GetCurrentAmmo()[Values.GetCurrentWeapon()]));
                    Values.IncrementCurrentAmmo(Values.GetCurrentWeapon(), (Values.GetReloadAmount()[Values.GetCurrentWeapon()] - Values.GetCurrentAmmo()[Values.GetCurrentWeapon()]));
                }
            }
            else
            {
                Values.SetIsReloading(false);
            }
            Utility.TimerController.RunTimer(Utility.TimerController.TimerEnum.ReloadTime);
        }

        public bool GetIsMelee()
        {
            return isMelee;
        }
        
    }
}
