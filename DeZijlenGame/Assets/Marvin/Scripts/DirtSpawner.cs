using System.Collections.Generic;
using UnityEngine;

public class DirtSpawner : MonoBehaviour
{
    public GameObject dirtPrefab;
    public List<Transform> spawnPoints;
    public int numberOfDirtToSpawn = 5;

    void Start()
    {
        SpawnDirtObjects();
    }

    private void SpawnDirtObjects()
    {
        if (dirtPrefab == null || spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("Zorg ervoor dat je een prefab en spawnpunten hebt toegewezen!");
            return;
        }

        for (int i = 0; i < numberOfDirtToSpawn; i++)
        {
            if (spawnPoints.Count == 0)
            {
                Debug.LogWarning("Geen spawnpunten meer beschikbaar!");
                break;
            }

            int randomIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];

            Instantiate(dirtPrefab, spawnPoint.position, spawnPoint.rotation);

            spawnPoints.RemoveAt(randomIndex);
        }
    }
}
