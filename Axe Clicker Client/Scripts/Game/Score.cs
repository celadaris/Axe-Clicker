using Newtonsoft.Json;
using SqlCipher4Unity3D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;

public class Score : MonoBehaviour
{
    static TextMeshProUGUI scoreText;
    GlobalVariables globalVariables;

    private void Start()
    {
        GameObject singletonObj = GameObject.Find("Singleton");
        globalVariables = singletonObj.GetComponent<GlobalVariables>();
        globalVariables.highScore = 0;
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    public async Task AddScore()
    {
        globalVariables.highScore++;
        scoreText.text = globalVariables.highScore.ToString();

        await UpdateOfflineScore();

        if (!globalVariables.offlineMode)
        {
            StartCoroutine(SendHighScore());
        }

    }

    IEnumerator SendHighScore()
    {
        ScoreProperty scoreDTO = new ScoreProperty()
        {
            highScore = globalVariables.highScore
        };

        string json = JsonConvert.SerializeObject(scoreDTO);
        byte[] jsonToSend = new UTF8Encoding().GetBytes(json);

        UnityWebRequest request;
        using (request = UnityWebRequest.Put("https://localhost:7295/updateScore", json))
        {
            //setup jwt
            string formattedResults = globalVariables.jwt.Substring(1, globalVariables.jwt.Length - 2);
            request.SetRequestHeader("Authorization", "Bearer " + formattedResults);
            request.SetRequestHeader("Accept", "application/json");

            //send json data
            request.disposeUploadHandlerOnDispose = true;
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            Debug.Log(request.responseCode);
        }
    }

    async Task UpdateOfflineScore()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
            if (dbExists == true)
            {
                SQLiteAsyncConnection db = new SQLiteAsyncConnection(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");

                int highScore = globalVariables.highScore;
                string playerName = globalVariables.offlineUserName;
                await Task.Run(async () =>
                {
                    ScoreTable playerTable = await db.Table<ScoreTable>().Where(x => x.playerName == playerName).FirstOrDefaultAsync();
                    if (playerTable.highScore < highScore)
                    {
                        playerTable.highScore = highScore;
                        await db.UpdateAsync(playerTable);
                    }
                });
                await db.CloseAsync();
            }
        }
#else
        bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
        if (dbExists == true)
        {
            SQLiteAsyncConnection db = new SQLiteAsyncConnection(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");

            int highScore = globalVariables.highScore;
            string playerName = globalVariables.offlineUserName;
            await Task.Run(async () =>
            {
                ScoreTable playerTable = await db.Table<ScoreTable>().Where(x => x.playerName == playerName).FirstOrDefaultAsync();
                if (playerTable.highScore < highScore)
                {
                    playerTable.highScore = highScore;
                    await db.UpdateAsync(playerTable);
                }
            });
            await db.CloseAsync();
        }
#endif
    }
}
