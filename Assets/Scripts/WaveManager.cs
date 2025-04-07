using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public bool SpawnAutomatically = true;
    public GameObject[] Enemy;
    public Transform[] SpawnPoints;
    public float TimeBetweenWaves = 5f;
    public int EnemiesPerWave = 5;

    // Probability a tanked is spawned is tankSpawnFactor / (tankSpawnFactor + rangedSpawnFact + meleeSpawnFactor)
    public int tankSpawnFactor = 1;
    public int rangedSpawnFactor = 5;
    public int meleeSpawnFactor = 5;

    private float countdown = 0f;

    void Update()
    {
        if(!SpawnAutomatically) return;

        if (countdown <= 0f)
        {
            SpawnWave();
            countdown = TimeBetweenWaves;
        }
        countdown -= Time.deltaTime;
    }

    public void SpawnWave()
    {
        EngineScript.curRound += 1;
        for (int i = 0; i < EnemiesPerWave; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Pick a random spawn point and enemy type
        int spawnIndex = Random.Range(0, SpawnPoints.Length);

        // Yes if you need add an additional unit you need to add an additional weight
        int[] EnemyWeights = {meleeSpawnFactor, rangedSpawnFactor, tankSpawnFactor};
        int enemyIndex = GetWeightedRandomIndex(EnemyWeights);

        // Spawn the enemy
        GameObject newEnemy = Instantiate(Enemy[enemyIndex],
                                          SpawnPoints[spawnIndex].position,
                                          SpawnPoints[spawnIndex].rotation);
    }

    // This will throw an out of bounds error if weights is a zero length array
    private int GetWeightedRandomIndex(int[] weights)
    {
        //https://leetcode.com/problems/random-pick-with-weight/description/

        // Get Prefix Sum of Weights
        int[] prefixSum = new int[weights.Length];
        prefixSum[0] = weights[0];
        for(int i = 1; i < prefixSum.Length; i++)
        {
            prefixSum[i] = weights[i] + prefixSum[i-1];
        }

        // Select random value using weights
        int totalWeights = prefixSum[prefixSum.Length - 1];
        int randomNum = Random.Range(0, totalWeights);
        for(int i = 0; i < prefixSum.Length; i++)
        {
            if(randomNum < prefixSum[i])
            {
                return i;
            }
        }

        return 0;
    }   
}
