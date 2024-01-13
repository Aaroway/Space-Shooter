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
    private SmartLaser _smartLaser;
    private int _enemySpeed = 6;


    void Start()
    {

        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
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

    void CalculateMovement() //movement works. laser needs to go up or down
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        {
            if (transform.position.y < -6f)
            {
                float randomX = Random.Range(-7.5f, 7.5f);
                transform.position = new Vector3(randomX, 7, 0);
            }
        }
    }




    void EnemyFire()
    {
        if (_player != null && Time.deltaTime > _canFire)
        {
            _fireRate = Random.Range(3f, 5f);
            _canFire = Time.time + _fireRate;

            if (transform.position.y < _player.transform.position.y)
            {
                _smartLaser.MoveUp();
            }
            else
            {
                _smartLaser.MoveDown();
            }
        }
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.Damage();
            EnemyEnd();
        }
        else if (other.TryGetComponent(out SmartLaser smartLaser))
        {
            Destroy(other.gameObject);
            EnemyEnd();
        }
        else if (other.TryGetComponent(out SmartEnemy smartEnemy))
        {
            Collider thisCollider = GetComponent<Collider>();
            Collider otherCollider = other.GetComponent<Collider>();
            Physics.IgnoreCollision(thisCollider, otherCollider);
        }
        else if (other.TryGetComponent(out Enemy enemyType2))
        {
            Collider thisCollider = GetComponent<Collider>();
            Collider otherCollider = other.GetComponent<Collider>();
            Physics.IgnoreCollision(thisCollider, otherCollider);
        }
        else if (other.TryGetComponent(out Laser laserType1))
        {
            Collider thisCollider = GetComponent<Collider>();
            Collider otherCollider = other.GetComponent<Collider>();
            Physics.IgnoreCollision(thisCollider, otherCollider);
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
