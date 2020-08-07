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
            effectSlider.value = 0.5f;
            musicSlider.value = 0.5f;
            sensibilitySlider.value = 0.5f;

            effectSlider.onValueChanged.AddListener(delegate { EffectSliderCheck(); });
            musicSlider.onValueChanged.AddListener(delegate { MusicSliderCheck(); });
            sensibilitySlider.onValueChanged.AddListener(delegate { SensibilitySliderCheck(); });

        }

        private void EffectSliderCheck()
        {
            Audio.SoundManager.SetEffectVolumeModifier(effectSlider.value);
        }

        private void MusicSliderCheck()
        {
            Audio.SoundManager.SetTrackVolumeModifier(musicSlider.value);
        }

        private void SensibilitySliderCheck()
        {
            Values.SetMouseSensibility(sensibilitySlider.value);
            
            //evitiamo di mettere altri valori in Values, ormai è saturo
            if (Values.GetPlayerTransform())
            {
                Values.GetPlayerTransform().GetComponentInChildren<CameraScript>().UpdateMouseSensibility();

            }
        }
    }
}
