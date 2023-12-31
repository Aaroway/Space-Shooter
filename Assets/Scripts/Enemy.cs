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
    }

    void Update()
    {
        CalculateMovement();

        EnemyFire();

        StartCoroutine(EnemyShields());

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
            _isEnemyShieldsActive = true;
            _anim.SetBool("IsShieldActive", true);
        }
        else
        {
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
                    player.Damage();
                }
                else if (_isEnemyShieldsActive == false)
                {
                    player.Damage();
                    EnemyEnd();
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

    private void EnemyEnd()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _enemySpeed = 0;

        if (_uiManager != null)
        {
            _uiManager.OnEnemyDestroyed(scoreValue);
        }

        Destroy(GetComponent<Collider2D>());
        _audioSource.Play();
        Destroy(this.gameObject, 1f);
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
