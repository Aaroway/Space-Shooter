using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _lspeed = 8f;
    private bool _isEnemyLaser = false;
    private bool _isSmartEnemyLaser = false;
    


    
    private LaserType _laserType;
    private bool _isPlayerLaser = false;

    public void AssignLaserType(LaserType type)
    {
        _laserType = type;
    }




    void Update()
    {
        AssignLaser();
        CalculateMovement();
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

    //laser logic: 1 decide who's laser it is and assign
    // 2 once decided, now movement, so each case will call moveup or down.
    // 3 still decided, handle collision

    void CalculateMovement()
    {
        Vector3 direction = (_laserType == LaserType.PlayerUp || _laserType == LaserType.EnemyUp) ? Vector3.up : Vector3.down;
        transform.Translate(direction * _lspeed * Time.deltaTime);

        if ((_laserType == LaserType.PlayerUp || _laserType == LaserType.EnemyUp) && transform.position.y > 10)
        {
            Destroy(gameObject);
        }
        else if (_laserType == LaserType.EnemyDown && transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }






    public void SetPlayerLaser(bool isPlayerLaser)
    {
        _isPlayerLaser = isPlayerLaser;
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
                if (player != null && !_isPlayerLaser)
                {
                    player.Damage();
                }
                Destroy(gameObject); // Destroy the spawned laser
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                break;

            case "Enemy":
                Collider enemyCollider = other.GetComponent<Collider>();
                Collider thisCollider = GetComponent<Collider>();

                if (enemy != null && _isPlayerLaser && enemyCollider != null && thisCollider != null)
                {
                    enemy.TakeDamage();
                }
                else if (enemyCollider != null && thisCollider != null && !_isPlayerLaser)
                {
                    Physics.IgnoreCollision(thisCollider, enemyCollider);
                }

                Destroy(gameObject); // Destroy the spawned laser
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                break;

            default:
                Debug.Log("OnTrigger Enemy");
                break;
        }
    }












    public enum LaserType
    {
        PlayerUp,
        EnemyDown,
        EnemyUp
    }
}
