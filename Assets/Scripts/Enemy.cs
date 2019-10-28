using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float _speed = 4.0f;
    
    private bool _isDamaging = false;

    private Material _mat;
    private Color _originalColor = Color.white;

    private Player _player;
    private UIManager _ui;

    void Start()
    {
        _player = FindObjectOfType<Player>();
        if (_player == null)
        {
            Debug.Log("Player not found!");
        }

        _ui = FindObjectOfType<UIManager>();
        if (_ui == null)
        {
            Debug.Log("UI Manager not found!");
        }

        _mat = GetComponent<SpriteRenderer>().material;
        if (_mat != null)
        {
            _originalColor = _mat.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        float topBound = 7.0f;
        float bottomBound = -5.27f;
        float leftBound = -8.0f;
        float rightBound = 8.0f;

        float xPos = transform.position.x;
        float yPos = transform.position.y;

        // Respawn at top with random x position
        if (transform.position.y < bottomBound)
        {
            xPos = Random.Range(leftBound, rightBound);
            yPos = topBound;
        }

        transform.position = new Vector3(xPos, yPos, 0.0f);
    }

    IEnumerator DamageFlash()
    {
        if (_mat != null)
        {
            _mat.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _mat.color = _originalColor;
        }
    }

    IEnumerator TakeDamageCoroutine()
    {
        _isDamaging = true;

        StartCoroutine(DamageFlash());
        yield return new WaitForSeconds(0.25f);

        Destroy(this.gameObject);

        _isDamaging = false;
    }

    internal void TakeDamage()
    {
        if (!_isDamaging)
        {
            StartCoroutine(TakeDamageCoroutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null && !_isDamaging)
            {
                player.TakeDamage();
                TakeDamage();
            }
        }
        else if (other.tag == "Laser")
        {
            Laser laser = other.gameObject.GetComponent<Laser>();
            if (laser != null && !_isDamaging)
            {
                Destroy(laser.gameObject);
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                TakeDamage();
            }
        }
    }
}
