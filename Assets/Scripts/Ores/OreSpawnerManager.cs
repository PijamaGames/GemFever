using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreSpawnerManager : MonoBehaviour
{
    [SerializeField] int oresToSpawn = 3;
    [SerializeField] GameObject orePrefab;
    List<OreSpawnPoint> spawnPoints = new List<OreSpawnPoint>();

    // Start is called before the first frame update
    void Start()
    {
        FindSpawnPoints();
        SpawnOres();
    }

    void FindSpawnPoints()
    {
        OreSpawnPoint[] foundSpawnPoints = FindObjectsOfType<OreSpawnPoint>();
        if (foundSpawnPoints == null)
            foundSpawnPoints = GetComponentsInChildren<OreSpawnPoint>();

        foreach (OreSpawnPoint spawnPoint in foundSpawnPoints)
        {
            this.spawnPoints.Add(spawnPoint);
        }
    }

    void SpawnOres()
    {
        int randomIndex = -1;

        for(int i = 0; i < oresToSpawn; i++)
        {
            randomIndex = Random.Range(0, spawnPoints.Count);
            GameObject.Instantiate(orePrefab, spawnPoints[randomIndex].transform.position, Quaternion.identity);
            spawnPoints.RemoveAt(randomIndex);
        }
    }
}
