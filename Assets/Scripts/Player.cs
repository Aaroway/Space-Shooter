using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    public float _speed = 6.5f;
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
    private float _thrusterDrainRate = 2f;
    private float _thrusterRechargeRate = 1f;
    public float maxEnergy = 10.0f;
    private bool _isShiftPressed = false;
    public float currentEnergy;
    private float _boostMultiplier = 1.4f;

    public int ammunition = 15;
    public bool isAmmoDepleted = false;
    private int _maxAmmunition = 15;
    private bool _isOverloadActive = false;

    [SerializeField]
    private bool _sensorDamage = false;








    void Start()
    {
        UI_Manager.Instance.AddScore(score);
        UI_Manager.Instance.InitializeThrusterSlider();
        UI_Manager.Instance.InitializeShieldSlider();
        transform.position = Vector3.zero;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnMan>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {      
        CalculateMovement();
        FireLaser();
        ManageThruster();
    }


    void CalculateMovement()
    {
        float horizotalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizotalInput, verticalInput, 0);
        MovementState currentState = GetMovementState();

        switch (currentState)
        {
            case MovementState.BothActive:
                transform.Translate(direction * _speed * (_speedMultiplier + _boostMultiplier) * Time.deltaTime);
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

        if (Input.GetKeyDown(KeyCode.Space) && (Time.time > _nextFire) && !isAmmoDepleted)
        {
            if (_isOverloadActive == true)
            {
                _nextFire = Time.time;
                ammunition = _maxAmmunition;
                _uiManager.UpdateAmmoCount(ammunition);
            }

            if (_isOverloadActive == false)
            {
                _nextFire = Time.time + _fireRate;
                ammunition--;
                _uiManager.UpdateAmmoCount(ammunition);

                if (ammunition <= 0)
                {
                    isAmmoDepleted = true;
                    ammunition = 0;
                    return;
                }
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


    public void ReplinishAmmunition()
    {
        ammunition = _maxAmmunition;
        UI_Manager.Instance.UpdateAmmoCount(ammunition);
    }


    public void LifeCollected()
    {
        if (_lives < 3)
        {
            _lives++;
            UI_Manager.Instance.UpdateLives(_lives);
            UpdateShieldSlider();
        }
    }

    public void Overload()
    {
        _isOverloadActive = true;
        StartCoroutine(OverloadPowerDown());
    }

    private IEnumerator OverloadPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _isOverloadActive = false;
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

        CameraShake.Instance.StartShaking(); //no singleton necessary here.

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


    private void ManageThruster()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isShiftPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isShiftPressed = false;
        }

        if (_isShiftPressed)
        {
            if (currentEnergy > 0)
            {
                DrainEnergy(_thrusterDrainRate);
                UI_Manager.Instance.UpdateThrusterSlider(currentEnergy);
            }
            else
            {
                Debug.Log("Energy Drained!");
            }
        }
        else
        {
            RechargeEnergyRate(_thrusterRechargeRate);
            UI_Manager.Instance.UpdateThrusterSlider(currentEnergy);
        }
    }

    public void DrainEnergy(float drainRatePerSecond)
    {
        float amount = drainRatePerSecond * Time.deltaTime;
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
    }

    public void RechargeEnergyRate(float rechargeRatePerSecond)
    {
        float amount = rechargeRatePerSecond * Time.deltaTime;
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
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

    public void NegativeEffect()
    {
        int effectIndex = Random.Range(1, 4); // Randomly choose an effect index (1 - 3)

        switch (effectIndex)
        {
            case 1:
                LostControl();
                break;
            case 2:
                SensorDamage();
                break;
            case 3:
                PowerDrain();
                break;
            default:
                Debug.LogError("Negative effect null");
                break;
        }
    }

    public void LostControl()
    {
        CameraShake.Instance.StartShaking();
        StartCoroutine(LostControlRoutine());
    }

    private IEnumerator LostControlRoutine()
    {
        float timer = 5f;
        while (timer > 0f)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            transform.Translate(randomDirection * _speed * Time.deltaTime);

            timer -= Time.deltaTime;
            yield return null;
        }
    }


    public bool GetSensorDamage()
    {
        return _sensorDamage;
    }

    public void SensorDamage()
    {
        _sensorDamage = true;
        StartCoroutine(SensorDamageRoutine());
    }

    private IEnumerator SensorDamageRoutine()
    {
        yield return new WaitForSeconds(5f);
        _sensorDamage = false;
    }

    public void PowerDrain()
    {
        ammunition /= 2;
        UI_Manager.Instance.UpdateAmmoCount(ammunition);


        if (_isShieldBoostActive)
        {
            _isShieldBoostActive = false;
            _shieldVisualizer.SetActive(false);
        }
    }
}
