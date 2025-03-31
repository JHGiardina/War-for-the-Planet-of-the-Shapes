using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public GameObject enemy;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    public int enemiesPerWave = 5;

    private float countdown = 0f;
    private int waveNumber = 0;

    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        waveNumber++;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0f);
        }
    }

    void SpawnEnemy()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(enemy, spawnPoints[index].position, spawnPoints[index].rotation);
    }
}