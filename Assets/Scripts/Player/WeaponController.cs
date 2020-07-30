using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class WeaponController : MonoBehaviour
    {
        
        //Camera
        

        // Start is called before the first frame update
        void Awake()
        {
            if (!Values.GetIsChangingScene())
            {
                   /*Setup Weapons*/
            
            Values.InitializeWeaponObjects();
            Values.InitializeWeaponBehaviours();
            Values.InitializeHeldWeapons();
            Values.InitializeCurrentAmmo();
            Values.InitializeAmmoReserve();
            Values.InitializeReloadAmount();
            Values.InitializeIsAttacking();
            
            Values.AddWeaponObject(Values.WeaponEnum.Knife, GameObject.Find("PlayerKnife"));
            Values.AddWeaponObject(Values.WeaponEnum.Pistol, GameObject.Find("PlayerPistol"));
            Values.AddWeaponObject(Values.WeaponEnum.SMG, GameObject.Find("PlayerSMG"));
            
            Values.AddWeaponBehaviour(Values.WeaponEnum.Knife, GameObject.Find("PlayerKnife").GetComponent<WeaponBehaviour>());
            Values.AddWeaponBehaviour(Values.WeaponEnum.Pistol, GameObject.Find("PlayerPistol").GetComponent<WeaponBehaviour>());
            Values.AddWeaponBehaviour(Values.WeaponEnum.SMG, GameObject.Find("PlayerSMG").GetComponent<WeaponBehaviour>());
            
            //Only temporary values, in the final game the player won't have all weapons from the start
            Values.AddHeldWeapon(Values.WeaponEnum.Knife, true);
            Values.AddHeldWeapon(Values.WeaponEnum.Pistol, true);    
            Values.AddHeldWeapon(Values.WeaponEnum.SMG, true);

            /*Setup Ammo Values*/
            
            Values.AddCurrentAmmo(Values.WeaponEnum.Knife, 0);
            Values.AddCurrentAmmo(Values.WeaponEnum.Pistol, 10);
            Values.AddCurrentAmmo(Values.WeaponEnum.SMG, 25);

            Values.AddAmmoReserve(Values.WeaponEnum.Knife, 0);
            Values.AddAmmoReserve(Values.WeaponEnum.Pistol, 100);
            Values.AddAmmoReserve(Values.WeaponEnum.SMG, 250);
            
            Values.AddReloadAmount(Values.WeaponEnum.Knife, 0);
            Values.AddReloadAmount(Values.WeaponEnum.Pistol, 10);
            Values.AddReloadAmount(Values.WeaponEnum.SMG, 25);

            /*Setup Attack State*/
            
            Values.AddIsAttacking(Values.WeaponEnum.Knife, false);
            Values.AddIsAttacking(Values.WeaponEnum.Pistol, false);
            Values.AddIsAttacking(Values.WeaponEnum.SMG, false);
            
            /*Sets Pistol as Default Weapon*/
            
            Values.GetWeaponObjects()[Values.WeaponEnum.Knife].SetActive(false);
            Values.GetWeaponObjects()[Values.WeaponEnum.Pistol].SetActive(true);
            Values.GetWeaponObjects()[Values.WeaponEnum.SMG].SetActive(false);

            Values.SetCurrentWeapon(Values.WeaponEnum.Pistol);
            }
         
        }
        
        public void ChangeWeapon()
        {
            //1 Knife
            if (Input.GetKeyDown("1") && Values.GetHeldWeapons()[Values.WeaponEnum.Knife])
            {
                Values.GetWeaponObjects()[Values.WeaponEnum.Knife].SetActive(true);
                Values.GetWeaponObjects()[Values.WeaponEnum.Pistol].SetActive(false);
                Values.GetWeaponObjects()[Values.WeaponEnum.SMG].SetActive(false);

                Values.SetCurrentWeapon(Values.WeaponEnum.Knife);
            }
            //2 Pistol
            if (Input.GetKeyDown("2") && Values.GetHeldWeapons()[Values.WeaponEnum.Pistol])
            {
                Values.GetWeaponObjects()[Values.WeaponEnum.Knife].SetActive(false);
                Values.GetWeaponObjects()[Values.WeaponEnum.Pistol].SetActive(true);
                Values.GetWeaponObjects()[Values.WeaponEnum.SMG].SetActive(false);
                
                Values.SetCurrentWeapon(Values.WeaponEnum.Pistol);
            }
            //3 SMG
            if (Input.GetKeyDown("3") &&  Values.GetHeldWeapons()[Values.WeaponEnum.SMG])
            {
                Values.GetWeaponObjects()[Values.WeaponEnum.Knife].SetActive(false);
                Values.GetWeaponObjects()[Values.WeaponEnum.Pistol].SetActive(false);
                Values.GetWeaponObjects()[Values.WeaponEnum.SMG].SetActive(true);
                
                Values.SetCurrentWeapon(Values.WeaponEnum.SMG);
            }
        }
        
        public void UseWeapon()
        {
            /*Calls Appropriate Weapon Behaviour*/
            
            var weaponTmp = Values.GetWeaponBehaviours()[Values.GetCurrentWeapon()];
                weaponTmp.Action();
        }

    }
    
}