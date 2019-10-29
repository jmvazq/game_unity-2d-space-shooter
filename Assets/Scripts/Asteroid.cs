using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 19.0f;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager not found!");
        }

        if (_explosionPrefab == null)
        {
            Debug.Log("Explosion prefab is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // rotate on z axis
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Laser laser = other.gameObject.GetComponent<Laser>();
            if (laser != null)
            {
                if (_explosionPrefab != null)
                {
                    Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                    Destroy(other.gameObject);
                }
                _spawnManager.StartSpawning();
                Destroy(this.gameObject, 0.25f);
            }
        }
    }
}
