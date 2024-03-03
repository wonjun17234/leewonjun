using System;
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
    public void ClickButton()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
    }
}
