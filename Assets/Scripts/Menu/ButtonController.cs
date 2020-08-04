 using UnityEngine;
 using System.Collections;
 using TMPro;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;

 namespace Menu
 {
     public class ButtonController : MonoBehaviour, ISelectHandler, IDeselectHandler
     {

         private TextMeshProUGUI text;
         
         void Start()
         {
             text = GetComponentInChildren<TextMeshProUGUI>();
         }

         public void OnSelect(BaseEventData eventData)
         {

             
                 text.color = Color.white;
                 Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.MenuBlip);
             
              
         }
         
         public void OnDeselect(BaseEventData eventData)
         {
             text.color = Color.gray;
         }
     }
 }
