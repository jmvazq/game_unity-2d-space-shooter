using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float _speed = 3.8f;
    [SerializeField] private float _speedBoostMultiplier = 2.0f;

    [SerializeField] private int _lives = 3;
    private bool _isDamaging = false;

    [SerializeField] private GameObject _shieldVisualizer;

    [Header("Firing")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _laserContainer;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    [SerializeField] private GameObject _tripleShotPrefab;

    [SerializeField] private float _fireRate = 0.15f;
    private float _nextFire = 0.0f;

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = FindObjectOfType<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }
    }

    private void HandleMovement()
    {
        // Handle movement via player input

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        Vector3 newDirection = new Vector3(1 * hInput, 1 * vInput, 0);

        transform.Translate(newDirection * _speed * Time.deltaTime);

        // Restrict player position based on screen / level boundaries
        float topBound = 1.0f;
        float bottomBound = -3.5f;
        float leftBound = -11.18f;
        float rightBound = -leftBound;

        float xPos = transform.position.x;

        // horizontal screen wrap
        if (transform.position.x < leftBound)
        {
            xPos = rightBound;
        }
        else if (transform.position.x > rightBound)
        {
            xPos = leftBound;
        }

        float yPos = Mathf.Clamp(transform.position.y, bottomBound, topBound);

        transform.position = new Vector3(xPos, yPos, 0);
    }

    private void FireLaser()
    {
        _nextFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            // Fire Triple Shot
            GameObject tripleShot = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            tripleShot.transform.SetParent(_laserContainer.transform);
        }
        else
        {
            // Fire single laser
            Vector3 posOffset = new Vector3(0, 1.05f, 0);
            GameObject newLaser = Instantiate(_laserPrefab, transform.position + posOffset, Quaternion.identity);
            newLaser.transform.SetParent(_laserContainer.transform);
        }
    }

    IEnumerator DamageFlash()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        Color originalColor = mat.color;

        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mat.color = originalColor;
    }

    IEnumerator TakeDamageCoroutine()
    {
        _isDamaging = true;

        StartCoroutine(DamageFlash());
        yield return new WaitForSeconds(0.25f);

        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject, 0.1f);
        }

        _isDamaging = false;
    }

    internal void TakeDamage()
    {
        if (!_isDamaging)
        {
            if (_isShieldActive)
            {
                DeactivateShield();
                return;
            }

            StartCoroutine(TakeDamageCoroutine());
        }
    }

    public void ActivateTripleShot()
    {
        if (_isTripleShotActive)
        {
            return;
        }

        _isTripleShotActive = true;
        StartCoroutine(CooldownTripleShotRoutine());
    }

    IEnumerator CooldownTripleShotRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void ActivateSpeedBoost()
    {
        if(_isSpeedBoostActive)
        {
            return;
        }

        _isSpeedBoostActive = true;
        _speed *= _speedBoostMultiplier;
        StartCoroutine(CooldownSpeedBoostRoutine());
    }

    IEnumerator CooldownSpeedBoostRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedBoostMultiplier;
    }

    public void ActivateShield()
    {
        if (_isShieldActive)
        {
            return;
        }

        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    private void DeactivateShield()
    {
        _isShieldActive = false;
        _shieldVisualizer.SetActive(false);
    }
}
