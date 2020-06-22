using Logic.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Various
{
    public class UiScript : MonoBehaviour
    {

        private PlayerController playerStatus;

        private GameObject oxygenCanvas, interactionCanvas, signCanvas;
        private Text healthText, staminaText, oxygenText;

        // Start is called before the first frame update
        void Start()
        {

            playerStatus = GetComponentInParent<PlayerController>();

            oxygenCanvas = GameObject.Find("Oxygen_Canvas");
            interactionCanvas = GameObject.Find("Interaction_Canvas");
            signCanvas = GameObject.Find("Sign_Canvas");

            healthText = GameObject.Find("health_edit").GetComponent<Text>();
            staminaText = GameObject.Find("stamina_edit").GetComponent<Text>();
            oxygenText = GameObject.Find("oxygen_edit").GetComponent<Text>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            healthText.text = Mathf.RoundToInt(playerStatus.GetHealth()).ToString();
            staminaText.text = Mathf.RoundToInt(playerStatus.GetStamina()).ToString();


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
            signCanvas.SetActive(playerStatus.IsPlayerReadingSign());

     

        }


    }
}
