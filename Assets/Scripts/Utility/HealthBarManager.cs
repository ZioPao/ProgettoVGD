using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{

    public class HealthBarManager : MonoBehaviour
    {

        public void changeHealthBarColor(int currentHealth, int maxHealth)
        {
            float H, S, V;
            float percentHealth;

            Color currentColor = gameObject.GetComponent<SpriteRenderer>().color;
            Color.RGBToHSV(currentColor, out H, out S, out V);

            percentHealth = (currentHealth / maxHealth) * 100;

            gameObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(percentHealth, S, V);
        }

        public void changeHealthBarWidthint (int currentHealth, int maxHealth)
        {
            float percentHealth;
            Vector3 scaleChange;

            percentHealth = (currentHealth / maxHealth);
            scaleChange = new Vector3(percentHealth, 1, 1);

            gameObject.transform.localScale = scaleChange;
        }

    }

}