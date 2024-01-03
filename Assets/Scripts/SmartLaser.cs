using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartLaser : MonoBehaviour
{

    public float _speed = 5f;
    [SerializeField]
    private GameObject _laserOptionsDown;
    [SerializeField]
    private GameObject _laserOptionsLeft;
    [SerializeField]
    private GameObject _laserOptionsRight;

    void Update()
    {

    }

    public void MoveDown()
    {
        GameObject laserDown = Instantiate(_laserOptionsDown, transform.position, Quaternion.identity);
        laserDown.transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    public void MoveLeftUp()
    {
        GameObject laserLeft = Instantiate(_laserOptionsLeft, transform.position, Quaternion.Euler(0f, 0f, -150f));

        Rigidbody2D rb = laserLeft.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Calculate the direction based on the rotation angle
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * -150f), Mathf.Sin(Mathf.Deg2Rad * -150f));

            // Apply velocity to the laser in the desired direction
            rb.velocity = direction * _speed * Time.deltaTime;
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on the laser prefab!");
        }
    }
    public void MoveRighUp()
    {
        GameObject laser = Instantiate(_laserOptionsRight, transform.position, Quaternion.Euler(0f, 0f, 150f));

        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * 150f), Mathf.Sin(Mathf.Deg2Rad * 150f));

            rb.velocity = direction * _speed * Time.deltaTime;
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on the laser prefab!");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
                Destroy(transform.parent.gameObject);
            }
        }

        else if (other.tag == "Enemy")
        {
            Collider thisCollider = GetComponent<Collider>();
            Collider otherCollider = other.GetComponent<Collider>();
            Physics.IgnoreCollision(thisCollider, otherCollider);
        }
    }
}
