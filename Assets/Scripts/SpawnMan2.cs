using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMan2 : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _enemyPrefab2;
    private int _spawnX = int.MinValue(-13f, 13f);

    private bool _isDead = false;


    private float currentSpawnDelay = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        yield return new WaitForSeconds(3.0f);
        StartSpawning();
    }

    // Update is called once per frame


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    public void SpawnEnemyRoutine()
    {
        while (!_isDead)
        {
            Vector3 spawnPosition = new Vector3(-13f, 13f), 4.5f, 0f);
        }
    }

}
