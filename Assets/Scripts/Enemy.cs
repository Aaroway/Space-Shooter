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
    private bool _isEnemyShieldsActive = false;
    [SerializeField]
    private float _destroyThreshold = 1f;
    [SerializeField]
    private float _shieldThreshold = .6f;

    [SerializeField]
    private float _defaultState = .1f;
    public float _lives;


    void Start()
    {

        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

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

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        {
            if (transform.position.y < -5f)
            {
                float randomX = Random.Range(-8f, 8f);
                transform.position = new Vector3(randomX, 7, 0);
                if (!_isEnemyShieldsActive)
                {
                    _isEnemyShieldsActive = false;
                    _anim.SetBool("IsShieldActive", false);
                }
            }
        }
    }



    private IEnumerator EnemyShields() //working on the annimator rn
    {
        int enemyShieldChance = Random.Range(1, 101);

        if (enemyShieldChance <= 30)
        {
            _lives = 2f;
            _isEnemyShieldsActive = true;
        }
        else
        {
            _lives = 1f;
            _isEnemyShieldsActive = false;
        }
        yield return null;
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                if (_isEnemyShieldsActive == true)
                {
                    _anim.SetBool("IsShieldActive", false);
                    TakeDamage();
                    player.Damage();
                }
                else if (_isEnemyShieldsActive == false)
                {
                    TakeDamage();
                    player.Damage();
                }
            }
        }
        else if (other.CompareTag("Laser"))
        {
            Laser laser = other.GetComponent<Laser>();

            if (laser != null)
            {
                if (_isEnemyShieldsActive == true)
                {
                    TakeDamage();
                    _anim.SetBool("IsShieldActive", false);
                    Destroy(other.gameObject);
                }
                else if (_isEnemyShieldsActive == false)
                {
                    Destroy(other.gameObject);
                    EnemyEnd();
                }
            }

        }
    }

    private enum EnemyState
    {
        Default,
        Shields,
        Destroyed
    }
    private EnemyState currentState = EnemyState.Default;

    private void EnemyLifeUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Default:
                if (_lives == 1)
                {
                    _anim.SetFloat("Shields", _defaultState);
                    _isEnemyShieldsActive = false;
                }
                break;
            case EnemyState.Shields:
                if (_lives == 2)
                {
                    _anim.SetFloat("Shields", _shieldThreshold);
                    _isEnemyShieldsActive = true;
                }
                break;
            case EnemyState.Destroyed:
                if (_lives == 0)
                    _anim.SetFloat("Destroy", _destroyThreshold);
                _isEnemyShieldsActive = false;
                break;
            default:
                _anim.SetFloat("Default", _defaultState);
                _isEnemyShieldsActive = false;
                break;

        }
    }



    private void TakeDamage()
    {
        _lives--;

        if (_lives <= 0)
        {
            EnemyEnd();
        }
    }

    
    

    private void EnemyEnd()
    {
        _enemySpeed = 0;

        if (_uiManager != null)
        {
            _uiManager.OnEnemyDestroyed(scoreValue);
        }

        Destroy(this.gameObject.GetComponent<Collider2D>());
        _fireRate -= 3f;
        _audioSource.Play();   
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
