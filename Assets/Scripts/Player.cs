using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _trippleShotPrefab;
    private float _fireRate = 0.1f;
    private float _nextFire = -0.3f;
    [SerializeField]
    private int _lives = 3;
    public int score;
    private SpawnMan _spawnManager;
    [SerializeField]
    private UI_Manager _uiManager;
    private Laser _laser;

    [SerializeField]
    private AudioClip _laserClip;
    private AudioSource _audioSource;


    private bool _isTrippleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldBoostActive = false;


    [SerializeField]
    private GameObject _trippleShotPowerUp;
    [SerializeField]
    private GameObject _speedPowerUp;
    [SerializeField]
    private GameObject _shieldBoostPowerUp;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
    private bool _isThrusterActive = false;
    private float _boostMultiplier = 1.4f;

    public int _ammunition = 15;
    public bool _isAmmoDepleted = false;


    private int _maxAmmunition = 15;

    private bool _isMegaLaserActive = false;
    [SerializeField]
    private GameObject _megaLaser;
    private GameObject _instantiatedMegaLaser;
    public float _distanceInFront = 5.9f;








    void Start()
    {
        
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        transform.position = Vector3.zero;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnMan>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CalculateMovement();

        if (!_isAmmoDepleted)
            FireLaser();

        ThrusterActive();
        
    }


    void CalculateMovement()
    {
        if (_isMegaLaserActive && _instantiatedMegaLaser != null)
        {
            _instantiatedMegaLaser.transform.position = transform.position;
        }

        float horizotalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizotalInput, verticalInput, 0);
        MovementState currentState = GetMovementState();

        switch (currentState)
        {
            case MovementState.BothActive:
                transform.Translate(direction * _speed * _speedMultiplier * _boostMultiplier * Time.deltaTime);
                break;
            case MovementState.ThrusterActive:
                transform.Translate(direction * _speed * _boostMultiplier * Time.deltaTime);
                break;
            case MovementState.SpeedBoostActive:
                transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
                break;
            case MovementState.Normal:
                transform.Translate(direction * _speed * Time.deltaTime);
                break;
        }


        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
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


    private void FireLaser()
    {

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && !_isAmmoDepleted && !_isMegaLaserActive)
        {
            _nextFire = Time.time + _fireRate;

            _ammunition--;
            _uiManager.UpdateAmmoCount(_ammunition);

            if (_ammunition <= 0)
            {
                _isAmmoDepleted = true;
                _ammunition = 0;
                return;
            }

            

            if (_isTrippleShotActive)
            {
                Instantiate(_trippleShotPrefab, transform.position, Quaternion.identity);
                _audioSource.Play();
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + Vector3.up * 0.8f, Quaternion.identity);
                _audioSource.Play();
            }
        }
    }

    public void ActivateMegaLaser()
    {
        if (!_isMegaLaserActive) //Im working on this part
        {
            _isMegaLaserActive = true;
            Vector3 laserOffset = transform.position + transform.forward * _distanceInFront;

            _instantiatedMegaLaser = Instantiate(_megaLaser, laserOffset, Quaternion.identity);
            StartCoroutine(DeactivateMegaLaser());
        }
    }

    private IEnumerator DeactivateMegaLaser()
    {
        yield return new WaitForSeconds(5f);

        if (_instantiatedMegaLaser != null)
        {
            Destroy(_instantiatedMegaLaser);
        }
        _isMegaLaserActive = false;
    }


    public void ReplinishAmmunition()
    {
        _ammunition = _maxAmmunition;
        _uiManager.UpdateAmmoCount(_ammunition);
    }


    public void LifeCollected()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
            UpdateShieldSlider();
        }
    }



    public void Damage()
    {
        if (_isShieldBoostActive == true)
        {
            _isShieldBoostActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if (_lives <= 2)
        {
            _rightEngine.SetActive(true);
        }
        else
        {
            _rightEngine.SetActive(false);
        }

        if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }
        else
        {
            _leftEngine.SetActive(false);
        }

        _uiManager.UpdateLives(_lives);
        UpdateShieldSlider();


        if (_lives < 1)
        {
            _spawnManager.PlayerDeath();
            Destroy(gameObject);
        }
    }


    public void TripleShotActive()
    {
        _isTrippleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTrippleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    private IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }


    public void ShieldBoostActive()
    {
        _isShieldBoostActive = true;
        _shieldVisualizer.SetActive(true);
    }

    private void ThrusterActive()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || (Input.GetKeyDown(KeyCode.RightShift)))
        {
            _isThrusterActive = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || (Input.GetKeyUp(KeyCode.RightShift)))
        {
            _isThrusterActive = false;
        }
    }
    private enum MovementState
    {
        Normal,
        ThrusterActive,
        SpeedBoostActive,
        BothActive
    }

    MovementState GetMovementState()
    {
        if (_isThrusterActive && _isSpeedBoostActive)
            return MovementState.BothActive;
        else if (_isThrusterActive)
            return MovementState.ThrusterActive;
        else if (_isSpeedBoostActive)
            return MovementState.SpeedBoostActive;
        else
            return MovementState.Normal;
    }

    private void UpdateShieldSlider()
    {
        float shieldPercentage = (float)_lives / 3f;
        _uiManager.UpdateShieldSlider(shieldPercentage);
    }
}
