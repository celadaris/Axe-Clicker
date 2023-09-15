using SqlCipher4Unity3D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;

public class RunOffline : MonoBehaviour
{
    [SerializeField] OfflinePlayerEntry offlinePlayerEntry;

    public async void GoOffline()
    {
        GameObject singletonObj = GameObject.Find("Singleton");
        GlobalVariables globalVariables = singletonObj.GetComponent<GlobalVariables>();
        globalVariables.offlineMode = true;

#if UNITY_ANDROID || UNITY_IOS

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
            if (dbExists == true)
            {
                SQLiteAsyncConnection db = new SQLiteAsyncConnection(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
                LoginTable guestLogin = await db.Table<LoginTable>().Where(x => x.Id == 1).FirstOrDefaultAsync();
                LoginTable memberLogin = await db.Table<LoginTable>().Where(x => x.Id == 2).FirstOrDefaultAsync();
                await db.CloseAsync();

                if (memberLogin != null)
                {
                    if (memberLogin.autoLogin == true && !string.IsNullOrEmpty(memberLogin.playerName))
                    {
                        globalVariables.offlineUserName = memberLogin.playerName;
                    }
                    else if (guestLogin != null && !string.IsNullOrEmpty(guestLogin.playerName))
                    {
                        globalVariables.offlineUserName = guestLogin.playerName;
                    }
                    else
                    {
                        globalVariables.offlineUserName = offlinePlayerEntry.RandomString(5);
                        offlinePlayerEntry.CreateOfflinePlayerEntry(globalVariables.offlineUserName);
                    }
                }
                else if (guestLogin != null && !string.IsNullOrEmpty(guestLogin.playerName))
                {
                    globalVariables.offlineUserName = guestLogin.playerName;
                }
                else
                {
                    globalVariables.offlineUserName = offlinePlayerEntry.RandomString(5);
                    offlinePlayerEntry.CreateOfflinePlayerEntry(globalVariables.offlineUserName);
                }
            }
        }
#else


        bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
        if (dbExists == true)
        {
            SQLiteAsyncConnection db = new SQLiteAsyncConnection(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
            LoginTable guestLogin = await db.Table<LoginTable>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            LoginTable memberLogin = await db.Table<LoginTable>().Where(x => x.Id == 2).FirstOrDefaultAsync();
            await db.CloseAsync();

            if (memberLogin != null)
            {
                if (memberLogin.autoLogin == true && !string.IsNullOrEmpty(memberLogin.playerName))
                {
                    globalVariables.offlineUserName = memberLogin.playerName;
                }
                else if (guestLogin != null && !string.IsNullOrEmpty(guestLogin.playerName))
                {
                    globalVariables.offlineUserName = guestLogin.playerName;
                }
                else
                {
                    globalVariables.offlineUserName = offlinePlayerEntry.RandomString(5);
                    offlinePlayerEntry.CreateOfflinePlayerEntry(globalVariables.offlineUserName);
                }
            }
            else if (guestLogin != null && !string.IsNullOrEmpty(guestLogin.playerName))
            {
                globalVariables.offlineUserName = guestLogin.playerName;
            }
            else
            {
                globalVariables.offlineUserName = offlinePlayerEntry.RandomString(5);
                offlinePlayerEntry.CreateOfflinePlayerEntry(globalVariables.offlineUserName);
            }
        }
#endif
        SceneManagerScript.LoadMainMenu();
    }
}
