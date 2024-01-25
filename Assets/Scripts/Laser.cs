using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _lspeed = 8f;
    private bool _isEnemyLaser = false;
    private bool _isSmartEnemyLaser = false;

    private Dictionary<LaserType, Vector3> movementMap = new Dictionary<LaserType, Vector3>();



    private LaserType _laserType;
    private bool _isPlayerLaser = false;

    private void Awake()
    {
        InitializeMovementMap();
    }

    private void InitializeMovementMap()
    {
        movementMap.Add(LaserType.PlayerUp, Vector3.up);
        movementMap.Add(LaserType.EnemyDown, Vector3.down);
        movementMap.Add(LaserType.EnemyUp, Vector3.up);
    }


    void Update()
    {
        AssignLaser();
        CalculateMovement();
    }



    public enum LaserType
    {
        PlayerUp,
        EnemyDown,
        EnemyUp
    }
    public void AssignLaserType(LaserType type)
    {
        _laserType = type;
    }

    public void AssignLaser()
    {
        switch (_laserType)
        {
            case LaserType.PlayerUp:
                _isPlayerLaser = true;
                break;

            case LaserType.EnemyDown:
                _isEnemyLaser = true;
                _isPlayerLaser = false;
                break;

            case LaserType.EnemyUp:
                _isSmartEnemyLaser = true;
                _isPlayerLaser = false;
                break;

            default:
                Debug.Log("AssignLaser");
                break;
        }
    }



    void CalculateMovement()
    {
        Vector3 direction = (_laserType == LaserType.PlayerUp || _laserType == LaserType.EnemyUp) ? Vector3.up : Vector3.down;

        transform.Translate(direction * _lspeed * Time.deltaTime);

        if ((_laserType == LaserType.PlayerUp || _laserType == LaserType.EnemyUp) && transform.position.y > 10)
        {
            DestroyLaser();
        }
        else if (_laserType == LaserType.EnemyDown && transform.position.y < -10)
        {
            DestroyLaser();
        }
    }







    public void SetPlayerLaser()
    {
        _isPlayerLaser = true;
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
        Player player = other.GetComponent<Player>();
        Enemy enemy = other.GetComponent<Enemy>();

        switch (other.gameObject.tag)
        {
            case "Player":
                HandlePlayerCollision(player);
                break;

            case "Enemy":
                HandleEnemyCollision(enemy);
                break;

            default:
                Debug.Log("OnTrigger Enemy");
                break;
        }
    }


    private void HandlePlayerCollision(Player player)
    {
        if (player != null && !_isPlayerLaser)
        {
            player.Damage();
        }
        DestroyLaser();
    }


    private void HandleEnemyCollision(Enemy enemy)
    {
        Collider enemyCollider = enemy.GetComponent<Collider>();
        Collider thisCollider = GetComponent<Collider>();

        if (enemy != null && ((_isPlayerLaser && !_isSmartEnemyLaser) || (_isSmartEnemyLaser && !_isPlayerLaser)))
        {
            enemy.TakeDamage();
        }
        else if (enemyCollider != null && thisCollider != null && !_isPlayerLaser)
        {
            Physics.IgnoreCollision(thisCollider, enemyCollider);
        }

        DestroyLaser();
    }

    private void DestroyLaser()
    {
        Destroy(gameObject);

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
