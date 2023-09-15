using TMPro;
using System;
using UnityEngine;

public class MyPlayerStats : MonoBehaviour
{
    [SerializeField] GetAllPlayers allPlayers;
    [SerializeField] TMP_Text playerNameTxt;
    [SerializeField] TMP_Text playerScoreTxt;
    [SerializeField] TMP_Text playerRankTxt;


    private void Start()
    {
        allPlayers.ReceivedPlayerScores += AllPlayers_ReceivedPlayerScores;
    }

    private void AllPlayers_ReceivedPlayerScores(object sender, EventArgs e)
    {
        GetAllPlayers getAllPlayers = sender as GetAllPlayers;

        GameObject singletonObj = GameObject.Find("Singleton");
        GlobalVariables globalVariables = singletonObj.GetComponent<GlobalVariables>();

        if (!globalVariables.offlineMode)
        {
            playerNameTxt.text = globalVariables.userName;
        }
        else
        {
            playerNameTxt.text = globalVariables.offlineUserName;
        }

        playerScoreTxt.text = getAllPlayers.allScores.playerScore.ToString();
        playerRankTxt.text = getAllPlayers.allScores.playerRank.ToString();
    }

    private void OnDestroy()
    {
        allPlayers.ReceivedPlayerScores -= AllPlayers_ReceivedPlayerScores;
    }
}
