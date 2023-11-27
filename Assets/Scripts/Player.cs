using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 6.5f;
    private float speedMultiplier = 2;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private GameObject trippleShotPrefab;
    private float fireRate = 0.1f;
    private float nextFire = -0.3f;
    [SerializeField]
    private int lives = 3;
    private int score;
    private SpawnMan spawnManager;
    [SerializeField]
    private UI_Manager UI_Manager;

    [SerializeField]
    private AudioClip _explosion;
    [SerializeField]
    private AudioClip _laserClip;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioSource _audioSourceE;

    private bool isTrippleShotActive = false;
    private bool isSpeedBoostActive = false;
    private bool isShieldBoostActive = false;
   

    [SerializeField]
    private GameObject trippleShotPowerUp;
    [SerializeField]
    private GameObject speedPowerUp;
    [SerializeField]
    private GameObject shieldBoostPowerUp;
    [SerializeField]
    private GameObject shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnMan>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        FireLaser();
    }
    void CalculateMovement()
    {
        float horizotalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizotalInput, verticalInput, 0);

        if (isSpeedBoostActive)
        {
            transform.Translate(direction * speed * speedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;


            if (isTrippleShotActive)
            {
                Instantiate(trippleShotPrefab, transform.position, Quaternion.identity);
                _audioSource.Play();
            }
            else
            {
                Instantiate(laserPrefab, transform.position + Vector3.up * 0.8f, Quaternion.identity);
                _audioSource.Play();
            }
        }


    }

    public void Damage()
    {
        if (isShieldBoostActive == true) // Check if shield boost is not active
        {
            isShieldBoostActive = false;
            shieldVisualizer.SetActive(false);
            return;
        }

        lives--;

        if (lives <= 2)
        {
            _rightEngine.SetActive(true);
        }
        else
        {
            _rightEngine.SetActive(false);
        }

        if (lives == 1)
        {
            _leftEngine.SetActive(true);
        }
        else
        {
            _leftEngine.SetActive(false);
        }

        UI_Manager.UpdateLives(lives);

            if (lives < 1)
            {
                _audioSourceE.Play();
                spawnManager.PlayerDeath();
                Destroy(gameObject);

                
            }
        }
    

    public void TripleShotActive()
    {
        isTrippleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        isTrippleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        isSpeedBoostActive = true;
        speed *= speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    private IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        isSpeedBoostActive = false;
        speed /= speedMultiplier;
    }

    public void ShieldBoostActive()
    {
        isShieldBoostActive = true;
        shieldVisualizer.SetActive(true);
    }
}
 