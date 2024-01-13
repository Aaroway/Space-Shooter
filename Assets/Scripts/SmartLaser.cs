using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartLaser : MonoBehaviour
{

    public float _speed = 5f;
    [SerializeField]
    private GameObject _laserDown;
    [SerializeField]
    private GameObject _laserUp;


    void Update()
    {

    }

    public void MoveDown()
    {
        GameObject laserDown = Instantiate(_laserDown, transform.position, Quaternion.identity);
        laserDown.transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    public void MoveUp()
    {
        GameObject laserUp = Instantiate(_laserUp, transform.position, Quaternion.identity);
        laserUp.transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 10)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.Damage();
            Destroy(gameObject);
            
        }
        else if (other.TryGetComponent(out Enemy enemy))
        {
            Collider thisCollider = GetComponent<Collider>();
            Collider otherCollider = other.GetComponent<Collider>();
            Physics.IgnoreCollision(thisCollider, otherCollider);
        }
        else if (other.TryGetComponent(out SmartEnemy smartEnemy))
        {
            Collider thisCollider = GetComponent<Collider>();
            Collider otherCollider = other.GetComponent<Collider>();
            Physics.IgnoreCollision(thisCollider, otherCollider);
        }
    }
}
