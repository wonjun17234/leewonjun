using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    FullScreenMode screenMode;
    public TMP_Dropdown dropdown;
    public Toggle fullscreenBtn;
    List<Resolution> resolutions = new List<Resolution>();

    public int resolutionNum;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.numerator == 239964)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }
        dropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "X" + item.height + "(" + item.refreshRateRatio + "hz)";
            dropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                dropdown.value = optionNum;
            }
            optionNum++;
        }

        dropdown.RefreshShownValue();

        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }
    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
    public void DropboxOptinoChange(int x)
    {
        resolutionNum = x;
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
    public void SelectQuality(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

}
