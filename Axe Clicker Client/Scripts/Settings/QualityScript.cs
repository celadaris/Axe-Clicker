using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QualityScript : MonoBehaviour
{
    [SerializeField] TMP_Dropdown qualityDropDown;

    // Start is called before the first frame update
    void Start()
    {
        ButtonActions.defaultButtonClicked += ButtonActions_defaultButtonClicked;
        qualityDropDown.value = QualitySettings.GetQualityLevel();
    }

    private void ButtonActions_defaultButtonClicked(SettingsTable table)
    {
        QualitySettings.SetQualityLevel(table.qualityLevel);
        qualityDropDown.value = table.qualityLevel;
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    private void OnDestroy()
    {
        ButtonActions.defaultButtonClicked -= ButtonActions_defaultButtonClicked;
    }
}
