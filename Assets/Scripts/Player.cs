using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _trippleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
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
    private GameObject _speedBoostPowerUp;


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
            _speed = 20;
        }
        {
            float horizotalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            transform.Translate(Vector3.right * horizotalInput * _speed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

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
    
    public void SpeedPowerUpActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }
    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }
}
