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

    private int _enemyLives = 1;


    void Start()
    {
        EnemyShields();
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

    private IEnumerator EnemyShields() //working on the annimator rn
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            int enemyShieldChance = Random.Range(1, 101);

            if (enemyShieldChance <= 11)
            {
                _isEnemyShieldsActive = true;
                _enemyLives++;
                _anim.SetTrigger("OnEnemyShieldsActive");

            }

            yield return new WaitForSeconds(3f);

        }
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();


            if (player != null)
            {
                if (!_isEnemyShieldsActive)
                {

                    _isEnemyShieldsActive = false;
                    _enemyLives--;
                    _anim.SetTrigger("EnemyShieldDown");

    
                    player.Damage();
                }

                if (_enemyLives < 1)
                {
                    EnemyEnd();
                }
            }

            if (other.CompareTag("Laser"))
            {
                if (!_isEnemyShieldsActive)
                {

                    _isEnemyShieldsActive = false;
                    _enemyLives--;
                    _anim.SetTrigger("EnemyShieldDown");


                    Destroy(other.gameObject);
                }

                if (_enemyLives < 1)
                {
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



    private void FireLaser()
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
