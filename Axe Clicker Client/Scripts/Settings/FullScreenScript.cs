using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using SqlCipher4Unity3D;
using System.Threading.Tasks;

public class FullScreenScript : MonoBehaviour
{
    [SerializeField] Toggle fullscreenToggle;

    // Start is called before the first frame update
    void Start()
    {
        ButtonActions.defaultButtonClicked += ButtonActions_defaultButtonClicked;
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    private void ButtonActions_defaultButtonClicked(SettingsTable table)
    {
        if (table.isFullscreen)
        {
            Screen.SetResolution(ResolutionScript.selectedResolution.width, ResolutionScript.selectedResolution.height, FullScreenMode.ExclusiveFullScreen, ResolutionScript.selectedResolution.refreshRate);
        }
        else
        {
            Screen.SetResolution(ResolutionScript.selectedResolution.width, ResolutionScript.selectedResolution.height, table.isFullscreen, ResolutionScript.selectedResolution.refreshRate);
        }
        fullscreenToggle.isOn = table.isFullscreen;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        if (isFullscreen)
        {
            Screen.SetResolution(ResolutionScript.selectedResolution.width, ResolutionScript.selectedResolution.height, FullScreenMode.ExclusiveFullScreen, ResolutionScript.selectedResolution.refreshRate);
        }
        else
        {
            Screen.SetResolution(ResolutionScript.selectedResolution.width, ResolutionScript.selectedResolution.height, isFullscreen, ResolutionScript.selectedResolution.refreshRate);
        }
#if UNITY_ANDROID
#elif UNITY_IOS
#else
        //only save if we arent mobile
        SaveFullscreen(isFullscreen);

#endif
    }

    void SaveFullscreen(bool fullScreenBool)
    {
        bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
        if (dbExists == true)
        {
            SQLiteConnectionAsync db = new SQLiteConnectionAsync(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
            SettingsTable customSettings = new SettingsTable()
            {
                Id = 2,
                isFullscreen = fullScreenBool,
                screenWidth = ResolutionScript.selectedResolution.width,
                screenHeight = ResolutionScript.selectedResolution.height,
                screenRefreshRate = ResolutionScript.selectedResolution.refreshRate
            };
            db.Update(customSettings);
            db.Close();
        }
    }

    private void OnDestroy()
    {
        ButtonActions.defaultButtonClicked -= ButtonActions_defaultButtonClicked;
    }
}
