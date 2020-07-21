using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class UiScript : MonoBehaviour
    {

        private GameObject oxygenCanvas, interactionCanvas, pickupCanvas, signCanvas, ammoCanvas, signParent;
        private Text healthString, staminaText, oxygenText, ammoText, signText;
        private Image healthSprite;
        private SignController signScript;

        // Start is called before the first frame update
        void Start()
        {
            oxygenCanvas = GameObject.Find("Oxygen_Canvas");
            interactionCanvas = GameObject.Find("Interaction_Canvas");
			pickupCanvas = GameObject.Find("Pickup_Canvas");
            signCanvas = GameObject.Find("Sign_Canvas");
            ammoCanvas = GameObject.Find("Ammo_Canvas");        //todo credo sia inutile

            
            //health
            healthString = GameObject.Find("health_text").GetComponent<Text>();
            healthSprite = GameObject.Find("health_sprite").GetComponent<Image>();
            
            staminaText = GameObject.Find("stamina_edit").GetComponent<Text>();
            oxygenText = GameObject.Find("oxygen_edit").GetComponent<Text>();
            ammoText = GameObject.Find("ammo_edit").GetComponent<Text>();
            signText = GameObject.Find("sign_text").GetComponent<Text>();
            
            signParent = GameObject.Find("InteractableObjects");
            
            signScript = signParent.GetComponent<SignController>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {

            int currentHealth = Mathf.RoundToInt(Player.Values.GetHealth());
            SetHealthSprite(currentHealth);
            
            healthString.text = currentHealth.ToString();
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
             
            float oxygen = Values.GetOxygen();

            if (oxygen < Values.GetMaxOxygen())
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

        private void SetHealthSprite(int health)
        {
            healthSprite.preserveAspect = true;
            
            switch (health)
            {
                case int n when (n >= 100):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 100");
                    break;
                case int n when (n < 90 && n >= 80):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 090");
                    break;
                case int n when (n < 80 && n >= 70):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 080");
                    break;
                case int n when (n < 70 && n >= 60):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 070");
                    break;
                case int n when (n < 60 && n >= 50):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 060");
                    break;
                case int n when (n < 50 && n >= 40):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 050");
                    break;
                case int n when (n < 40 && n >= 30):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 040");
                    break;
                case int n when (n < 30 && n >= 20):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 030");
                    break;
                case int n when (n < 20 && n >= 10):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 020");
                    break;
                case int n when (n < 10 && n >= 0):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 010");
                    break;
                default:
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 000");
                    break;
            }
        }
    }
}
