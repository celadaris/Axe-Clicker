using Newtonsoft.Json;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Android;
using SqlCipher4Unity3D;

public class GetAllPlayers : MonoBehaviour
{
    public LeaderBoardData allScores { get; private set; }
    public event EventHandler ReceivedPlayerScores;
    GlobalVariables globalVariables;

    void Start()
    {
        GameObject singletonObj = GameObject.Find("Singleton");
        globalVariables = singletonObj.GetComponent<GlobalVariables>();

        if (!globalVariables.offlineMode)
        {
            StartCoroutine(RetrievePlayers());
        }
        else
        {
            StartCoroutine(RetrieveOfflinePlayers());
        }

    }

    IEnumerator RetrievePlayers()
    {
        //get jwt from singleton
        string token = globalVariables.jwt;

        UnityWebRequest request;
        using (request = UnityWebRequest.Get("https://localhost:7295/allPlayers"))
        {
            string formattedResults = token.Substring(1, token.Length - 2);
            request.SetRequestHeader("Authorization", "Bearer " + formattedResults);
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            Debug.Log(request.responseCode);

            TextReader reader = new StringReader(request.downloadHandler.text);
            JsonTextReader json = new JsonTextReader(reader);
            JsonSerializer serializer = new JsonSerializer();

            allScores = serializer.Deserialize<LeaderBoardData>(json);

        }
        request.Dispose();
        ReceivedPlayerScores?.Invoke(this, EventArgs.Empty);
    }

    IEnumerator RetrieveOfflinePlayers()
    {
#if UNITY_ANDROID || UNITY_IOS

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
            if (dbExists == true)
            {
                SQLiteConnectionAsync db = new SQLiteConnectionAsync(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
                List<ScoreDTO> playerTable = new List<ScoreDTO>();
                List<ScoreTable> scoreTables = db.Table<ScoreTable>().ToList();

                //order by highScore
                scoreTables = scoreTables.OrderByDescending(x => x.highScore).ToList();

                //find our player name in the offline db
                ScoreTable currentPlayerTable = scoreTables.Find(x => x.playerName == globalVariables.offlineUserName);
                int playerRank = scoreTables.FindIndex(x => x == currentPlayerTable) + 1;

                //for each record store it in playerTable variable
                scoreTables.ForEach(x =>
                {
                    ScoreDTO score = new ScoreDTO()
                    {
                        playerName = x.playerName,
                        highScore = x.highScore
                    };
                    playerTable.Add(score);
                });

                //add all this info to the LeaderBoardData variable
                LeaderBoardData leaderBoardData = new LeaderBoardData()
                {
                    playerRank = playerRank,
                    playerScore = currentPlayerTable.highScore,
                    scoreDTOs = playerTable
                };

                //save LeaderBoardData to public variable for other scripts to use
                allScores = leaderBoardData;

                db.Close();

                yield return new WaitForSeconds(0.1f);
                //tell all other scripts that the public variable is updated
                ReceivedPlayerScores?.Invoke(this, EventArgs.Empty);
            }
        }
#else

        bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
        if (dbExists == true)
        {
            SQLiteConnectionAsync db = new SQLiteConnectionAsync(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
            List<ScoreDTO> playerTable = new List<ScoreDTO>();
            List<ScoreTable> scoreTables = db.Table<ScoreTable>().ToList();

            //order by highScore
            scoreTables = scoreTables.OrderByDescending(x => x.highScore).ToList();

            //find our player name in the offline db
            ScoreTable currentPlayerTable = scoreTables.Find(x => x.playerName == globalVariables.offlineUserName);
            int playerRank = scoreTables.FindIndex(x => x == currentPlayerTable) + 1;

            //for each record store it in playerTable variable
            scoreTables.ForEach(x =>
            {
                ScoreDTO score = new ScoreDTO()
                {
                    playerName = x.playerName,
                    highScore = x.highScore
                };
                playerTable.Add(score);
            });

            //add all this info to the LeaderBoardData variable
            LeaderBoardData leaderBoardData = new LeaderBoardData()
            {
                playerRank = playerRank,
                playerScore = currentPlayerTable.highScore,
                scoreDTOs = playerTable
            };

            //save LeaderBoardData to public variable for other scripts to use
            allScores = leaderBoardData;

            db.Close();

            yield return new WaitForSeconds(0.1f);

            //tell all other scripts that the public variable is updated
            ReceivedPlayerScores?.Invoke(this, EventArgs.Empty);
        }

#endif
    }
}