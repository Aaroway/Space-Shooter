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
        MoveEnemy();
        CheckBounds();

        
        uiManager = GameObject.FindObjectOfType<UI_Manager>(); // Find the UIManager in the scene
        

    }

    void MoveEnemy()
    {
        transform.Translate(Vector3.down * enemySpeed * Time.deltaTime);
    }

    void CheckBounds()
    {
        if (transform.position.y < -5.5f)
        {
            RespawnEnemy();
        }
    }

    void RespawnEnemy()
    {
        transform.position = new Vector3(Random.Range(-11f, 11f), 7.8f, 0f);
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
            audioSource.Play();
            Destroy(this.gameObject, 1.0f);

        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            _anim.SetTrigger("OnEnemyDeath");
            enemySpeed = 0f;
            Destroy(this.gameObject, 2.4f);

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
