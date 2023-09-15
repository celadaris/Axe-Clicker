using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneManagerScript.LoadGameOver();
    }
}
