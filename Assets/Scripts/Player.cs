﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 4.5f;
    private float speedMultiplier = 2;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private GameObject trippleShotPrefab;
    private float fireRate = 0.5f;
    private float nextFire = -1f;
    [SerializeField]
    private int lives = 3;
    private SpawnMan spawnManager;

    private bool isTrippleShotActive = false;
    private bool isSpeedBoostActive = false;
    [SerializeField]
    private GameObject trippleShotPowerUp;
    [SerializeField]
    private GameObject speedPowerUp;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnMan>();

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
            speed = 8.5f;
            transform.Translate(direction * speed * speedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -11.3f, 11.3f),
            Mathf.Clamp(transform.position.y, -3.8f, 0),
            0
        );
    }

    private void FireLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            if (isTrippleShotActive)
            {
                Instantiate(trippleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(laserPrefab, transform.position + Vector3.up * 0.8f, Quaternion.identity);
            }
        }
    }

    public void Damage()
    {
        lives--;

        if (lives < 1)
        {
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
}
 