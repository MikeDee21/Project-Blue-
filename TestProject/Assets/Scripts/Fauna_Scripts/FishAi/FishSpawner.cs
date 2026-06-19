using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [Header("Spawning")]
    // Changed this to a list so you can drop multiple different fish prefabs here
    public List<GameObject> fishPrefabs = new List<GameObject>();
    public int maxFish = 20;
    public float spawnInterval = 2f;

    [Header("Spawn Area")]
    public Vector3 spawnArea = new Vector3(20f, 5f, 0f);

    List<GameObject> activeFish = new List<GameObject>();
    float timer;

    void Update()
    {
        activeFish.RemoveAll(f => f == null);
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            if (activeFish.Count < maxFish)
                SpawnFish();
        }
    }

    void SpawnFish()
    {
        // Safety check to ensure you've added fish to the list in the Inspector
        if (fishPrefabs == null || fishPrefabs.Count == 0)
        {
            Debug.LogWarning("Please assign at least one fish prefab to the Fish Spawner script!");
            return;
        }

        // 1. Calculate the randomized position across width (X) and height (Y)
        Vector3 pos = transform.position + new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            0f
        );

        // 2. Pick a random fish prefab from your list
        int randomPrefabIndex = Random.Range(0, fishPrefabs.Count);
        GameObject selectedPrefab = fishPrefabs[randomPrefabIndex];

        // 3. Spawn the randomly selected fish
        GameObject fish = Instantiate(selectedPrefab, pos, Quaternion.identity);

        // 4. Randomize the visual scale for size variety
        fish.transform.localScale = Vector3.one * Random.Range(0.8f, 1.5f);

        activeFish.Add(fish);
    }
}