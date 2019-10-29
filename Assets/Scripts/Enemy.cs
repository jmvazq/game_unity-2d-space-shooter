using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float _speed = 4.0f;

    private Player _player;
    private UIManager _ui;

    private Animator _anim;
    private BoxCollider2D _collider;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("Player not found!");
        }

        _ui = GameObject.Find("UICanvas").GetComponent<UIManager>();
        if (_ui == null)
        {
            Debug.Log("UI Manager not found!");
        }

        _collider = GetComponent<BoxCollider2D>();
        if (_collider == null)
        {
            Debug.Log(name + "'s Box Collider component not found!");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.Log(name + "'s Animator component not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        float topBound = 7.0f;
        float bottomBound = -6.5f;
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

    internal void TakeDamage()
    {
        if (_collider.enabled)
        {
            _collider.enabled = false;
            _anim.SetTrigger("Death");
            _speed = _speed / 2.0f;
            Destroy(this.gameObject, _anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage();
                TakeDamage();
            }
        }
        else if (other.tag == "Laser")
        {
            Laser laser = other.gameObject.GetComponent<Laser>();
            if (laser != null)
            {
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                TakeDamage();
            }
        }
    }
}
