using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartPage : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);

    }
    public void ClickOption()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void ClickStart()
    {
        //로딩 화면으로 이동 또는 모드 선택
    }
}
