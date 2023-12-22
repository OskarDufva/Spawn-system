using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    // References to other scripts and variables
    PlayerScore score;
    public List<EnemyData> enemyDataList; // List for enemy data
    public List<GameObject> enemyPrefabs; // List for enemy prefabs
    public List<Transform> spawnLocations;
    public float spawnCooldown = 10f;
    private float _nextSpawnTime;

    private List<ObjectPool<PoolObject>> _enemyPools; // List for object pools


    private void OnEnable()
    {
        // Find reference to PlayerScore and initialize object pools
        score = FindObjectOfType<PlayerScore>();

        // Initialize object pools based on the enemy prefabs
        _enemyPools = new List<ObjectPool<PoolObject>>();
        foreach (var prefab in enemyPrefabs)
        {
            _enemyPools.Add(new ObjectPool<PoolObject>(prefab));
        }

        _nextSpawnTime = Time.time;
        StartCoroutine(SpawnOverTime());
    }

    // Coroutine to spawn enemies over time
    IEnumerator SpawnOverTime()
    {
        while (true)
        {
            if (Time.time > _nextSpawnTime)
            {
                Spawn(); // Trigger enemy spawn
                _nextSpawnTime = Time.time + spawnCooldown;
            }

            yield return null;
        }
    }

    // Spawn an enemy at a random location
    public void Spawn()
    {
        // Calculate the total weight of all enemy types
        float totalWeight = 0f;
        foreach (var enemy in enemyDataList)
        {
            totalWeight += enemy.weight;
        }

        // Generate a random value between 0 and the total weight
        float randomValue = Random.Range(0f, totalWeight);

        // Choose the enemy type based on the weighted random value
        GameObject prefab = GetWeightedRandomEnemy(randomValue);

        Debug.Log($"Spawned enemy type: {prefab.name}, Random Value: {randomValue}, Total Weight: {totalWeight}");

        // Ensure there are valid spawn locations in the list
        if (spawnLocations.Count > 0)
        {
            int randomLocationIndex = Random.Range(0, spawnLocations.Count);
            Transform spawnPoint = spawnLocations[randomLocationIndex];

            int index = enemyDataList.FindIndex(ed => ed.prefab == prefab);
            int cost = (index != -1) ? enemyDataList[index].cost : 0;

            if (score.CanAfford(cost))
            {
                // Use the correct object pool for the specified prefab
                ObjectPool<PoolObject> objectPool = GetObjectPool(prefab);
                if (objectPool != null)
                {
                    GameObject spawnedObject = objectPool.PullGameObject(spawnPoint.position, Random.rotation);
                    // Additional initialization if needed
                    spawnedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    score.DeductRoundScore(cost);
                }
            }
            else
            {
                // Handle the case when the player cannot afford the enemy
                return;
            }
        }
        else
        {
            Debug.LogError("No spawn locations set in the Spawner component.");
            return;
        }
    }

    // Get the appropriate object pool for a given prefab
    private ObjectPool<PoolObject> GetObjectPool(GameObject prefab)
    {
        int index = enemyPrefabs.IndexOf(prefab);
        if (index != -1)
        {
            return _enemyPools[index];
        }

        return null;
    }

    // Get a weighted random enemy based on the provided random value
    private GameObject GetWeightedRandomEnemy(float randomValue)
    {
        float cumulativeWeight = 0f;

        // Iterate through enemy types and find the one corresponding to the random value
        for (int i = 0; i < enemyDataList.Count; i++)
        {
            cumulativeWeight += enemyDataList[i].weight;

            // If the random value is less than the cumulative weight, choose this enemy type
            if (randomValue <= cumulativeWeight)
            {
                return enemyDataList[i].prefab;
            }
        }

        // Fallback to the last enemy type if something goes wrong
        return enemyDataList[enemyDataList.Count - 1].prefab;
    }
}
