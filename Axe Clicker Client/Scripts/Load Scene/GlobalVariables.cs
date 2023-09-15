using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public string userName { get; set; }
    public string offlineUserName { get; set; }
    public string jwt { get; set; }
    public int highScore { get; set; }
    public bool offlineMode { get; set; }

    private static GlobalVariables _instance;

    public static GlobalVariables Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GlobalVariables>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
