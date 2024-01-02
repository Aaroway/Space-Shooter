using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartLaser : MonoBehaviour
{
    
    public float _speed = 5f;
    [SerializeField]
    private GameObject[] _laserOptions;// Adjust the speed of the laser

    void Update()
    {

    }

    public void MoveDown()
    {
        GameObject laser = Instantiate(_laserOptions[0], transform.position, Quaternion.identity);
        laser.transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    public void MoveLeftUp()
    {
        // Instantiate the laser prefab
        GameObject laser = Instantiate(_laserOptions[1], transform.position, Quaternion.Euler(0f, 0f, -150f));

        // Access the rigidbody of the laser (assuming it has one)
        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();

        // Set the velocity of the laser based on the rotation angle
        if (rb != null)
        {
            // Calculate the direction based on the rotation angle
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * -150f), Mathf.Sin(Mathf.Deg2Rad * -150f));

            // Apply velocity to the laser in the desired direction
            rb.velocity = direction * _speed;
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on the laser prefab!");
        }
    }
    public void MoveRighUp()
    {
        // Instantiate the laser prefab
        GameObject laser = Instantiate(_laserOptions[0], transform.position, Quaternion.Euler(0f, 0f, 150f));

        // Access the rigidbody of the laser (assuming it has one)
        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();

        // Set the velocity of the laser based on the rotation angle
        if (rb != null)
        {
            // Calculate the direction based on the rotation angle
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * 150f), Mathf.Sin(Mathf.Deg2Rad * 150f));

            // Apply velocity to the laser in the desired direction
            rb.velocity = direction * _speed;
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on the laser prefab!");
        }
    }
}
