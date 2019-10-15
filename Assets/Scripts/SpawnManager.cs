using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    private bool _spawnEnabled = true;

    [Header("Enemy Spawning")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

    [SerializeField] private float _minEnemySpawnRate = 3.0f;
    [SerializeField] private float _maxEnemySpawnRate = 7.0f;

    [Header("Powerup Spawning")]
    [SerializeField] private float _minPowerupSpawnRate = 3.0f;
    [SerializeField] private float _maxPowerupSpawnRate = 8.0f;
    [SerializeField] private GameObject[] _powerups;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
        StartCoroutine(SpawnPowerupsRoutine());
    }

    // Spawn enemies every few seconds
    IEnumerator SpawnEnemiesRoutine()
    {
        while(_spawnEnabled)
        {
            SpawnEnemies(1);
            yield return new WaitForSeconds(Random.Range(_minEnemySpawnRate, _maxEnemySpawnRate));
        }
    }

    // Spawn powerups every few seconds
    IEnumerator SpawnPowerupsRoutine()
    {
        while (_spawnEnabled)
        {
            SpawnPowerups(1);
            yield return new WaitForSeconds(Random.Range(_minPowerupSpawnRate, _maxPowerupSpawnRate));
        }
    }

    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // positioned at bottom since the enemy already positions itself randomly when reaching that point
            Vector3 posToSpawn = new Vector3(0, -12.0f, 0);
            GameObject enemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
        }
    }

    private void SpawnPowerups(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, _powerups.Count());
            Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7.8f, 0);
            GameObject powerup = Instantiate(_powerups[randomIndex], posToSpawn, Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _spawnEnabled = false;
    }
}
