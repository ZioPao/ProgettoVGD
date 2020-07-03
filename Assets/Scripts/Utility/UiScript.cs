using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class UiScript : MonoBehaviour
    {

        private GameObject oxygenCanvas, interactionCanvas, pickupCanvas, signCanvas, ammoCanvas, signParent;
        private Text healthText, staminaText, oxygenText, ammoText, signText;
        private SignController signScript;

        // Start is called before the first frame update
        void Start()
        {
            oxygenCanvas = GameObject.Find("Oxygen_Canvas");
            interactionCanvas = GameObject.Find("Interaction_Canvas");
			pickupCanvas = GameObject.Find("Pickup_Canvas");
            signCanvas = GameObject.Find("Sign_Canvas");
            ammoCanvas = GameObject.Find("Ammo_Canvas");        //todo credo sia inutile

            healthText = GameObject.Find("health_edit").GetComponent<Text>();
            staminaText = GameObject.Find("stamina_edit").GetComponent<Text>();
            oxygenText = GameObject.Find("oxygen_edit").GetComponent<Text>();
            ammoText = GameObject.Find("ammo_edit").GetComponent<Text>();
            signText = GameObject.Find("sign_text").GetComponent<Text>();
            
            signParent = GameObject.Find("SignParent");
            
            signScript = signParent.GetComponent<SignController>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            healthText.text = Mathf.RoundToInt(Player.Values.GetHealth()).ToString();
            staminaText.text = Mathf.RoundToInt(Player.Values.GetStamina()).ToString();
            
            //Prende l'arma attiva al momento
            //todo still pesante


            if (!Values.GetWeaponBehaviours()[Values.GetCurrentWeapon()].GetIsMelee())
            {
                ammoCanvas.SetActive(true);
                ammoText.text = Values.GetCurrentAmmo()[Values.GetCurrentWeapon()] + "/" +
                                Values.GetAmmoReserve()[Values.GetCurrentWeapon()];

            }
            else
            {
                ammoCanvas.SetActive(false);
            }
             
            float oxygen = Player.Values.GetOxygen();

            if (oxygen < Player.Values.GetMaxOxygen())
            {
                oxygenCanvas.SetActive(true);
                oxygenText.text = Mathf.RoundToInt(oxygen).ToString();
            }
            else
            {
                oxygenCanvas.SetActive(false);
            }
                       
            interactionCanvas.SetActive(Player.Values.GetIsNearInteractable() && !Values.GetIsInteracting());
			pickupCanvas.SetActive(Player.Values.GetIsNearPickup());
            signCanvas.SetActive(Player.Values.GetIsReadingSign());
            
            signText.text = signScript.GetSignText();
            signText.alignment = TextAnchor.MiddleCenter;

        }
    }
}
