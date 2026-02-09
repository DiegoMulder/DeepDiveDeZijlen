using System.Collections.Generic;
using UnityEngine;

public class DirtRemover : MonoBehaviour
{
    public GameObject tempGameObject;

    public int requiredSweeps = 3;
    private int currentSweeps = 0;
    public List<GameObject> targets;

    /// <summary>
    /// Voert een veegactie uit op het vuil. Als het aantal benodigde vegen is bereikt, wordt het vuil verwijderd. Zorg ervoor dat de targets-lijst correct is ingesteld in de Inspector!
    /// </summary>
    public void Sweep()
    {
        currentSweeps++;
        Debug.Log($"Dirt swept {currentSweeps}/{requiredSweeps} times.");

        if (currentSweeps >= requiredSweeps)
        {
            Destroy(tempGameObject.gameObject);
            currentSweeps = 0;
        }
    }

    // Wordt niet gebruikt
    private void RemoveGameObject(GameObject dirt)
    {
        if (targets.Contains(dirt))
        {
            targets.Remove(dirt);
            Debug.Log($"Dirt removed from list. Remaining dirt: {targets.Count}");
        }
    }

    // Detecteert wanneer een collider met het tag "Dirt" binnenkomt en voert de veegactie uit. Zorg ervoor dat de vuilobjecten het tag "Dirt" hebben toegewezen in de Inspector!
    private void OnTriggerEnter(Collider other)
    {
        tempGameObject = other.gameObject;
        if (other.CompareTag("Dirt"))
        {
            Sweep();
        }
    }
}