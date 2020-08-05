using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript
{

    private Resolution[] resolutions;

    public void Init()
    {
        resolutions = Screen.resolutions;
        
    }
    public void SetResolutionOptions(Dropdown resolutionDropdown)
    {
        //Settings stuff
        
        resolutionDropdown.ClearOptions();

        List<string> possibleResolutions = new List<string>();
        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            possibleResolutions.Add(resolutions[i].width + "x" + resolutions[i].height + " (" + resolutions[i].refreshRate + "Hz)" );
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }

        }

        resolutionDropdown.AddOptions(possibleResolutions);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetChosenResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
