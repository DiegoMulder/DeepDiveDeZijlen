using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel, startNieuwPanel, playerDataPanel;
    public TMP_InputField voorNaam, achterNaam;
    public Button startButton;
    public KaartManager kaartManager; // Referentie naar KaartManager

    void Start()
    {
        startButton.interactable = false;
    }

    void Update()
    {
        // Activeer de knop alleen als velden zijn ingevuld
        startButton.interactable = !string.IsNullOrEmpty(voorNaam.text) && !string.IsNullOrEmpty(achterNaam.text);
    }

    public void StartBehaviour()
    {
        string voornaam = voorNaam.text;
        string achternaam = achterNaam.text;

        // Voeg een nieuw kaartje toe via KaartManager
        if (kaartManager != null)
        {
            kaartManager.VoegNieuwKaartjeToe(voornaam, achternaam);
        }

        // Scene wisselen
        Debug.Log("Switching to the next scene...");
        SceneManager.LoadScene(1); // Controleer of "1" de juiste index is in Build Settings
    }



    public void OnStartNieuw() => startNieuwPanel.SetActive(true);

    public void OffStartNieuw() => startNieuwPanel.SetActive(false);

    public void OnPlayerData() => playerDataPanel.SetActive(true);

    public void OffPlayerData() => playerDataPanel.SetActive(false);
}
