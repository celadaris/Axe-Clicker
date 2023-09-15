using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text;
using BestProfanityDetector;
using System;
using System.IO;
using SqlCipher4Unity3D;
using UnityEngine.Android;
using TMPro;

public class AutoSignin : MonoBehaviour
{
    [SerializeField] TMP_Text loadingText;
    [SerializeField] SimpleLoading simpleLoading;
    [SerializeField] OfflinePlayerEntry offlinePlayerEntry;

    string salt = "d3@gh";

    public IEnumerator AutoRegister()
    {
        simpleLoading.enabled = true;

        string userName = offlinePlayerEntry.RandomString(5);
        string password = offlinePlayerEntry.RandomString(10);

        Debug.Log("user: " + userName);
        Debug.Log("pass: " + password);

        ProfanityFilter filter = new ProfanityFilter();

        //check for bad words in username
        if (!filter.HasProfanity(userName))
        {
            //salt password
            string saltedPwrd = password + salt;

            //get hash
            byte[] data = Encoding.ASCII.GetBytes(saltedPwrd);
            data = SHA256.Create().ComputeHash(data);
            string hash = Encoding.ASCII.GetString(data);

            LoginDTO loginTable = new LoginDTO()
            {
                playerName = userName,
                pwrdHash = hash
            };

            string json = JsonConvert.SerializeObject(loginTable);
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);

            UnityWebRequest request;
            using (request = UnityWebRequest.Post("https://localhost:7295/register", json))
            {
                request.disposeUploadHandlerOnDispose = true;
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.SetRequestHeader("Content-Type", "application/json");

                loadingText.text = "Registering...";

                yield return request.SendWebRequest();

                if (request.responseCode == 201)
                {
                    loadingText.text = "Registration Succesful";

                    //save user/pass in local db
                    SaveCredentials(userName, hash, 1, true);

                    //login
                    yield return AutoLogin(userName, hash);
                }
                else
                {
                    simpleLoading.enabled = false;
                    float timeToWait = 5;
                    float incrementToRemove = 1;
                    while (timeToWait > 0)
                    {
                        loadingText.text = "Registration Failed Error: " + request.responseCode + "\nRetrying in " + timeToWait;
                        yield return new WaitForSeconds(incrementToRemove);
                        timeToWait -= incrementToRemove;
                    }

                    yield return AutoRegister();
                }
            }
        }
        else
        {
            yield return AutoRegister();
        }

    }

    public IEnumerator AutoLogin(string userName, string hash)
    {
        simpleLoading.enabled = true;

        LoginDTO loginTable = new LoginDTO()
        {
            playerName = userName,
            pwrdHash = hash
        };

        string json = JsonConvert.SerializeObject(loginTable);
        byte[] jsonToSend = new UTF8Encoding().GetBytes(json);

        UnityWebRequest request;
        using (request = UnityWebRequest.Post("https://localhost:7295/login", json))
        {
            request.disposeUploadHandlerOnDispose = true;
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", "application/json");

            loadingText.text = "Logging in...";

            yield return request.SendWebRequest();

            loadingText.text = "Login Response: " + request.downloadHandler.text;

            //if response is good then get jwt/username and put it on singleton
            if (request.responseCode == 201)
            {
                loadingText.text = "Login Succesful";
                GameObject singletonObj = GameObject.Find("Singleton");
                GlobalVariables globalVariables = singletonObj.GetComponent<GlobalVariables>();
                globalVariables.userName = loginTable.playerName;
                globalVariables.offlineUserName = loginTable.playerName;

                //create offline user in local leaderboard
                offlinePlayerEntry.CreateOfflinePlayerEntry(loginTable.playerName);

                globalVariables.jwt = request.downloadHandler.text;
            }
            else
            {
                simpleLoading.enabled = false;
                float timeToWait = 5;
                float incrementToRemove = 1;
                while (timeToWait > 0)
                {
                    loadingText.text = "Login Failed Error: " + request.responseCode + "\nRetrying in " + timeToWait;
                    yield return new WaitForSeconds(incrementToRemove);
                    timeToWait -= incrementToRemove;
                }

                yield return AutoLogin(userName, hash);
            }

        }
        SceneManagerScript.LoadMainMenu();
    }

    async void SaveCredentials(string playerName, string pswrdHash, int primaryKey, bool autoLogin)
    {
#if UNITY_ANDROID || UNITY_IOS

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
            if (dbExists == true)
            {
                SQLiteAsyncConnection db = new SQLiteAsyncConnection(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
                LoginTable table = await db.Table<LoginTable>().Where(x => x.Id == primaryKey).FirstOrDefaultAsync();

                //if table exists then update the record
                if (table != null)
                {
                    table.Id = primaryKey;
                    table.playerName = playerName;
                    table.pwrdHash = pswrdHash;
                    table.autoLogin = autoLogin;
                    await db.UpdateAsync(table);
                }
                //else create a record
                else
                {
                    table = new LoginTable();
                    table.Id = primaryKey;
                    table.playerName = playerName;
                    table.pwrdHash = pswrdHash;
                    table.autoLogin = autoLogin;
                    await db.InsertAsync(table);
                }
                await db.CloseAsync();
            }
        }


#else
        bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
        if (dbExists == true)
        {
            SQLiteAsyncConnection db = new SQLiteAsyncConnection(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
            LoginTable table = await db.Table<LoginTable>().Where(x => x.Id == primaryKey).FirstOrDefaultAsync();

            //if table exists then update the record
            if (table != null)
            {
                table.Id = primaryKey;
                table.playerName = playerName;
                table.pwrdHash = pswrdHash;
                table.autoLogin = autoLogin;
                await db.UpdateAsync(table);
            }
            //else create a record
            else
            {
                table = new LoginTable();
                table.Id = primaryKey;
                table.playerName = playerName;
                table.pwrdHash = pswrdHash;
                table.autoLogin = autoLogin;
                await db.InsertAsync(table);
            }
            await db.CloseAsync();
        }
#endif
    }
}