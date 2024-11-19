using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel, startNieuwPanel, playerDataPanel;
    public TMP_InputField voorNaam, achterNaam;
    public Button startButton;
    public KaartManager kaartManager;

    void Start()
    {
        startButton.interactable = false; // Knop begint inactief
    }

    void Update()
    {
        // Controleer invoer en activeer de knop alleen als de namen geldig zijn
        startButton.interactable = IsValidName(voorNaam.text) && IsValidName(achterNaam.text);
    }

    public void StartBehaviour()
    {
        if (kaartManager == null)
        {
            Debug.LogError("KaartManager is niet ingesteld in de Inspector.");
            return;
        }

        if (string.IsNullOrEmpty(voorNaam.text) || string.IsNullOrEmpty(achterNaam.text))
        {
            Debug.LogError("Voornaam of Achternaam is leeg.");
            return;
        }

        string voornaam = voorNaam.text;
        string achternaam = achterNaam.text;

        Debug.Log($"Kaartje toevoegen: Voornaam={voornaam}, Achternaam={achternaam}");
        kaartManager.VoegNieuwKaartjeToe(voornaam, achternaam);

        Debug.Log("Scenewissel wordt aangeroepen.");
        SceneManager.LoadScene(1);
    }

    public void OnStartNieuw() => startNieuwPanel.SetActive(true);

    public void OffStartNieuw() => startNieuwPanel.SetActive(false);

    public void OnPlayerData() => playerDataPanel.SetActive(true);

    public void OffPlayerData() => playerDataPanel.SetActive(false);

    // Valideert een naam: moet niet leeg zijn en alleen letters bevatten
    private bool IsValidName(string input)
    {
        return !string.IsNullOrEmpty(input) && Regex.IsMatch(input, @"^[a-zA-Z]+$");
    }
}
