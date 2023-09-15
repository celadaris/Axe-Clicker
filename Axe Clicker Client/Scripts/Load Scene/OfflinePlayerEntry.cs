using SqlCipher4Unity3D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Android;

public class OfflinePlayerEntry : MonoBehaviour
{
    public async void CreateOfflinePlayerEntry(string playerName)
    {
#if UNITY_ANDROID || UNITY_IOS

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
            if (dbExists == true)
            {
                SQLiteAsyncConnection db = new SQLiteAsyncConnection(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
                ScoreTable playerTable = await db.Table<ScoreTable>().Where(x => x.playerName == playerName).FirstOrDefaultAsync();
                if (playerTable == null)
                {
                    ScoreTable newPlayerTable = new ScoreTable()
                    {
                        playerName = playerName,
                        highScore = 0,

                    };
                    await db.InsertAsync(newPlayerTable);
                }
                await db.CloseAsync();
            }
        }

#else

        bool dbExists = File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, "SaveData.db"));
        if (dbExists == true)
        {
            SQLiteAsyncConnection db = new SQLiteAsyncConnection(Application.persistentDataPath + "/" + "SaveData.db", "qfa5Ds");
            ScoreTable playerTable = await db.Table<ScoreTable>().Where(x => x.playerName == playerName).FirstOrDefaultAsync();
            if (playerTable == null)
            {
                ScoreTable newPlayerTable = new ScoreTable()
                {
                    playerName = playerName,
                    highScore = 0,

                };
                await db.InsertAsync(newPlayerTable);
            }
            await db.CloseAsync();
        }

#endif
    }

    //https://stackoverflow.com/a/32932758
    public string RandomString(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(valid[(int)(num % (uint)valid.Length)]);
            }
        }

        return res.ToString();
    }
}
