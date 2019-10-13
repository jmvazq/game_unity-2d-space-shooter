using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    private static int _enemyPoolSize = 5;
    private List<GameObject> _enemyPool;

    [SerializeField]
    private float _spawnRate = 5.0f;

    private bool _spawnEnabled = true;


    // Start is called before the first frame update
    void Start()
    {
        CreateEnemyPool();
        StartCoroutine("SpawnGameObjects");
    }

    // Update is called once per frame
    void Update()
    {
        // TODO
    }

    private void CreateEnemyPool()
    {
        if (_enemyPrefab == null)
        {
            return;
        }

        _enemyPool = new List<GameObject>(_enemyPoolSize);

        for (int i = 0; i < _enemyPoolSize; i++)
        {
            GameObject newEnemy = Object.Instantiate(_enemyPrefab, Vector3.zero, Quaternion.identity);
            newEnemy.SetActive(false);
            newEnemy.transform.SetParent(_enemyContainer.transform);

            _enemyPool.Add(newEnemy);
        }
    }

    // Spawn enemies and other game objects every 5 seconds
    IEnumerator SpawnGameObjects()
    {
        while(_spawnEnabled)
        {
            SpawnEnemies(1);
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    private void SpawnEnemies(int count)
    {
        List<GameObject> inactiveEnemies = _enemyPool.Where(e => !e.activeInHierarchy).ToList<GameObject>();
        if (inactiveEnemies.Count() < count)
        {
            count = inactiveEnemies.Count();
        }

        for (int i = 0; i < count; i++)
        {
            GameObject enemy = inactiveEnemies[i];
            enemy.transform.position = new Vector3(0, -12.0f, 0);
            enemy.SetActive(true);
        }
    }

    public void OnPlayerDeath()
    {
        _spawnEnabled = false;
    }
}
