using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private float _spawnCooldown = 1;
    [HideInInspector]
    public float thisIsHiddenInInspector;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _spawning = true;
    private int _waveSize = 5;
    [SerializeField]
    private float _timeBetweenWaves = 5;

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
        _spawning = false;
    }

    private IEnumerator SpawnEnemy()
    {
        while (_spawning)
        {
            GameObject[] waveArray = new GameObject[_waveSize];
            GenerateNewWave(_waveSize, waveArray);
            int i = 0;
            while (i < _waveSize)
            {            
                GameObject newEnemy = Instantiate(waveArray[i], new Vector3(Random.Range(-9f, 9f), 7.25f, 0), Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                i++;
                yield return new WaitForSeconds(_spawnCooldown);
            }
            _waveSize = (_waveSize + (int)((float)_waveSize*1.5));
            yield return new WaitForSeconds(_timeBetweenWaves);
        }
    } 

    private IEnumerator SpawnPowerUp()
    {
        while (_spawning)
        {
            int randomPowerUp = Random.Range(0, _powerUpPrefab.Length);
            GameObject newPowerUp;
            if (randomPowerUp == 4)
            {
                if (Random.Range(0f,9f) > 8)
                {
                    newPowerUp = Instantiate(_powerUpPrefab[randomPowerUp], new Vector3(Random.Range(-9f, 9f), 7.25f, 0), Quaternion.identity);
                }
                else
                {
                    yield break;
                }
            }
            else
            {
                newPowerUp = Instantiate(_powerUpPrefab[randomPowerUp], new Vector3(Random.Range(-9f, 9f), 7.25f, 0), Quaternion.identity);
            }
            newPowerUp.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    private void GenerateNewWave(int waveSize, GameObject[] waveArray)
    {
        for (int i = 0; i < waveSize; i++)
        {
            waveArray[i] = _enemy;
        }
    }
}
