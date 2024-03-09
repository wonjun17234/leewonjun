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
    public TMP_Dropdown rdropdown;
    public TMP_Dropdown qdropdown;
    public Toggle fullscreenBtn;
    List<Resolution> resolutions = new List<Resolution>();

    [SerializeField]
    List<RenderPipelineAsset> RenderPipelineAssets;

    public int resolutionNum;


    public GameDataObject scriptableObject;

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
        rdropdown.options.Clear();

        Set();
        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "X" + item.height;

            bool isA = false;
            for(int i =0; i < rdropdown.options.Count; i++)
            {
                if (rdropdown.options[i].text.Equals(option.text))
                {
                    isA = true;
                    break;
                }
            }
            if (!isA)
            {
                rdropdown.options.Add(option);
            }

            if (item.width == scriptableObject.resolutionWidth && item.height == scriptableObject.resolutionHeight)
            {
                rdropdown.value = optionNum;
            }
            optionNum++;
        }

        rdropdown.RefreshShownValue();

        fullscreenBtn.isOn = scriptableObject.screenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void Set()
    {
        Screen.SetResolution(scriptableObject.resolutionWidth, scriptableObject.resolutionHeight, scriptableObject.screenMode);
        screenMode = scriptableObject.screenMode;


        qdropdown.value = scriptableObject.qualitySettingsNum;
        QualitySettings.SetQualityLevel(scriptableObject.qualitySettingsNum);
        QualitySettings.renderPipeline = RenderPipelineAssets[scriptableObject.qualitySettingsNum];

    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        scriptableObject.screenMode = screenMode;
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
    public void DropboxOptinoChange(int x)
    {
        resolutionNum = x;
        scriptableObject.resolutionWidth = resolutions[resolutionNum].width; //
        scriptableObject.resolutionHeight = resolutions[resolutionNum].height;
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
        
    }
    public void SelectQuality(int value)
    {
        scriptableObject.qualitySettingsNum = value;
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = RenderPipelineAssets[value];
        //퀄리티 나중에 설정
    }

    public void soundChange(float value)
    {
        //나중에 사운드 설정 https://malbongcode.tistory.com/40
    }

}
