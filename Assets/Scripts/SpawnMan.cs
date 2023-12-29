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
    [SerializeField]
    public GameObject advancedEnemyLeft;
    [SerializeField]
    public GameObject advancedEnemyRight;
    public Vector3 advancedPositionLeft = new Vector3(-8, 5.5f, 0); // Define advanced positions
    public Vector3 advancedPositionRight = new Vector3(8, 5.5f, 0);
    private EnemyAdvanced _enemyAdvanced;


    private void Awake()
    {
        _instance = this;
    }




    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }



    public IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(5.0f);




        while (!_isDead)
        {

            int randomChance = Random.Range(1, 101);
            GameObject enemyToSpawn = null;

            switch (randomChance)
            {
                case int n when (n <= 30): // 30% chance for Enemy2
                    Vector3 spawnPosition = new Vector3(Random.Range(-11f, 11f), 7f, 0f);
                    enemyToSpawn = Instantiate(_enemyPrefab2, spawnPosition, Quaternion.identity);
                    break;

                case int n when (n <= 80): // 50% chance for Enemy
                    Vector3 basicPosition = new Vector3(Random.Range(-11f, 11f), 7f, 0f);
                    enemyToSpawn = Instantiate(_enemyPrefab, basicPosition, Quaternion.identity);
                    break;

                default: // 20% chance for EnemyAdvanced
                    int rightOrLeft = Random.Range(1, 3);
                    GameObject advancedEnemyPrefab = null;
                    Vector3 spawnPosition = Vector3.zero; 



                    if (rightOrLeft == 1 || rightOrLeft == 2)
                    {
                        advancedEnemyPrefab = _enemyPrefab2;
                        spawnPosition = rightOrLeft == 1 ? advancedPositionLeft : advancedPositionRight;
                    }
                    else if (rightOrLeft == 2)
                    {
                        advancedEnemyPrefab = _enemyPrefab2;
                        spawnPosition = advancedPositionRight;
                    }
                    else
                    {
                        spawnPosition = Vector3.zero;
                    }

                    if (advancedEnemyPrefab != null)
                    {
                        if (spawnPosition != Vector3.zero)
                        {
                            enemyToSpawn = Instantiate(advancedEnemyPrefab, spawnPosition, Quaternion.identity);
                        }
                        else
                        {
                            Debug.LogError("Spawn position is not assigned");
                        }
                    }
                    if (rightOrLeft == 1)
                    {
                        spawnPosition = advancedPositionLeft;
                    }
                    else if (rightOrLeft == 2)
                    {
                        spawnPosition = advancedPositionRight;
                    }

                    if (advancedEnemyPrefab != null)
                    {
                        if (spawnPosition != Vector3.zero) // Check if spawnPosition is assigned
                        {
                            enemyToSpawn = Instantiate(advancedEnemyPrefab, spawnPosition, Quaternion.identity);
                        }
                        else
                        {
                            Debug.LogError("Spawn position not assigned for advanced enemy.");
                        }
                    }


                    break;
            }
            yield return new WaitForSeconds(currentSpawnDelay);
        }
    }









    private IEnumerator SpawnPowerUpRoutine()
    {
        int powerUpsToSpawn = 50;
        while (!_isDead)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
            int randomChance = Random.Range(1, 101);

            if (randomChance <= 10)
            {
                Instantiate(_powerUps[5], spawnPosition, Quaternion.identity);
            }
            else if (randomChance >= 11 && randomChance <= 30)
            {
                Instantiate(_powerUps[6], spawnPosition, Quaternion.identity);
            }
            else
            {
                int randomPowerUps = Random.Range(0, 4);
                Instantiate(_powerUps[randomPowerUps], spawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(Random.Range(3, 8)); // Delay between power-up spawns
            powerUpsToSpawn--;
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


    public void PlayerDeath()
    {
        _isDead = true;
    }
}
