using SqlCipher4Unity3D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;
    float volume;


    // Start is called before the first frame update
    void Start()
    {
        ButtonActions.defaultButtonClicked += ButtonActions_defaultButtonClicked;
        
        //read from database
        volumeSlider.value = GetVolume();
        SetVolume(volumeSlider.value);
    }

    private void ButtonActions_defaultButtonClicked(SettingsTable table)
    {
        audioMixer.SetFloat("volume", table.volume);
        volumeSlider.value = table.volume;
    }

    private float GetVolume()
    {
#if UNITY_ANDROID || UNITY_IOS

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
            if (dbExists == true)
            {
                float volume = new float();
                audioMixer.GetFloat("volume", out volume);

                SQLiteConnectionAsync db = new SQLiteConnectionAsync(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
                SettingsTable customSettings = db.Table<SettingsTable>().Where(x => x.Id == 2).FirstOrDefault();
                db.Close();
                return customSettings.volume;
            }
            else
            {
                return 0;

            }
        }
        else
        {
            return 0;
        }

#else
    
        bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
            if (dbExists == true)
            {
                float volume = new float();
                audioMixer.GetFloat("volume", out volume);

                SQLiteConnectionAsync db = new SQLiteConnectionAsync(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
                SettingsTable customSettings = db.Table<SettingsTable>().Where(x => x.Id == 2).FirstOrDefault();
                db.Close();
                return customSettings.volume;
            }
            else
            {
                return 0;

            }
#endif
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    private void OnDestroy()
    {
        ButtonActions.defaultButtonClicked -= ButtonActions_defaultButtonClicked;
    }
}
