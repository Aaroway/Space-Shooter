using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvancedLeft : MonoBehaviour
{

   
    private float _speed = 3.0f;
    [SerializeField] 
    private float _fireRateMultiplier = 3f; // Fire rate multiplier

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





    public void CalculateMovement()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);

        if (transform.position.x > 13f)
        {
            transform.position = new Vector3(-13f, 4.3f, 0);
        }
    }
        




    private void EnemyFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 5f) * _fireRateMultiplier;
            _canFire = Time.time + _fireRate;

            GameObject disruptor = Instantiate(_disruptorPrefab, transform.position, Quaternion.identity);
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
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            EnemyEnd();
        }
    }
    private void EnemyEnd()
    {
        _anim.SetTrigger("OnEnemyAdvDestroy");
        _speed = 0;
        _fireRate = 0;

        if (_uiManager != null)
        {
            _uiManager.OnEnemyDestroyed(scoreValue);
        }

        Destroy(GetComponent<Collider2D>());
        _audioSource.Play();
        Destroy(this.gameObject, 1f);
    }
}
