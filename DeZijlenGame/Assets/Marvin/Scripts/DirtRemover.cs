using UnityEngine;

public class DirtRemover : MonoBehaviour
{
    public int requiredSweeps = 3; // Hoeveel keer de bezem moet langskomen
    private int currentSweeps = 0;

    // Functie die aangeroepen wordt wanneer de bezem de dirt raakt
    public void Sweep()
    {
        currentSweeps++;
        Debug.Log($"Dirt swept {currentSweeps}/{requiredSweeps} times.");

        if (currentSweeps >= requiredSweeps)
        {
            RemoveDirt();
        }
    }

    private void RemoveDirt()
    {
        Debug.Log("Dirt removed!");
        Destroy(gameObject); // Verwijdert het dirt-object uit de scene
    }
}

public class Broom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dirt"))
        {
            DirtRemover dirt = other.GetComponent<DirtRemover>();
            if (dirt != null)
            {
                dirt.Sweep(); // Roept de Sweep-functie aan op het dirt-object
            }
        }
    }
}
