using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvanced : MonoBehaviour
{
    public bool isSpawnFromLeft;
    public int rightOrLeft;
    [SerializeField] private float _enemySpeed = 5.0f; // Adjust the speed as needed
    [SerializeField] private float _horizontalSpeed = 5.0f; // Speed for horizontal movement
    [SerializeField] private float _verticalSpeed = 5.0f; // Speed for vertical movement
    [SerializeField] private float _fireRateMultiplier = 2.0f; // Fire rate multiplier

    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    [SerializeField] private GameObject _disruptorPrefab;

    private void Start()
    {
        SpawnPosition();
    }

    public void SpawnPosition()
    {
        rightOrLeft = Random.Range(1, 3);

        if (rightOrLeft == 1)
        {
            isSpawnFromLeft = true;
            transform.position = new Vector3(-13f, transform.position.y, 0f);
        }
        else
        {
            isSpawnFromLeft = false;
            transform.position = new Vector3(13f, transform.position.y, 0f);
        }
    }

    private void Update()
    {
        CalculateMovement();
        EnemyFire();
    }

    private void CalculateMovement()
    {
        float direction = isSpawnFromLeft ? 1.0f : -1.0f;
        Vector3 movement = Vector3.right * direction * _horizontalSpeed * Time.deltaTime;

        transform.Translate(movement);

        // Change movement direction if spawned from the left
        if (isSpawnFromLeft && transform.position.x >= 13f)
        {
            isSpawnFromLeft = false;
        }
        // Change movement direction if spawned from the right
        else if (!isSpawnFromLeft && transform.position.x <= -13f)
        {
            isSpawnFromLeft = true;
        }

        // Move vertically once at player's Y position
        if (transform.position.y <= 4f)
        {
            Vector3 verticalMovement = Vector3.down * _verticalSpeed * Time.deltaTime;
            transform.Translate(verticalMovement);
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
}
