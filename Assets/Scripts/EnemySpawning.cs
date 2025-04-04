using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public GameObject[] Enemy;
    public GameObject healthBarPrefab;
    public Transform[] SpawnPoints;
    public float TimeBetweenWaves = 5f;
    public int EnemiesPerWave = 5;

    private float countdown = 0f;
    private int waveNumber = 0;

    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = TimeBetweenWaves;
        }
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        waveNumber++;

        for (int i = 0; i < EnemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0f);
        }
    }

    void SpawnEnemy()
    {
        // Pick a random spawn point and enemy type
        int spawnIndex = Random.Range(0, SpawnPoints.Length);
        int enemyIndex = Random.Range(0, Enemy.Length);

        // Spawn the enemy
        GameObject newEnemy = Instantiate(Enemy[enemyIndex],
                                          SpawnPoints[spawnIndex].position,
                                          SpawnPoints[spawnIndex].rotation);

        // Spawn the Health Bar
        GameObject newHealthBar = Instantiate(healthBarPrefab);

        // Link them
        UnitHealthBar barScript = newHealthBar.GetComponent<UnitHealthBar>();
        if (barScript != null)
        {
            UnitHealth unitHealth = newEnemy.GetComponent<UnitHealth>();
            barScript.targetHealth = unitHealth;
        }
    }
}
