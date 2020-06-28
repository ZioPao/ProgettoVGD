using Player;
using UnityEngine;

namespace Player
{
    public class WeaponBehaviour : MonoBehaviour
    {


        [SerializeField] private bool isMelee = false;
        [SerializeField] private int damagePerShot;
        [SerializeField] private float shotSpread;


        //Variabili per info aggiuntive

        private float shootAngle;        //Ampiezza dello sparo. Per shotgun e simili
        
        
        public void ShootProjectile()
        {

            if (Values.GetCurrentAmmo()[Values.GetCurrentWeapon()] > 0)
            {
                Values.DecrementCurrentAmmo(Values.GetCurrentWeapon(), 1);
                
            }
            else
                PlayClackSound();
            
            
        }

        private void PlayClackSound()
        {
            print("clack clack");
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
