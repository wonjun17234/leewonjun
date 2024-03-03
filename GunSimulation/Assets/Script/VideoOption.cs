using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class VideoOption : MonoBehaviour
{
    FullScreenMode screenMode;
    public TMP_Dropdown dropdown;
    public Toggle fullscreenBtn;
    List<Resolution> resolutions = new List<Resolution>();

    [SerializeField]
    List<RenderPipelineAsset> RenderPipelineAssets;

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
            //if (Screen.resolutions[i].refreshRateRatio.numerator == 239964)
            //{
                resolutions.Add(Screen.resolutions[i]);
            //}
        }
        dropdown.options.Clear();


        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "X" + item.height;

            bool isA = false;
            for(int i =0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text.Equals(option.text))
                {
                    isA = true;
                    break;
                }
            }
            if (!isA)
            {
                dropdown.options.Add(option);
            }

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
    public void SelectQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = RenderPipelineAssets[value];
        //퀄리티 나중에 설정
    }

    public void soundChange(float value)
    {
        //나중에 사운드 설정 https://malbongcode.tistory.com/40
    }

}
