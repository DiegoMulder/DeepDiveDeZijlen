using System.Collections.Generic;
using UnityEngine;

public class DirtSpawner : MonoBehaviour
{
    public GameObject dirtPrefab, leavesPrefab;
    public  List<GameObject> dirtSpawnPoints, leavesSpawnPoints;
    public int numberOfDirtToSpawn = 5, numbersOfLeavesToSpawn = 5;

    void Start()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        if (leavesSpawnPoints.Count == 0 || leavesSpawnPoints == null || leavesPrefab == null || dirtPrefab == null || dirtSpawnPoints == null || dirtSpawnPoints.Count == 0)
        {
            Debug.LogWarning("Zorg ervoor dat je een prefab en spawnpunten hebt toegewezen!");
            return;
        }

        for (int i = 0; i < numberOfDirtToSpawn; i++)
        {
            if (dirtSpawnPoints.Count == 0)
            {
                Debug.LogWarning("Geen spawnpunten meer beschikbaar!");
                break;
            }

            int randomIndex = Random.Range(0, dirtSpawnPoints.Count);
            GameObject spawnPoint = dirtSpawnPoints[randomIndex];

            Instantiate(dirtPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);

            dirtSpawnPoints.RemoveAt(randomIndex);
        }

        for (int i = 0; i < numbersOfLeavesToSpawn; i++)
        {
            if (leavesSpawnPoints.Count == 0)
            {
                Debug.LogWarning("Geen spawnpunten meer beschikbaar!");
                break;
            }

            int randomIndex = Random.Range(0, leavesSpawnPoints.Count);
            GameObject spawnPoint = leavesSpawnPoints[randomIndex];

            Instantiate(leavesPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);

            leavesSpawnPoints.RemoveAt(randomIndex);
        }
    }
}
