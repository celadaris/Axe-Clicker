using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopPlayers : MonoBehaviour
{
    [SerializeField] GameObject contentGameObj;
    [SerializeField] GetAllPlayers allPlayers;
    [SerializeField] GameObject playerScoreUI;
    [SerializeField] Scrollbar scrollbar;

    private void Start()
    {
        allPlayers.ReceivedPlayerScores += AllPlayers_ReceivedPlayerScores;
    }

    private void AllPlayers_ReceivedPlayerScores(object sender, System.EventArgs e)
    {
        GetAllPlayers getAllPlayers = sender as GetAllPlayers;

        for (int i = 0; i < getAllPlayers.allScores.scoreDTOs.Count; i++)
        {
            GameObject currentPlayer = Instantiate(playerScoreUI);
            currentPlayer.transform.SetParent(contentGameObj.transform, false);

            GameObject rank = currentPlayer.transform.Find("RankTxt").gameObject;
            rank.GetComponent<TMP_Text>().text = (i + 1).ToString();

            GameObject name = currentPlayer.transform.Find("NameTxt").gameObject;
            name.GetComponent<TMP_Text>().text = getAllPlayers.allScores.scoreDTOs[i].playerName.ToString();

            GameObject score = currentPlayer.transform.Find("ScoreTxt").gameObject;
            score.GetComponent<TMP_Text>().text = getAllPlayers.allScores.scoreDTOs[i].highScore.ToString();
        }

        StartCoroutine(MoveScrollBar());
    }

    IEnumerator MoveScrollBar()
    {
        yield return null;
        scrollbar.value = 1;
    }

    private void OnDestroy()
    {
        allPlayers.ReceivedPlayerScores -= AllPlayers_ReceivedPlayerScores;
    }
}
