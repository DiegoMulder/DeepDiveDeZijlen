using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

[System.Serializable]
public class KaartData
{
    public string voornaam;
    public string achternaam;
    public int opdrachtenGedaan;
    public int maxOpdrachten;
}

public class KaartManager : MonoBehaviour
{
    public GameObject kaartPrefab; // Prefab voor een kaartje
    public Transform content; // Content object binnen Scroll View
    private List<KaartData> kaartLijst = new List<KaartData>(); // De lijst van kaartjes
    private string savePath;

    void Start()
    {
        // Bepaal het pad voor het opslaan van kaartjes
        savePath = Path.Combine(Application.persistentDataPath, "kaartjes.json");

        // Log het pad om te zien of het correct wordt ingesteld
        Debug.Log("Kaartjes zullen opgeslagen worden op: " + savePath);

        // Laad de kaartjes bij het opstarten
        LaadKaartjes();
    }

    // Voeg een nieuw kaartje toe aan de lijst en de UI
    public void VoegNieuwKaartjeToe(string voornaam, string achternaam)
    {
        // Maak een nieuw KaartData-object
        KaartData nieuweKaart = new KaartData
        {
            voornaam = voornaam,
            achternaam = achternaam,
            opdrachtenGedaan = 0,
            maxOpdrachten = 10
        };

        // Voeg het nieuwe kaart toe aan de lijst en maak de bijbehorende UI
        kaartLijst.Add(nieuweKaart);
        MaakKaartjeUI(nieuweKaart);

        // Sla de kaartjes op
        SlaKaartjesOp();
    }

    // Maak de UI voor een kaartje
    private void MaakKaartjeUI(KaartData kaartData)
    {
        // Instantieer de kaart prefab in de UI
        GameObject nieuwKaartje = Instantiate(kaartPrefab, content);

        // Update de tekst van de UI-elementen
        nieuwKaartje.transform.Find("VoornaamText").GetComponent<TMP_Text>().text = kaartData.voornaam;
        nieuwKaartje.transform.Find("AchternaamText").GetComponent<TMP_Text>().text = kaartData.achternaam;
        nieuwKaartje.transform.Find("OpdrachtenText").GetComponent<TMP_Text>().text =
            $"{kaartData.opdrachtenGedaan}/{kaartData.maxOpdrachten}";

        // Voeg functionaliteit toe aan de Delete-knop
        Button deleteButton = nieuwKaartje.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() =>
        {
            // Verwijder het kaartje uit de lijst en de UI
            kaartLijst.Remove(kaartData); // Verwijder het kaartje uit de lijst
            Destroy(nieuwKaartje); // Verwijder het kaartje uit de UI

            // Sla de gewijzigde lijst op
            SlaKaartjesOp();
        });
    }

    public void SlaKaartjesOp()
    {
        // Controleer of het pad geldig is
        if (string.IsNullOrEmpty(savePath))
        {
            Debug.LogError("Opslagpad is niet ingesteld.");
            return;
        }

        // Maak een KaartDataWrapper om de lijst van kaartjes op te slaan
        KaartDataWrapper wrapper = new KaartDataWrapper();
        wrapper.kaartjes = kaartLijst;

        // Serialiseer de lijst van kaartjes naar JSON
        string jsonData = JsonUtility.ToJson(wrapper, true);

        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError("JSON-data is leeg. Opslaan geannuleerd.");
            return;
        }

        // Log het pad om te verifiëren of het correct is
        Debug.Log($"Opslaan naar pad: {savePath}");

        try
        {
            File.WriteAllText(savePath, jsonData);
            Debug.Log("Kaartjes succesvol opgeslagen.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Fout bij opslaan: {ex.Message}");
        }
    }

    // Laad de kaartjes van het bestand
    private void LaadKaartjes()
    {
        // Verwijder eerst alle bestaande kaartjes in de UI
        foreach (Transform child in content)
        {
            Destroy(child.gameObject); // Verwijder de oude UI-elementen
        }

        // Controleer of het bestand bestaat
        if (File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                KaartDataWrapper wrapper = JsonUtility.FromJson<KaartDataWrapper>(json);

                // Update de lijst van kaartjes met de geladen data
                kaartLijst = wrapper.kaartjes;

                // Maak de kaartjes in de UI op basis van de geladen data
                foreach (KaartData kaart in kaartLijst)
                {
                    MaakKaartjeUI(kaart); // Voeg elk kaartje opnieuw toe aan de UI
                }

                Debug.Log("Kaartjes succesvol geladen.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Fout bij laden: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Geen kaartjesbestand gevonden.");
        }
    }

    // Een wrapper om de lijst van KaartData te kunnen serialiseren naar JSON
    [System.Serializable]
    private class KaartDataWrapper
    {
        public List<KaartData> kaartjes;
    }
}
