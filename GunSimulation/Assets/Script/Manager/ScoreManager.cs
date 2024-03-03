using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int level = 0;
    private float time = 50;
    private int enemyNum = 0;
    private int currentEnemyNum = 0;
    public Text text;
    public int score = 0;

    public GameObject play;
    public GameObject option;
    public bool isOptionOn;
    public GameObject player;

    public GameObject enemyPrefab;
    public GameObject enemyBase;
    void Start()
    {
        isOptionOn = false;
        text.text = "score : " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= 20f)
        {
            score += (enemyNum - currentEnemyNum) * level * 100;
            Reset();
            time = 0;
            level++;
            enemyNum = (3 + level) / 2;
            for (int i =0; i < enemyNum; i++)
            {
                float random = Random.Range(-4f, 4f);
                Instantiate(enemyPrefab, enemyBase.transform.position + Vector3.forward * random, enemyBase.transform.rotation, enemyBase.transform);
            }
        }

        currentEnemyNum = enemyBase.transform.childCount;

        text.text = "score : " + (score + (enemyNum - currentEnemyNum) * level * 100).ToString();

        if (enemyBase.transform.childCount == 0)
        {
            time = 20f;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ClickOptionOut();
        }

    }

    public void Reset()
    {    
        for (int i = 0; i < enemyBase.transform.childCount; i++)
        {
            Destroy(enemyBase.transform.GetChild(i).gameObject);
        }
    }

    public void ClickOptionOut()
    {
        play.SetActive(isOptionOn);
        isOptionOn = !isOptionOn;
        option.SetActive(isOptionOn);
        if (isOptionOn)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = isOptionOn;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = isOptionOn;
        }
    }
}
