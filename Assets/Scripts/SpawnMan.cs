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
    private GameObject[] _enemyPrefabs;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject _enemyContainer;


    

    private bool _isDead = false;

    
    private float currentSpawnDelay = 5.0f;
    private int[] scoreThresholds = {150, 300, 500, 800};
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



    public IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(currentSpawnDelay);

        while (!_isDead)
        {

            int randomEnemy = Random.Range(1, 101);
            GameObject enemyToSpawn = null;

           

            switch (randomEnemy)
            {
                case int n when (n <= 20):
                    Vector3 spawnPosition = new Vector3(Random.Range(-11f, 11f), 7f, 0f);
                    enemyToSpawn = Instantiate(_enemyPrefabs[0], spawnPosition, Quaternion.identity);
                    break;
                case int n when (n > 20 && n <= 40):
                    Vector3 spawnPosition2 = new Vector3(Random.Range(-11f, 11f), 7f, 0f);
                    enemyToSpawn = Instantiate(_enemyPrefabs[1], spawnPosition2, Quaternion.identity);
                    break;

                case int n when (n > 40 && n <= 50):
                    Vector3 spawnPosition3 = new Vector3(-11f, 3f, 0f);
                    enemyToSpawn = Instantiate(_enemyPrefabs[2], spawnPosition3, Quaternion.identity);
                    break;
                case int n when (n > 50 && n <= 60):
                    Vector3 spawnPosition4 = new Vector3(-11f, 3f, 0f);
                    enemyToSpawn = Instantiate(_enemyPrefabs[3], spawnPosition4, Quaternion.identity);
                    break;
                case int n when (n > 60 && n <= 101):
                    Vector3 spawnPosition5 = new Vector3(-11f, 3f, 0f);
                    enemyToSpawn = Instantiate(_enemyPrefabs[4], spawnPosition5, Quaternion.identity);
                    break;
                default:
                    Vector3 spawnPosition6 = new Vector3(11f, 3f, 0f);
                    enemyToSpawn = Instantiate(_enemyPrefabs[1], spawnPosition6, Quaternion.identity);
                    break;
            }
            yield return new WaitForSeconds(currentSpawnDelay);
        }
    }









    private IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(currentSpawnDelay);
        while (!_isDead)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
            int randomChance = Random.Range(1, 101);

            if (randomChance <= 10)
            {
                Instantiate(_powerUps[3], spawnPosition, Quaternion.identity);
            }
            else if (randomChance >= 11 && randomChance <= 30)
            {
                
                Instantiate(_powerUps[5], spawnPosition, Quaternion.identity);
            }
            else if (randomChance > 30 && randomChance <= 60)
            {
                Instantiate(_powerUps[4], spawnPosition, Quaternion.identity);
            }
            else if (randomChance > 60 && randomChance <= 85)
            {
                int randomPowerUps = Random.Range(0, 2);
                Instantiate(_powerUps[randomPowerUps], spawnPosition, Quaternion.identity);
            }
            else 
            {    
                Instantiate(_powerUps[6], spawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(currentSpawnDelay);
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
