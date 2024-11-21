using System.Collections.Generic;
using UnityEngine;

public class DirtRemover : MonoBehaviour
{
    public GameObject tempDirt, leaves;

    public int requiredSweeps = 3;
    private int currentSweeps = 0;
    public List<GameObject> targets;

    public void Sweep()
    {
        currentSweeps++;
        Debug.Log($"Dirt swept {currentSweeps}/{requiredSweeps} times.");

        if (currentSweeps >= requiredSweeps)
        {
            Destroy(tempDirt.gameObject);
            currentSweeps = 0;
        }
    }

    private void RemoveDirt(GameObject dirt)
    {
        if (targets.Contains(dirt))
        {
            targets.Remove(dirt);
            Debug.Log($"Dirt removed from list. Remaining dirt: {targets.Count}");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        tempDirt = other.gameObject;
        if (other.CompareTag("Dirt"))
        {
            print("test");
            Sweep();
        }
    }
}