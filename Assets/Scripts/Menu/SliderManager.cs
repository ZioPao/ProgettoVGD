using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{

    public Slider effectSlider;
    public Slider musicSlider;


    void Awake()
    {
        effectSlider.value = 0.5f;
        musicSlider.value = 0.5f;

        effectSlider.onValueChanged.AddListener(delegate { EffectSliderCheck(); });
        musicSlider.onValueChanged.AddListener(delegate { MusicSliderCheck(); });
    }

    private void EffectSliderCheck()
    {
        Audio.SoundManager.SetEffectVolumeModifier(effectSlider.value);
    }

    private void MusicSliderCheck()
    {
        Audio.SoundManager.SetTrackVolumeModifier(musicSlider.value);
    }
}
