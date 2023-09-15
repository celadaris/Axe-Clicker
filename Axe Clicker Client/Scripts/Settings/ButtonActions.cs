using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SqlCipher4Unity3D;
using UnityEngine.Android;
using System.IO;
using UnityEngine.Audio;
using System;

public class ButtonActions : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public delegate void DefaultButtonClicked(SettingsTable table);
    public static event DefaultButtonClicked defaultButtonClicked;

    public void SaveBtn()
    {
#if UNITY_ANDROID || UNITY_IOS
        
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Save();
        }
        else
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }

#else

        Save();

#endif
    }

    public void DefaultBtn()
    {
#if UNITY_ANDROID || UNITY_IOS
        
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Default();
        }
        else
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }

#else

        Default();

#endif
    }

    void Save()
    {
        bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
        if (dbExists == true)
        {
            float volume = new float();
            audioMixer.GetFloat("volume", out volume);

            SQLiteConnectionAsync db = new SQLiteConnectionAsync(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
            SettingsTable customSettings = new SettingsTable()
            {
                Id = 2,
                isFullscreen = Screen.fullScreen,
                qualityLevel = QualitySettings.GetQualityLevel(),
                volume = volume,
                framerate = Application.targetFrameRate,
                screenWidth = ResolutionScript.selectedResolution.width,
                screenHeight = ResolutionScript.selectedResolution.height,
                screenRefreshRate = ResolutionScript.selectedResolution.refreshRate
            };
            db.Update(customSettings);
            db.Close();
        }
        else
        {
            //TODO create a database... and save...
            Application.Quit();

        }
    }

    void Default()
    {
        bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
        if (dbExists == true)
        {
            SQLiteConnectionAsync db = new SQLiteConnectionAsync(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
            SettingsTable defaultSettings = db.Table<SettingsTable>().Where(x => x.Id == 1).FirstOrDefault();
            defaultButtonClicked?.Invoke(defaultSettings);
            db.Close();

        }
        else
        {
            //TODO create a database... and load default settings...
            Application.Quit();
        }
    }
}
