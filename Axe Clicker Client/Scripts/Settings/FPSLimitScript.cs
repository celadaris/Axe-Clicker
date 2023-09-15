using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSLimitScript : MonoBehaviour
{
    [SerializeField] TMP_Dropdown fpsCapDropDown;
    List<int> fpsLimitList;

    // Start is called before the first frame update
    void Start()
    {
        ButtonActions.defaultButtonClicked += ButtonActions_defaultButtonClicked;
        fpsCapDropDown.ClearOptions();
        fpsCapDropDown.AddOptions(FpsCapString());
        fpsCapDropDown.value = GetFpsCapIndex(Application.targetFrameRate);
    }

    private void ButtonActions_defaultButtonClicked(SettingsTable table)
    {
        Application.targetFrameRate = table.framerate;
        fpsCapDropDown.value = GetFpsCapIndex(table.framerate);
    }

    List<string> FpsCapString()
    {
        List<int> fpsList = new List<int>();
        fpsList.Add(30);
        fpsList.Add(60);
        fpsList.Add(120);
        int customCap = Screen.currentResolution.refreshRate;

        bool isIncluded = fpsList.Contains(customCap);

        if (!isIncluded)
        {
            fpsList.Add(customCap);
            fpsList.Sort();
        }

        List<string> fpsListStrings = new List<string>();

        fpsList.ForEach(x => { fpsListStrings.Add(x.ToString()); });
        fpsListStrings.Add("Unlimited");

        fpsList.Add(-1);
        fpsLimitList = fpsList;

        return fpsListStrings;
    }

    public void SetFpsCap(int index)
    {
        Application.targetFrameRate = fpsLimitList[index];
    }

    int GetFpsCapIndex(int fps)
    {
        int index = 0;

        for (int i = 0; i < fpsLimitList.Count; i++)
        {
            if (fps == fpsLimitList[i])
            {
                index = i;
            }
        }
        return index;
    }

    private void OnDestroy()
    {
        ButtonActions.defaultButtonClicked -= ButtonActions_defaultButtonClicked;
    }
}
