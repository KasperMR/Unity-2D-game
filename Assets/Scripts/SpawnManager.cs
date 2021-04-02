using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private float _spawnCooldown = 5;
    [HideInInspector]
    public float thisIsHiddenInInspector;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool spawning = true;

    [SerializeField]
    private GameObject[] _powerUpPrefab;
    [SerializeField]
    private GameObject _powerUpContainer;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartSpawning()
    {
        IEnumerator spawn = SpawnEnemy();
        StartCoroutine(spawn);
        spawn = SpawnPowerUp();
        StartCoroutine(spawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopSpawning() 
    {
        spawning = false;
    }

    private IEnumerator SpawnEnemy()
    {        
        while (spawning)
        {            
            GameObject newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-9f, 9f), 7.25f, 0),Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnCooldown);
        }        
    } 

    private IEnumerator SpawnPowerUp()
    {
        while (spawning)
        {
            int randomPowerUp = Random.Range(0, _powerUpPrefab.Length);
            GameObject newPowerUp = Instantiate(_powerUpPrefab[randomPowerUp], new Vector3(Random.Range(-9f, 9f), 7.25f, 0), Quaternion.identity);
            newPowerUp.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }
}
