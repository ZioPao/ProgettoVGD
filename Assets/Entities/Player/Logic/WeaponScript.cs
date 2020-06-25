using UnityEngine;

namespace Logic.Player
{
    public class WeaponScript : MonoBehaviour
    {


        [SerializeField] private bool isMelee = false;
        [SerializeField] private int ammoStart;        //100 proiettili all'inizio del game
        [SerializeField] private int maxBulletsInMag;    // Quanti proiettili nel caricatore
    
    
        //Variabili per info aggiuntive
        private PlayerController playerController;
    
    
    
        //Variabili di stato, da usare per ami da fuoco
        private int ammo;
        private int bulletsInMag;
        private float shootAngle;        //Ampiezza dello sparo. Per shotgun e simili

        
        //Variabili generali, anche per coltello
     
        private int damagePerShot;
    
        void Start()
        {
            playerController = GetComponentInParent<PlayerController>();
            if (isMelee)
            {
                
            }
            else
            {
                       //Nel caso fossero armi da fuoco
                       ammo = ammoStart;
                       bulletsInMag = maxBulletsInMag;
            }
            


        }

        private void FixedUpdate()
        {
            if (!playerController.IsPlayerShooting()) return;
            Shoot();

            //todo finiisci
            // if (playerController.IsPlayerReloading())
            // {
            //     Reload();
            // }

        }


        private void Shoot()
        {

            if (bulletsInMag > 0)
            {
                bulletsInMag--;
                
            }
            else
                PlayClackSound();
            
            
            playerController.SetIsPlayerShooting(false);

        }

        private void PlayClackSound()
        {
            print("clack clack");
        }

        private void Reload()
        {
            print("chick chick clack ricaricata");
        }
        
        
        
        ///////SETTERS & GETTERS

        public int GetCurrentAmmo()
        {
            return ammo;
        }

        public bool GetIsMelee()
        {
            return isMelee;
        }

        public int GetCurrentBulletsInMag()
        {
            return bulletsInMag;
        }
    }
}
