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
    private float _fireRate = 0.5f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private SpawnMan _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnMan>();

        if (_spawnManager == null)
        {
            Debug.LogError("This Shit is Broken");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);

        }
    }
    void CalculateMovement()
    {
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

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.PlayerDeath();
            Destroy(this.gameObject);
        }
    }
}