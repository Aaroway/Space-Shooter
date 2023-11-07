using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMan : MonoBehaviour
{
    [SerializeField]
    private GameObject _EnemyPrefab;
    [SerializeField]
    private GameObject _EnemyContainer;
    [SerializeField]
    private bool _dead = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnRoutine()
    {
        while (_dead == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_EnemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _EnemyContainer.transform;

            yield return new WaitForSeconds(5.0f);
        }
    }
    public void playerDeath()
    {
        _dead = true;
    }
}
