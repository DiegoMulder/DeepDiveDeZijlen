using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gameSelector;
    public GameObject optionsMenu;

    public void StartButton()
    {
        mainMenu.SetActive(false);
        gameSelector.SetActive(true);
    }

    public void BackToMain()
    {
        mainMenu.SetActive(true);
        gameSelector.SetActive(false);
    }

    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void CloseGameSelector()
    {
        gameSelector.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        //Environment.Exit(69);
        Debug.Log("Close Game");
    }

    public void OpenHarkSpel()
    {
        Debug.Log("Open het hark spelletje");
    }

    public void OpenSorteerSpel()
    {
        Debug.Log("Open het sorteer spelletje");
    }

    public void OpenSchoonmaakSpel()
    {
        Debug.Log("Open het schoonmaak spelletje");
    }

    public void OpenFotografieSpel()
    {
        Debug.Log("Open het fotografie spelletje");
    }

    public void Open___Spel()
    {
        Debug.Log("Open het ... spelletje");
    }
}
