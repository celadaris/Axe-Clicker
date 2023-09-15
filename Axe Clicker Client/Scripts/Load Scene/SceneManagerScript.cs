using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public static void LoadGame()
    {
        SceneManager.LoadScene(2);
    }

    public static void LoadGameOver()
    {
        SceneManager.LoadScene(3);
    }

    public static void LoadOptions()
    {
        SceneManager.LoadScene(4);
    }

    public static void LoadLeaderBoard()
    {
        SceneManager.LoadScene(5);
    }

    public static void LoadCredits()
    {
        SceneManager.LoadScene(6);
    }

    public static void LoadLogin()
    {
        SceneManager.LoadScene(7);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
