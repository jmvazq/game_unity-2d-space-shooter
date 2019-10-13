using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // Destroy when it goes off-screen
        if (transform.position.y > 6.87f)
        {
            Destroy(this.gameObject);
        }
    }
}
