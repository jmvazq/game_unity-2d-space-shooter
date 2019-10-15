using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _dropChance = 1.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // Destroy when it goes off-screen
        if (transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // TODO: collect by player
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.ActivateTripleShot();
            }

            Destroy(this.gameObject);
        }
    }
}
