using Entities.Player.Logic;
using Logic.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Various
{
    public class UiScript : MonoBehaviour
    {

        private PlayerController playerStatus;
        private WeaponScript currentWeapon;

        private GameObject weaponsObject;
        
        private GameObject oxygenCanvas, interactionCanvas, pickupCanvas, signCanvas, ammoCanvas;
        private Text healthText, staminaText, oxygenText, ammoText;

        // Start is called before the first frame update
        void Start()
        {

            playerStatus = GetComponentInParent<PlayerController>();
            weaponsObject = GameObject.Find("PlayerWeapons");
            
            oxygenCanvas = GameObject.Find("Oxygen_Canvas");
            interactionCanvas = GameObject.Find("Interaction_Canvas");
			pickupCanvas = GameObject.Find("Pickup_Canvas");
            signCanvas = GameObject.Find("Sign_Canvas");
            ammoCanvas = GameObject.Find("Ammo_Canvas");        //todo credo sia inutile

            healthText = GameObject.Find("health_edit").GetComponent<Text>();
            staminaText = GameObject.Find("stamina_edit").GetComponent<Text>();
            oxygenText = GameObject.Find("oxygen_edit").GetComponent<Text>();
            ammoText = GameObject.Find("ammo_edit").GetComponent<Text>();

        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            healthText.text = Mathf.RoundToInt(playerStatus.GetHealth()).ToString();
            staminaText.text = Mathf.RoundToInt(playerStatus.GetStamina()).ToString();
            
            //Prende l'arma attiva al momento
            //todo still pesante
            currentWeapon = weaponsObject.GetComponentInChildren<WeaponScript>();

            if (!currentWeapon.GetIsMelee())
            {
                ammoCanvas.SetActive(true);
                ammoText.text = currentWeapon.GetCurrentBulletsInMag() + "/" +
                                currentWeapon.GetCurrentAmmo();

            }
            else
            {
                ammoCanvas.SetActive(false);
            }
             
            float oxygen = playerStatus.GetOxygen();

            if (oxygen < playerStatus.GetMaxOxygen())
            {
                oxygenCanvas.SetActive(true);
                oxygenText.text = Mathf.RoundToInt(oxygen).ToString();
            }
            else
            {
                oxygenCanvas.SetActive(false);
            }
                       
            interactionCanvas.SetActive(playerStatus.IsPlayerNearInteractable());
			pickupCanvas.SetActive(playerStatus.IsPlayerNearPickup());
            signCanvas.SetActive(playerStatus.IsPlayerReadingSign());
        }
    }
}
