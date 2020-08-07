using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{

    public class HealthBarManager : MonoBehaviour
    {

        public void ChangeHealthBarColor(float currentHealth, float maxHealth)
        {

            float percentHealth;
            percentHealth = (currentHealth / maxHealth) * 0.36f;

            //grazie unity
            gameObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(percentHealth, 0.83f, 1);
        }

        public void ChangeHealthBarWidth (float currentHealth, float maxHealth)
        {
            float percentHealth;
            Vector3 scaleChange;

            percentHealth = (currentHealth / maxHealth);
            scaleChange = new Vector3(percentHealth, 1, 1);

            gameObject.transform.localScale = scaleChange;
        }

    }

}