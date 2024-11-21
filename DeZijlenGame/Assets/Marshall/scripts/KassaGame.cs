using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KassaGame : MonoBehaviour
{
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI PaidText;
    public TextMeshProUGUI ChangeText;
    public TextMeshProUGUI ResultText; // Om feedback te geven (bijv. "Correct!" of "Te veel wisselgeld!")

    private float price;
    private float paid;
    private float change;

    public List<float> AvailableMoney = new List<float> { 0.01f, 0.05f, 0.10f, 0.20f, 0.50f, 1.00f, 5.00f, 10.00f };

    private float currentChangeGiven;

    void Start()
    {
        GenerateNewTransaction();
    }

    public void GenerateNewTransaction()
    {
        // Reset game variables
        currentChangeGiven = 0;

        // Randomize price and paid amount
        price = Random.Range(1.0f, 50.0f); // Random prijs tussen €1 en €50
        paid = Random.Range(price + 1.0f, price + 20.0f); // Random betaald bedrag

        change = Mathf.Round((paid - price) * 100f) / 100f; // Zorg dat wisselgeld 2 decimalen heeft

        // Update UI
        PriceText.text = "Prijs: €" + price.ToString("F2");
        PaidText.text = "Betaald: €" + paid.ToString("F2");
        ChangeText.text = "Wisselgeld: €" + change.ToString("F2");
        ResultText.text = "";
    }

    public void AddToChange(float amount)
    {
        currentChangeGiven += amount;
        CheckChange();
    }

    private void CheckChange()
    {
        if (currentChangeGiven == change)
        {
            ResultText.text = "Correct! Wisselgeld gegeven!";
            Invoke("GenerateNewTransaction", 2); // Na 2 seconden nieuwe transactie
        }
        else if (currentChangeGiven > change)
        {
            ResultText.text = "Te veel wisselgeld!";
        }
    }
}
