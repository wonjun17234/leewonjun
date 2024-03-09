using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game Settings")]

public class GameDataObject : ScriptableObject
{
    public int resolutionWidth;
    public int resolutionHeight;
    public FullScreenMode screenMode;
    public int qualitySettingsNum;
}
