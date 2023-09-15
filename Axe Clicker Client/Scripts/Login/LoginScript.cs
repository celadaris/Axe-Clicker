using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.IO;
using SqlCipher4Unity3D;
using System.Linq;
using UnityEngine.Android;

public class LoginScript : MonoBehaviour
{
    [SerializeField] TMP_InputField userName;
    [SerializeField] TMP_InputField password;
    [SerializeField] Toggle autoLoginToggle;
    [SerializeField] TMP_Text responseTxt;
    [SerializeField] OfflinePlayerEntry offlinePlayerEntry;
    string salt = "d3@gh";

    public void Login()
    {
        StartCoroutine(LoginName());
    }

    IEnumerator LoginName()
    {
        //salt password
        string saltedPwrd = password.text + salt;

        //get hash
        byte[] data = Encoding.ASCII.GetBytes(saltedPwrd);
        data = new SHA256Managed().ComputeHash(data);
        string hash = Encoding.ASCII.GetString(data);

        LoginDTO loginTable = new LoginDTO()
        {
            playerName = userName.text,
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

            responseTxt.text = "Logging in...";

            yield return request.SendWebRequest();

            //if response is good then get jwt/username and put it on singleton
            if (request.responseCode == 201)
            {

                GameObject singletonObj = GameObject.Find("Singleton");
                GlobalVariables globalVariables = singletonObj.GetComponent<GlobalVariables>();
                globalVariables.jwt = request.downloadHandler.text.ToString();

                //retreive jwt
                string token = globalVariables.jwt.Replace("Bearer ", string.Empty).Replace("\"", string.Empty);

                ReadJWTPacket packetReader = new ReadJWTPacket();
                JWTPacketDetails packet = packetReader.JWTConverter(token);

                //read the token and use the correct case-sensitive playername
                string playerName = packet.aud;


                //retrieve player name from the jwt
                globalVariables.userName = playerName;

                globalVariables.offlineUserName = playerName;
                globalVariables.offlineMode = false;

#if UNITY_ANDROID || UNITY_IOS

                if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
                {
                    bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
                    if (dbExists == true)
                    {
                        SQLiteConnectionAsync db = new SQLiteConnectionAsync(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
                        LoginTable memberTable = db.Table<LoginTable>().Where(x => x.Id == 2).FirstOrDefault();
                        LoginTable guestTable = db.Table<LoginTable>().Where(x => x.Id == 1).FirstOrDefault();

                        //create a offline leaderboard recrd if there is non
                        offlinePlayerEntry.CreateOfflinePlayerEntry(playerName);


                        if (autoLoginToggle.isOn)
                        {
                            if (memberTable != null)
                            {
                                memberTable.autoLogin = true;
                                memberTable.playerName = playerName;
                                memberTable.pwrdHash = loginTable.pwrdHash;
                                db.Update(memberTable);

                                if (guestTable != null)
                                {
                                    guestTable.autoLogin = false;
                                    db.Update(guestTable);
                                }
                            }
                            else
                            {
                                memberTable = new LoginTable();
                                memberTable.autoLogin = true;
                                memberTable.playerName = playerName;
                                memberTable.pwrdHash = loginTable.pwrdHash;
                                memberTable.Id = 2;
                                db.Insert(memberTable);

                                if (guestTable != null)
                                {
                                    guestTable.autoLogin = false;
                                    db.Update(guestTable);
                                }
                            }
                        }
                        else
                        {
                            if (memberTable != null)
                            {
                                memberTable.autoLogin = false;
                                db.Update(memberTable);

                                if (guestTable != null)
                                {
                                    guestTable.autoLogin = true;
                                    db.Update(guestTable);
                                }
                            }
                        }
                    }
                }
#else
                bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
                if (dbExists == true)
                {
                    SQLiteConnectionAsync db = new SQLiteConnectionAsync(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
                    LoginTable memberTable = db.Table<LoginTable>().Where(x => x.Id == 2).FirstOrDefault();
                    LoginTable guestTable = db.Table<LoginTable>().Where(x => x.Id == 1).FirstOrDefault();

                    //create a offline leaderboard record if there is non
                    offlinePlayerEntry.CreateOfflinePlayerEntry(playerName);


                    if (autoLoginToggle.isOn)
                    {
                        if (memberTable != null)
                        {
                            memberTable.autoLogin = true;
                            memberTable.playerName = playerName;

                            memberTable.pwrdHash = loginTable.pwrdHash;
                            db.Update(memberTable);

                            if (guestTable != null)
                            {
                                guestTable.autoLogin = false;
                                db.Update(guestTable);
                            }
                        }
                        else
                        {
                            memberTable = new LoginTable();
                            memberTable.autoLogin = true;
                            memberTable.playerName = playerName;
                            memberTable.pwrdHash = loginTable.pwrdHash;
                            memberTable.Id = 2;
                            db.Insert(memberTable);

                            if (guestTable != null)
                            {
                                guestTable.autoLogin = false;
                                db.Update(guestTable);
                            }
                        }
                    }
                    else
                    {
                        if (memberTable != null)
                        {
                            memberTable.autoLogin = false;
                            db.Update(memberTable);

                            if (guestTable != null)
                            {
                                guestTable.autoLogin = true;
                                db.Update(guestTable);
                            }
                        }
                    }
                }
#endif
                responseTxt.text = "Logged in";
            }
            else
            {
                responseTxt.text = "Login failed: " + request.downloadHandler.text;
            }

        }
    }
}
