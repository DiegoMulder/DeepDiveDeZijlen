using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeldScript : MonoBehaviour
{
    public float Value; // Waarde van de munt/biljet
    private KassaGame kassaGame;

    void Start()
    {
        kassaGame = FindObjectOfType<KassaGame>();
    }

    public void OnClick()
    {
        kassaGame.AddToChange(Value);
    }
}
