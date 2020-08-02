 using UnityEngine;
 using System.Collections;
 using TMPro;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;

 namespace Menu
 {
     public class ButtonController : MonoBehaviour, ISelectHandler, IDeselectHandler
     {

         private TextMeshProUGUI buttonText;
         
         void Start()
         {
             buttonText = GetComponentInChildren<TextMeshProUGUI>();
         }

         public void OnSelect(BaseEventData eventData)
         {
             buttonText.color = Color.white;
             Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.MenuBlip);
         }
         
         public void OnDeselect(BaseEventData eventData)
         {
             buttonText.color = Color.gray;
         }
     }
 }
