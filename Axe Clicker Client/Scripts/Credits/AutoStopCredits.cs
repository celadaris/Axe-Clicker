using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStopCredits : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManagerScript.LoadMainMenu();
    }
}
