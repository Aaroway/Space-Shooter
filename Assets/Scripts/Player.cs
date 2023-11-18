using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _speed = 4.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _trippleShotPrefab;
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnMan _spawnManager;
    [SerializeField]
    private bool _isTrippleShotActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private GameObject _trippleShotPowerUp;
    [SerializeField]
    private GameObject _speedPowerUp;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnMan>();

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        FireLaser();

    }
    void CalculateMovement()
    {
        if(_isSpeedBoostActive == true)
        {
            _speed = 8.5f;
        }
        {
            float horizotalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizotalInput, verticalInput, 0);

            if (_isSpeedBoostActive == false)
            {
                transform.Translate(direction * _speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
            }
            if (transform.position.y >= 0)
            {
                transform.position = new Vector3(transform.position.x, 0, 0);
            }
            else if (transform.position.y < -3.8f)
            {
                transform.position = new Vector3(transform.position.x, -3.8f, 0);
            }

            if (transform.position.x > 11.3f)
            {
                transform.position = new Vector3(-11.3f, transform.position.y, 0);
            }
            else if (transform.position.x < -11.3f)
            {
                transform.position = new Vector3(11.3f, transform.position.y, 0);
            }
        }

    }

    void FireLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            {
                if (_isTrippleShotActive == true)
                {
                    Instantiate(_trippleShotPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                }
            }
        }
    }
    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.PlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TrippleShotActive()
    {
        _isTrippleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTrippleShotActive = false;
    }
    
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }
    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
}
