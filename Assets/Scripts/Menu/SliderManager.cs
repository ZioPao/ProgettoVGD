using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class SliderManager : MonoBehaviour
    {
        [SerializeField] private Slider effectSlider, musicSlider, sensibilitySlider;




        void Awake()
        {

            float effectSliderValue, musicSliderValue, sensibilityValue;

            ManagePrefs(Values.EffectKey, out effectSliderValue);
            ManagePrefs(Values.MusicKey, out musicSliderValue);
            ManagePrefs(Values.SensibilityKey, out sensibilityValue);

            effectSlider.value = effectSliderValue;
            musicSlider.value = musicSliderValue;
            sensibilitySlider.value = sensibilityValue;

            effectSlider.onValueChanged.AddListener(delegate { EffectSliderCheck(); });
            musicSlider.onValueChanged.AddListener(delegate { MusicSliderCheck(); });
            sensibilitySlider.onValueChanged.AddListener(delegate { SensibilitySliderCheck(); });
        }

        private void EffectSliderCheck()
        {
            Audio.SoundManager.SetEffectVolumeModifier(effectSlider.value);
            PlayerPrefs.SetFloat(Values.EffectKey, effectSlider.value);
            PlayerPrefs.Save();
            
        }

        private void MusicSliderCheck()
        {
            Audio.SoundManager.SetTrackVolumeModifier(musicSlider.value);
            PlayerPrefs.SetFloat(Values.MusicKey, effectSlider.value);
            PlayerPrefs.Save();
        }

        private void SensibilitySliderCheck()
        {
            Values.SetMouseSensibility(sensibilitySlider.value);

            //evitiamo di mettere altri valori in Values, ormai è saturo
            if (Values.GetPlayerTransform())
            {
                Values.GetPlayerTransform().GetComponentInChildren<CameraScript>().UpdateMouseSensibility();
            }
            PlayerPrefs.SetFloat(Values.SensibilityKey, sensibilitySlider.value);
            PlayerPrefs.Save();
        }

        private void ManagePrefs(string key, out float value)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetFloat(key, 0.5f);
            }

            value = PlayerPrefs.GetFloat(key);

        }
    }
}