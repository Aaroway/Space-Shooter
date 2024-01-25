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
    [SerializeField]
    private GameObject _laserBack;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private bool _isEnemySmart = false;
    

    private int _shieldStateParameter;
    private float _lives = 1f;

    public enum EnemyState
    {
        EnemyNormal,//enemy goes down. no special. one life. 
        EnemyFiring,//enemy goes down. fires at random. one life.
        EnemyShielded,//enemy goes down. may or may not fire. two lives.
        EnemySmart//enemy moves down. may or may not fire. does fire up. one life.
    }

    public EnemyState currentState;

    void Start()
    {
        currentState = EnemyState.EnemyNormal;

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
        

    }

    void Update()
    {
        
        CalculateMovement();
        EnemyFire();
        EnemyLifeUpdate();
        CheckPlayerPosition();
    }

    void GetState()
    {
        float randomStateValue = Random.value * 100;

        switch (currentState)
        {
            case EnemyState.EnemyNormal:
                if (randomStateValue <=40)
                {
                    currentState = EnemyState.EnemyFiring;
                }
                break;
            case EnemyState.EnemyFiring:
                if (randomStateValue <= 60)
                {
                    currentState = EnemyState.EnemyShielded;
                }
                break;
            case EnemyState.EnemyShielded://if random.range <= 30%
                {
                    StartCoroutine(EnemyShields());
                }
                break;
            case EnemyState.EnemySmart://if enemy normal or firing && player transform >= enemy transform
                if (randomValue <= 100 && _player.transform.position.x >= transform.position.x)
                {
                    currentState = EnemyState.EnemySmart;
                }
                break;
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        {
            if (transform.position.y < -7f)
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

    private IEnumerator EnemyShields() 
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
        switch (other.gameObject.tag)
        {
            case "Player":
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    TakeDamage();
                    player.Damage();
                }
                break;

            case "Laser":
                Laser laser = other.GetComponent<Laser>();
                if (laser != null)
                {
                    TakeDamage();
                    Destroy(other.gameObject);
                }
                break;

            default:
                Debug.Log("OnTrigger Enemy");
                break;
        }
    }






    public void TakeDamage()
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

        if (_audioSource != null)
        {
            _audioSource.Play();
        }

        Destroy(this.gameObject, 2.0f);
    }





    void CheckPlayerPosition()
    {
        Laser laser = GetComponent<Laser>();
        if (_player != null && transform.position.x <= _player.transform.position.x)
        {
            laser.AssignLaserType(Laser.LaserType.EnemyUp);
        }
        else
        {
            Debug.LogError("CheckPlayerPosition");
        }
    }




    private void EnemyFire()
    {
        if (Time.time > _canFire && transform.position.y > _player.transform.position.y)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaserDown();
            }
        }
        else if (Time.time > _canFire && transform.position.y <= _player.transform.position.y)
        {
            _fireRate = Random.Range(1f, 4f);
            _canFire = Time.time + _fireRate;
            GameObject laserBack = Instantiate(_laserBack, transform.position + Vector3.up * 0.8f, Quaternion.identity);
            Laser[] lasers= laserBack.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaserUp();
            }
        }
    }
}
