using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvanced : MonoBehaviour
{
    public bool isSpawnFromLeft;
    public int rightOrLeft;
    [SerializeField]
    private float _horizontalSpeed = 3.0f; // Speed for horizontal movement
    [SerializeField] 
    private float _verticalSpeed = 5.0f; // Speed for vertical movement
    [SerializeField] 
    private float _fireRateMultiplier = 2.0f; // Fire rate multiplier

    private float _fireRate = 4.0f;
    private float _canFire = -1f;
    [SerializeField]
    private GameObject _disruptorPrefab;
    [SerializeField]
    private Player _player;

    private AudioSource _audioSource;
    private UI_Manager _uiManager;
    private Animator _anim;
    public byte scoreValue = 40;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _anim = GetComponent<Animator>();
    }


    private void Update()
    {
        CalculateMovement();
        EnemyFire();
    }





    public IEnumerator CalculateMovement()
    {
        while (true) // Continuously move the enemy
        {
            if (transform.position.y == -1f)
            {
                transform.Translate(Vector3.right * _horizontalSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * _horizontalSpeed * Time.deltaTime);
            }

            if (transform.position.x == _player.transform.position.x || transform.position.y == 0f)
            {
                transform.Translate(Vector3.down * _verticalSpeed * Time.deltaTime);
                yield return new WaitForSeconds(1.0f);
            }

            yield return null; // Yielding null allows the coroutine to iterate in the next frame
        }
    }

    private void EnemyFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f) * _fireRateMultiplier;
            _canFire = Time.time + _fireRate;

            GameObject disruptor = Instantiate(_disruptorPrefab, transform.position, Quaternion.identity);
            // Adjust the fire rate as needed for this specific enemy
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
        _verticalSpeed = 0;

        if (_uiManager != null)
        {
            _uiManager.OnEnemyDestroyed(scoreValue);
        }

        Destroy(GetComponent<Collider2D>());
        _audioSource.Play();
        Destroy(this.gameObject, 1.0f);
    }
}
