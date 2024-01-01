using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 2.5f;
    private float _sideMovementSpeed = 2.0f;
    public int scoreValue = 20;
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
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        EnemyFire();
    }
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);


        float randomX = Mathf.Sin(Time.time) * _sideMovementSpeed;
        transform.Translate(Vector3.right * randomX * Time.deltaTime);

        // Wrap around the screen horizontally if needed
        if (transform.position.x < -13f || transform.position.x > 13f)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
        }
    
        

        // Wrap around the screen vertically if it goes below a certain point
        if (transform.position.y < -5f)
        {
            float randomAngle = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomAngle, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            EnemyEnd();
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            EnemyEnd();
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
        Destroy(this.gameObject, 1.0f);
    }

    void OnDestroy()  //what happens when player death
    {
        if (_uiManager != null)
        {
            _uiManager.AddScore(scoreValue);
        }
    }

    

    private void EnemyFire()
    {
        if (Time.time > _canFire) //method
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


