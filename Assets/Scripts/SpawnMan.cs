using System.Collections;
using UnityEngine;

public class SpawnMan : MonoBehaviour
{
    private static SpawnMan _instance;
    public static SpawnMan Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("UI Manager is null");
            }
            return _instance;
        }
    }
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _enemyPrefab2;

    private bool _isDead = false;

    
    private float currentSpawnDelay = 5.0f;
    private int[] scoreThresholds = {100, 200, 300};
    private float[] spawnDelays = {5.0f, 4.0f, 3.0f, 2.0f};


    private void Awake()
    {
        _instance = this;
    }


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {

        yield return new WaitForSeconds(5.0f);

        while (!_isDead)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
            GameObject newEnemy;

            int randomChance = Random.Range(1, 101);

            if (randomChance <= 20)
            {
                newEnemy = Instantiate(_enemyPrefab2, spawnPosition, Quaternion.identity);
            }
            else
            {
                newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);

            }

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(currentSpawnDelay);
        }

    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_isDead)
        {
            int randomPowerUp = Random.Range(0, _powerUps.Length); //didn't need this one.

            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
            int randomChance = Random.Range(1, 101);


            if (randomChance <= 10)
            {
                Instantiate(_powerUps[5], spawnPosition, Quaternion.identity);
            }
            else if (randomChance>= 11 && randomChance <=30)
            {
                Instantiate(_powerUps[6], spawnPosition, Quaternion.identity);
            }
            else
            {
                int randomPowerUps = Random.Range(0, 4);
                Instantiate(_powerUps[randomPowerUps], spawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }


    public void CheckScoreThreshold(int playerScore)
    {
        for (int i = 0; i < scoreThresholds.Length - 1; i++)
        {
            if (playerScore >= scoreThresholds[i] && playerScore < scoreThresholds[i + 1])
            {
                currentSpawnDelay = spawnDelays[i];
                break;
            }
        }

        if (playerScore >= scoreThresholds[scoreThresholds.Length -1])
        {
            currentSpawnDelay = spawnDelays[spawnDelays.Length - 1];
        }

    }
    // a switch statement that will set spawn difficulty
    //a method to check player score and set a switch statement
    //spawn wave 1 - 100 score
    //spawn wave 2 - 250 score
    //spawn wave 3 - 400 score

    public void PlayerDeath()
    {
        _isDead = true;
    }
}
