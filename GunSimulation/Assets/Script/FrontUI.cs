using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FrontUI : MonoBehaviour
{
    public GameObject bluePrefab;
    public GameObject blueBase;
    public Text blueText;
    private int bluePeople;
    public GameObject redPrefab;
    public GameObject redBase;
    public Text redText;
    private int redPeople;

    public Text speedText;

    private int[] timeScales = {1, 2, 4, 8, 16};
    private int timeIndex = 0;


    public void restart()
    {
        for(int i =0; i < bluePeople; i++)
        {
            Destroy(blueBase.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < redPeople; i++)
        {
            Destroy(redBase.transform.GetChild(i).gameObject);
        }
    }
    public void createEnemy(bool isBlue)
    {
        float random = Random.Range(-4f,4f);
        if(isBlue && bluePeople < 5)
        {
            Instantiate(bluePrefab, blueBase.transform.position + Vector3.forward * random, blueBase.transform.rotation, blueBase.transform);
        }
        else if(!isBlue && redPeople < 5)
        {
            Instantiate(redPrefab, redBase.transform.position + Vector3.forward * random, redBase.transform.rotation, redBase.transform);
        }
    }
    public void speedSet()
    {
        timeIndex = (timeIndex + 1) % 5;
        Time.timeScale = timeScales[timeIndex];
        speedText.text = "X" + timeScales[timeIndex];
    }
    public void LateUpdate()
    {
        bluePeople = blueBase.transform.childCount;
        redPeople = redBase.transform.childCount;
        blueText.text = bluePeople + "/5";
        redText.text = redPeople +"/5";

    }
}
