using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    
    private bool _isDamaging = false;

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
        Material mat = GetComponent<MeshRenderer>().material;
        Color originalColor = mat.color;

        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mat.color = originalColor;
    }

    IEnumerator TakeDamageCoroutine()
    {
        _isDamaging = true;

        StartCoroutine("DamageFlash");
        yield return new WaitForSeconds(0.25f);

        gameObject.SetActive(false);

        _isDamaging = false;
    }

    internal void TakeDamage()
    {
        if (!_isDamaging)
        {
            StartCoroutine("TakeDamageCoroutine");
        }
    }

    private void OnTriggerEnter(Collider other)
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
                laser.Destroy();
                TakeDamage();
            }
        }
    }
}
