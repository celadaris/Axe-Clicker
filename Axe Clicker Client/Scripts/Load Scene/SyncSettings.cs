using UnityEngine;
using SqlCipher4Unity3D;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.Android;

public class SyncSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AutoSignin autoSignin;
    float volume;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            StartupAccess();
        }
        else
        {
            if (Screen.currentResolution.refreshRate > 30)
            {
                Application.targetFrameRate = Screen.currentResolution.refreshRate;
            }
            else
            {
                Application.targetFrameRate = 60;
            }
        }


#elif UNITY_STANDALONE || UNITY_EDITOR

        StartupAccess();

#endif
    }

    async void StartupAccess()
    {
#if UNITY_ANDROID || UNITY_IOS

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
            Debug.Log("it is " + dbExists + " that the file exists");
            volume = new float();

            if (dbExists)
            {
                SQLiteAsyncConnection db = new SQLiteAsyncConnection(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
                LoadTableSettings(2);

                LoginTable guestLogin = await db.Table<LoginTable>().Where(x => x.Id == 1).FirstOrDefaultAsync();
                LoginTable memberLogin = await db.Table<LoginTable>().Where(x => x.Id == 2).FirstOrDefaultAsync();
                await db.CloseAsync();

                //login to whatever has auto login bool set to true
                if (guestLogin == null)
                {
                    StartCoroutine(autoSignin.AutoRegister());
                }
                else
                {
                    if (guestLogin.autoLogin == true)
                    {
                        StartCoroutine(autoSignin.AutoLogin(guestLogin.playerName, guestLogin.pwrdHash));
                    }
                }

                if (memberLogin != null)
                {
                    if (memberLogin.autoLogin == true)
                    {

                        StartCoroutine(autoSignin.AutoLogin(memberLogin.playerName, memberLogin.pwrdHash));
                    }
                }


            }
            else
            {
                CreateDefaults();
                LoadTableSettings(2);

                //create credentials and login
                StartCoroutine(autoSignin.AutoRegister());
            }
        }

#elif UNITY_STANDALONE || UNITY_EDITOR
        bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
        Debug.Log("it is " + dbExists + " that the file exists");
        volume = new float();

        if (dbExists)
        {
            SQLiteAsyncConnection db = new SQLiteAsyncConnection(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
            LoadTableSettings(2);

            LoginTable guestLogin = await db.Table<LoginTable>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            LoginTable memberLogin = await db.Table<LoginTable>().Where(x => x.Id == 2).FirstOrDefaultAsync();
            await db.CloseAsync();

            //login to whatever has auto login bool set to true
            if (guestLogin == null)
            {
                StartCoroutine(autoSignin.AutoRegister());
            }
            else
            {
                if (guestLogin.autoLogin == true)
                {
                    StartCoroutine(autoSignin.AutoLogin(guestLogin.playerName, guestLogin.pwrdHash));
                }
            }

            if (memberLogin != null)
            {
                if (memberLogin.autoLogin == true)
                {

                    StartCoroutine(autoSignin.AutoLogin(memberLogin.playerName, memberLogin.pwrdHash));
                }
            }


        }
        else
        {
            CreateDefaults();
            LoadTableSettings(2);

            //create credentials and login
            StartCoroutine(autoSignin.AutoRegister());
        }

#endif


    }

    void CreateDefaults()
    {
        SQLiteConnectionAsync db = new SQLiteConnectionAsync(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");

        //create a active and default settings table
        db.CreateTable<SettingsTable>();
        SettingsTable defaults = new SettingsTable() { Id = 1 };
        SaveDefaults(defaults);
        SettingsTable customSettings = new SettingsTable() { Id = 2 };
        SaveDefaults(customSettings);

        //create a local login/leaderboard table in db
        db.CreateTable<LoginTable>();
        db.CreateTable<ScoreTable>();
        db.Close();

    }

    async void SaveDefaults(SettingsTable defaults)
    {
        bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));

        if (dbExists)
        {
            SQLiteAsyncConnection db = new SQLiteAsyncConnection(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");

            defaults.isFullscreen = Screen.fullScreen;
            defaults.qualityLevel = QualitySettings.GetQualityLevel();
            audioMixer.GetFloat("volume", out volume);
            defaults.volume = volume;
            defaults.screenWidth = Screen.currentResolution.width;
            defaults.screenHeight = Screen.currentResolution.height;
            defaults.screenRefreshRate = Screen.currentResolution.refreshRate;

            if (Screen.currentResolution.refreshRate < 30)
            {
                defaults.framerate = 120;
            }
            else
            {
                defaults.framerate = Screen.currentResolution.refreshRate;
            }

            await db.InsertAsync(defaults);
            await db.CloseAsync();
        }
    }

    void LoadTableSettings(int primaryKey)
    {
        bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));

        if (dbExists)
        {
            SQLiteConnectionAsync db = new SQLiteConnectionAsync(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
            SettingsTable loadedTable = db.Table<SettingsTable>().Where(x => x.Id == primaryKey).FirstOrDefault();
            db.Close();

            Application.targetFrameRate = loadedTable.framerate;
            QualitySettings.SetQualityLevel(loadedTable.qualityLevel);
            audioMixer.SetFloat("volume", loadedTable.volume);
        }

    }
}
