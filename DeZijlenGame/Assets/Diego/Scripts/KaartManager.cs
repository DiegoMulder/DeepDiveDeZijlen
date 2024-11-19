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
    private List<KaartData> kaartLijst = new List<KaartData>();
    private string savePath;

    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "kaartjes.json");
        LaadKaartjes();
    }

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

        // Voeg toe aan de lijst en maak UI-kaart
        kaartLijst.Add(nieuweKaart);
        MaakKaartjeUI(nieuweKaart);

        // Sla de gegevens op
        SlaKaartjesOp();
    }

    private void MaakKaartjeUI(KaartData kaartData)
    {
        // Instantieer de kaart prefab
        GameObject nieuwKaartje = Instantiate(kaartPrefab, content);

        // Update de UI-elementen
        nieuwKaartje.transform.Find("VoornaamText").GetComponent<TMP_Text>().text = kaartData.voornaam;
        nieuwKaartje.transform.Find("AchternaamText").GetComponent<TMP_Text>().text = kaartData.achternaam;
        nieuwKaartje.transform.Find("OpdrachtenText").GetComponent<TMP_Text>().text =
            $"{kaartData.opdrachtenGedaan}/{kaartData.maxOpdrachten}";

        // Voeg functionaliteit toe aan de Delete-knop
        Button deleteButton = nieuwKaartje.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() =>
        {
            kaartLijst.Remove(kaartData);
            Destroy(nieuwKaartje);
            SlaKaartjesOp();
        });
    }

    private void SlaKaartjesOp()
    {
        Debug.Log("Kaartjes worden opgeslagen...");
        string json = JsonUtility.ToJson(new KaartDataWrapper { kaartjes = kaartLijst }, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"Kaartjes opgeslagen naar {savePath}");
    }


    private void LaadKaartjes()
    {
        // Controleer of het bestand bestaat
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            KaartDataWrapper wrapper = JsonUtility.FromJson<KaartDataWrapper>(json);
            kaartLijst = wrapper.kaartjes;

            // Maak kaartjes in de UI
            foreach (KaartData kaart in kaartLijst)
            {
                MaakKaartjeUI(kaart);
            }
        }
    }

    [System.Serializable]
    private class KaartDataWrapper
    {
        public List<KaartData> kaartjes;
    }
}
