using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    private PlayerController playerStatus;

    private Text healthText, staminaText, oxygenText;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = GetComponentInParent<PlayerController>();
        healthText = GameObject.Find("health_edit").GetComponent<Text>();
        staminaText = GameObject.Find("stamina_edit").GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthText.text = playerStatus.GetHealth().ToString();
        staminaText.text = playerStatus.GetStamina().ToString();
    }
}
