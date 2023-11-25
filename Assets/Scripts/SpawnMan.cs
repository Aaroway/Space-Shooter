using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMan : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject[] powerups;
    [SerializeField] private GameObject enemyContainer;

    private bool isDead = false;

    private void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!isDead)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;

            yield return new WaitForSeconds(5.0f);
        }
    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!isDead)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
            int randomPowerUp = Random.Range(0, powerups.Length);
            Instantiate(powerups[randomPowerUp], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void PlayerDeath()
    {
        isDead = true;
    }
}
