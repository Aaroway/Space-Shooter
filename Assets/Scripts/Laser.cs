using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _lspeed = 8f;
    private bool _isEnemyLaser = false;
   



    void Update()
    {
        IsEnemyLaser();
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _lspeed * Time.deltaTime);

        if (transform.position.y > 10)
        {
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _lspeed * Time.deltaTime);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player) && _isEnemyLaser == true)
        {
            player.Damage();
            Destroy(gameObject);
            Destroy(transform.parent.gameObject);
        }
        else if (other.TryGetComponent(out Enemy enemy) && _isEnemyLaser == true)
        {
            Collider thisCollider = GetComponent<Collider>();
            Collider otherCollider = other.GetComponent<Collider>();
            Physics.IgnoreCollision(thisCollider, otherCollider);
        }
    }

    void IsEnemyLaser()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }
}
