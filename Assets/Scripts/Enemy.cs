using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;
    public int scoreValue = 10;
    private UI_Manager _uiManager;
    private Player _player;
    private Animator _anim;

    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    

    private int _shieldStateParameter;
    private float _lives = 1f;
    private float _shields = 2f;
   
    


    void Start()
    {
        

        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _shieldStateParameter = Animator.StringToHash("_shieldState");
        
        

        if (_player == null)
        {
            Debug.LogError("player is null");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("Animator is null");
        }
        StartCoroutine(EnemyShields());

    }
    

    void Update()
    {
        CalculateMovement();
        EnemyFire();
        EnemyLifeUpdate();
    }

    void DefaultEnemyBehavior()
    {

    }

    void FiringEnemyBehavior()
    {

    }

    void ZigZagEnemyBehavior()
    {

    }
    void LeftToRightEnemyBehavior()
    {

    }
    void RightToLeftEnemyBehavior()
    {

    }
    void ShieldedEnemy()
    {

    }
    void AggressiveEnemyBehavior()
    {

    }
    void SmartEnemyBehavior()
    {

    }


    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        {
            if (transform.position.y < -5f)
            {
                float randomX = Random.Range(-8f, 8f);
                transform.position = new Vector3(randomX, 7, 0);
            }
        }
    }
    private void EnemyLifeUpdate()
    {
        if (_lives >= 2)
        {
            _anim.SetBool(_shieldStateParameter, true);
        }
        else if (_lives <= 0f)
        {
            EnemyEnd();
        }
        else
        {
            _anim.SetBool(_shieldStateParameter, false);
        }

    }

    private IEnumerator EnemyShields() //working on the annimator rn
    {
        int enemyShieldTimer = Random.Range(1, 4);
        int enemyShieldChance = Random.Range(1, 101);

        if (enemyShieldChance <= 30)
        {
            _lives++;    
        }
        
        yield return new WaitForSeconds(enemyShieldTimer * Time.deltaTime);
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                TakeDamage();
                player.Damage();
            }
        }
        else if (other.CompareTag("Laser"))
        {
            Laser laser = other.GetComponent<Laser>();

            if (laser != null)
            {
                TakeDamage();
                Destroy(other.gameObject);
            }
        }
    }

   
    



    private void TakeDamage()
    {
        _lives--;
    }




    private void EnemyEnd()
    {
        _anim.SetTrigger("_onDestroy");
        _enemySpeed = 0;
        

        if (_uiManager != null)
        {
            _uiManager.OnEnemyDestroyed(scoreValue);
        }
        

        Destroy(GetComponent<Collider2D>());
        _fireRate -= 3f;
        
        _audioSource.Play();
        Destroy(this.gameObject, 2.0f);
    }




    



    private void EnemyFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }
}
