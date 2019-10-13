using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
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

    internal void TakeDamage()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        Destroy(this.gameObject, 0.25f);
    }

    private void OnTriggerEnter(Collider other)
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
                laser.Destroy();
                TakeDamage();
            }
        }
    }
}
