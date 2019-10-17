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

    private Dictionary<int, List<int>> _pwSpawnWeights;
    private int _pwSpawnWeightTotal = 0;
    private int[] _pwUniqueSpawnWeights;

    // Start is called before the first frame update
    void Start()
    {
        SetupPowerupSpawnWeights();

        StartCoroutine(SpawnEnemiesRoutine());
        StartCoroutine(SpawnPowerupsRoutine());
    }

    private void SetupPowerupSpawnWeights()
    {
        _pwSpawnWeights = new Dictionary<int, List<int>>() { };

        for (int i = 0; i < _powerups.Length; i++)
        {
            Powerup powerup = _powerups[i].GetComponent<Powerup>();
            if (powerup != null)
            {
                int weight = powerup.spawnChance;
                if (_pwSpawnWeights.ContainsKey(weight))
                {
                    _pwSpawnWeights[weight].Add(i);
                }
                else
                {
                    _pwSpawnWeights.Add(weight, new List<int>() { i });
                    _pwSpawnWeightTotal += weight;
                }
            }
        }

        _pwUniqueSpawnWeights = _pwSpawnWeights.Keys.ToArray();
        System.Array.Sort(_pwUniqueSpawnWeights);
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
            int randomIndex = GetRandomPowerupIndex();
            if (randomIndex >= 0 && randomIndex < _powerups.Length)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7.8f, 0);
                GameObject powerup = Instantiate(_powerups[randomIndex], posToSpawn, Quaternion.identity);
            } else
            {
                Debug.Log("Unspecified powerup!");
            }
        }
    }

    private int GetRandomPowerupIndex()
    {
        int randomNumber = Random.Range(0, _pwSpawnWeightTotal);
        int index = -1;

        for (int i = 0; i < _pwUniqueSpawnWeights.Length; i++)
        {
            int weight = _pwUniqueSpawnWeights[i];

            if (randomNumber <= weight)
            {
                var numOptions = _pwSpawnWeights[weight].Count();
                if (numOptions > 1)
                {
                    index = Random.Range(0, numOptions);
                }
                else
                {
                    index = _pwSpawnWeights[weight][0];
                }
                break;
            }
            else
            {
                randomNumber -= weight;
            }
        }

        return index;
    }

    public void OnPlayerDeath()
    {
        _spawnEnabled = false;
    }
}
