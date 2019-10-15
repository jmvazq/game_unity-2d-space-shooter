using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType { TripleShot, SpeedBoost, Shield, Heal }

public class Powerup : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private PowerupType _type;
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
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // TODO: add powerup to a queue if another is currently active or interrupt the current one
                switch(_type)
                {
                    case PowerupType.TripleShot:
                        player.ActivateTripleShot();
                        break;
                    case PowerupType.SpeedBoost:
                        player.ActivateSpeedBoost();
                        break;
                    case PowerupType.Shield:
                        player.ActivateShield();
                        break;
                    case PowerupType.Heal:
                        // TODO
                        break;
                    default:
                        Debug.Log("Unspecified powerup type!");
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
