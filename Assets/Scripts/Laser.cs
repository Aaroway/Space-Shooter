using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _lspeed = 8f;
    private bool _isEnemyLaser;
    private bool _isSmartEnemyLaser;




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
            Destroy(this.gameObject);
        }
    }
   
    

   

    public void AssignEnemyLaserDown()
    {
        _isEnemyLaser = true;
    }

    public void AssignEnemyLaserUp()
    {
        _isSmartEnemyLaser = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            if (!_isEnemyLaser)
            {
                enemy.TakeDamage();
                Destroy(gameObject);
            }
            else
            {
                Collider thisCollider = GetComponent<Collider>();
                Collider otherCollider = other.GetComponent<Collider>();
                Physics.IgnoreCollision(thisCollider, otherCollider);
            }
        }
        else if (other.TryGetComponent(out Player player))
        {
            player.Damage();
            Destroy(gameObject);
            Destroy(transform.parent.gameObject);
        }
    }

    void IsEnemyLaser()
    {
        if (_isEnemyLaser == false && _isSmartEnemyLaser == false)
        {
            MoveUp();
        }
        else if (_isEnemyLaser == true && _isSmartEnemyLaser == false)
        {
            MoveDown();
        }
        else if (_isSmartEnemyLaser == true)
        {
            MoveUp();
        }
    }
}
