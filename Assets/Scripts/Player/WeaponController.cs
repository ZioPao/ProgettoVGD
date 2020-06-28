﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class WeaponController : MonoBehaviour
    {
        
        //Camera
        private GameObject cameraMain;

        // Start is called before the first frame update
        void Start()
        {
            /*Assign Camera*/
            cameraMain = GameObject.Find("Camera_Main");
            
            /*Setup Weapons*/
            Values.InitializeWeaponObjects();
            Values.InitializeWeaponBehaviours();
            Values.InitializeHeldWeapons();
            Values.InitializeCurrentAmmo();
            Values.InitializeAmmoReserve();
            
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

            Values.AddCurrentAmmo(Values.WeaponEnum.Knife, 0);
            Values.AddCurrentAmmo(Values.WeaponEnum.Pistol, 10);
            Values.AddCurrentAmmo(Values.WeaponEnum.SMG, 25);

            Values.AddAmmoReserve(Values.WeaponEnum.Knife, 0);
            Values.AddAmmoReserve(Values.WeaponEnum.Pistol, 100);
            Values.AddAmmoReserve(Values.WeaponEnum.SMG, 250);

            Values.GetWeaponObjects()[Values.WeaponEnum.Knife].SetActive(false);
            Values.GetWeaponObjects()[Values.WeaponEnum.Pistol].SetActive(true);
            Values.GetWeaponObjects()[Values.WeaponEnum.SMG].SetActive(false);

            Values.SetCurrentWeapon(Values.WeaponEnum.Pistol);
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
        
        public void ShootControl()
        {
            if (Input.GetMouseButtonDown(0) && !Values.GetIsRunning() && !Values.GetIsShooting())
            {
                Values.SetIsShooting(true);

                var weaponTmp = Values.GetWeaponBehaviours()[Values.GetCurrentWeapon()];
                weaponTmp.ShootProjectile();
                if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit projectile, Values.GetProjectileDistance(), LayerMask.GetMask("EnemyHitbox")))
                {
                    Destroy(projectile.transform.parent.gameObject);
                }
                
            }
            else
            {
                Values.SetIsShooting(false); //todo forse da togliere
            }
        }

    }
    
}