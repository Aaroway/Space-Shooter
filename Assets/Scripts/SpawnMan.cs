using System.Collections;
using UnityEngine;

public class SpawnMan : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _isDead = false;






    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_isDead)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5.0f);
        }
    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_isDead)
        {
            int randomPowerUp = Random.Range(0, _powerUps.Length);

            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
            int randomChance = Random.Range(1, 101);


            if (randomChance <= 10)
            {
                Instantiate(_powerUps[5], spawnPosition, Quaternion.identity);
            }
            else
            {
                int randomPowerUps = Random.Range(0, 4);
                Instantiate(_powerUps[randomPowerUps], spawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(Random.Range(3, 8));


        }
    }

    public void PlayerDeath()
    {
        _isDead = true;
    }
}
