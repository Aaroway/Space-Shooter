using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    private UI_Manager _uiManager;
    
    private Player _player;
    
    private AudioSource _audioSource;
    public int scoreValue = 50;
    private Animator _anim;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    public bool fireLeft = false;
    public bool fireRight = false;
    [SerializeField]
    private SmartLaser _smartLaser;
    private int _enemySpeed = 6;

    // Start is called before the first frame update
    void Start()
    {

        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>(); // Adjust this line
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _smartLaser = GetComponent<SmartLaser>();
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
            if (transform.position.y < -6f)
            {
                float randomX = Random.Range(-7.5f, 7.5f);
                transform.position = new Vector3(randomX, 7, 0);
            }
        }
        if (transform.position.y < _player.transform.position.y && transform.position.x > _player.transform.position.x)
        {
            FireRight();
            return;
        }
        else if (transform.position.y < _player.transform.position.y && transform.position.x > _player.transform.position.x)
        {
            FireLeft();
            return;
        }
        else
        {
            Debug.LogError("Fire Behind");
        }
    }

    public void FireRight()
    {
        if (_smartLaser != null)
        {
            _smartLaser.MoveRighUp();
        }
        else
        {
            Debug.LogError("SmartLaser component not found!");
        }
    }

    public void FireLeft()
    {
        if (_smartLaser != null)
        {
            _smartLaser.MoveLeftUp();
        }
        else
        {
            Debug.LogError("SmartLaser component not found!");
        }
    }

    void EnemyFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 5f);
            _canFire = Time.time + _fireRate;
            _smartLaser.MoveDown();
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
}
