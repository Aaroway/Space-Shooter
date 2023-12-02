using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemySpeed = 4.0f;
    public int scoreValue = 10; // Score value for destroying this enemy
    private UI_Manager uiManager;
    private Player player;
    private Animator _anim;

    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();

        if (player == null)
        {
            Debug.LogError ("player is null");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError ("Animator is null");
        }
    }

    void Update()
    {
        CalculateMovement();

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

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * enemySpeed * Time.deltaTime);
        {
            if (transform.position.y < -5f)
            {
                float randomX = Random.Range(-8f, 8f);
                transform.position = new Vector3(randomX, 7, 0);
            }
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
            _anim.SetTrigger("OnEnemyDeath");
            enemySpeed = 0;

            Destroy(GetComponent<Collider2D>());
            audioSource.Play();  //play explosion on hit
            Destroy(this.gameObject, 1.0f);

        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            _anim.SetTrigger("OnEnemyDeath");
            enemySpeed = 0f;

            Destroy(GetComponent<Collider2D>());
            audioSource.Play();  //play explosion once hit
            Destroy(this.gameObject, 1.4f);
        }
    }
    void OnDestroy()
    {
        if (uiManager != null)
        {
            uiManager.AddScore(scoreValue); // Add score to the player's score when this enemy is destroyed
        }
    }
}
