using SqlCipher4Unity3D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class ResolutionScript : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resDropDown;
    Resolution[] resolutions;
    public static Resolution selectedResolution;

    // Start is called before the first frame update
    void Start()
    {
        ButtonActions.defaultButtonClicked += ButtonActions_defaultButtonClicked;
        //get list of resolutions
        resolutions = Screen.resolutions;
        //add that list to the drop down
        resDropDown.ClearOptions();
        resDropDown.AddOptions(ResolutionStringList());

        //get current resolution
        Resolution currentResolution = new Resolution();
        currentResolution.width = Screen.width;
        currentResolution.height = Screen.height;
        currentResolution.refreshRate = Screen.currentResolution.refreshRate;
        int initialResolutionIndex = FindResolutionIndex(currentResolution);
        resDropDown.value = initialResolutionIndex;
    }

    private void ButtonActions_defaultButtonClicked(SettingsTable table)
    {
        Resolution defaultResolution = new Resolution();
        defaultResolution.width = table.screenWidth;
        defaultResolution.height = table.screenHeight;
        defaultResolution.refreshRate = table.screenRefreshRate;
        if (table.isFullscreen)
        {
            Screen.SetResolution(defaultResolution.width, defaultResolution.height, FullScreenMode.ExclusiveFullScreen, defaultResolution.refreshRate);
        }
        else
        {
            Screen.SetResolution(defaultResolution.width, defaultResolution.height, table.isFullscreen, defaultResolution.refreshRate);
        }
        resDropDown.value = FindResolutionIndex(defaultResolution);
    }

    List<string> ResolutionStringList()
    {
        List<string> resolutionsList = new List<string>();
        string resolutionListing;

        foreach (Resolution r in resolutions)
        {
            if (r.refreshRate < 30)
            {
                resolutionListing = r.width.ToString() + " x " + r.height.ToString();
            }
            else
            {
                resolutionListing = r.width.ToString() + " x " + r.height.ToString() + " @ " + r.refreshRate.ToString() + "hz";
            }
            resolutionsList.Add(resolutionListing);
        }
        return resolutionsList;
    }

    public int FindResolutionIndex(Resolution resolutionToFind)
    {
        int resolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == resolutionToFind.width && resolutions[i].height == resolutionToFind.height && resolutions[i].refreshRate == resolutionToFind.refreshRate)
            {
                resolutionIndex = i;
            }
        }
        return resolutionIndex;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution reConstructedResolution = new Resolution();
        reConstructedResolution.width = resolutions[resolutionIndex].width;
        reConstructedResolution.height = resolutions[resolutionIndex].height;
        reConstructedResolution.refreshRate = resolutions[resolutionIndex].refreshRate;

        if (Screen.fullScreen)
        {
            Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, FullScreenMode.ExclusiveFullScreen, resolutions[resolutionIndex].refreshRate);
        }
        else
        {
            Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen, resolutions[resolutionIndex].refreshRate);
        }
        resDropDown.value = resolutionIndex;
        selectedResolution = reConstructedResolution;
    }

    private void OnDestroy()
    {
        ButtonActions.defaultButtonClicked -= ButtonActions_defaultButtonClicked;
    }
}
