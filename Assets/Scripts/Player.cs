using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.8f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _nextFire = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Set starting position
        transform.position = new Vector3(0, 0, 0);
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
        float topBound = 0f;
        float bottomBound = -3.8f;
        float leftBound = -11.26f;
        float rightBound = 11.26f;

        // horizontal screen wrap
        float xPos = transform.position.x;

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
        Object.Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        _nextFire = Time.time + _fireRate;
    }
}
