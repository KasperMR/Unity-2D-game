using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _fanFireEnemy;
    [SerializeField]
    private GameObject _shieldedEnemy;
    [SerializeField]
    private GameObject _rearGunnerEnemy;
    [SerializeField]
    private GameObject _dodgerEnemy;
    [SerializeField]
    private GameObject _boss;

    [SerializeField]
    private float _spawnCooldown = 1;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _spawning = true;
    private int _waveSize = 5;
    [SerializeField]
    private float _timeBetweenWaves = 5;
    [SerializeField]
    private int _bossWave = 10;

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
        //spawn = SpawnPowerUp();
        //StartCoroutine(spawn);
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
            int i = 0;
            if (_waveSize < _bossWave)
            { 
                GenerateNewWave(_waveSize, waveArray);
            }
            else if (_waveSize > _bossWave)
            {
                _waveSize = 1;
                waveArray = new GameObject[_waveSize];
                waveArray[0] = _boss;
                _timeBetweenWaves = 9999;
            }
            
            while (i < waveArray.Length)
            {            
                GameObject newEnemy = Instantiate(waveArray[i], new Vector3(Random.Range(-9f, 9f), 7.25f, 0), Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                i++;
                yield return new WaitForSeconds(_spawnCooldown);
            }
            _waveSize = ((int)((float)_waveSize*1.5));
            yield return new WaitForSeconds(_timeBetweenWaves);
        }
    } 

    //private IEnumerator SpawnPowerUp()
    //{
    //    while (_spawning)
    //    {
    //        int randomPowerUp = Random.Range(0, _powerUpPrefab.Length);
    //        GameObject newPowerUp;
    //        if (randomPowerUp == 4)
    //        {
    //            if (Random.Range(0f,9f) > 8)
    //            {
    //                newPowerUp = Instantiate(_powerUpPrefab[randomPowerUp], new Vector3(Random.Range(-9f, 9f), 7.25f, 0), Quaternion.identity);
    //            }
    //            else
    //            {
    //                yield break;
    //            }
    //        }
    //        else
    //        {
    //            newPowerUp = Instantiate(_powerUpPrefab[randomPowerUp], new Vector3(Random.Range(-9f, 9f), 7.25f, 0), Quaternion.identity);
    //        }
    //        newPowerUp.transform.parent = _powerUpContainer.transform;
    //        yield return new WaitForSeconds(Random.Range(3f, 7f));
    //    }
    //}

    private void GenerateNewWave(int waveSize, GameObject[] waveArray)
    {
        for (int i = 0; i < waveSize; i++)
        {
            int rng = Random.Range(0, 110); 
            if (rng < 50)
            {
                waveArray[i] = _enemy;
            }
            else if(rng < 55)
            {
                waveArray[i] = _fanFireEnemy;
            }
            else if(rng < 60)
            {
                waveArray[i] = _powerUpPrefab[0]; //tripleshot powerup
            }
            else if (rng < 65)
            {
                waveArray[i] = _powerUpPrefab[1]; //speed powerup
            }
            else if (rng < 70)
            {
                waveArray[i] = _powerUpPrefab[2]; //shield powerup
            }
            else if (rng < 80)
            {
                waveArray[i] = _powerUpPrefab[3]; //laser ammo pickup
            }
            else if (rng < 85)
            {
                waveArray[i] = _powerUpPrefab[4]; //health pickup
            }
            else if (rng < 90)
            {
                waveArray[i] = _powerUpPrefab[5]; //rocket powerup 
            }
            else if (rng < 95)
            {
                waveArray[i] = _powerUpPrefab[6]; //snail powerdown
            }
            else if (rng < 100)
            {
                waveArray[i] = _shieldedEnemy;
            }
            else if (rng < 105)
            {
                waveArray[i] = _rearGunnerEnemy;
            }
            else if (rng < 110)
            {
                waveArray[i] = _dodgerEnemy;
            }


        }
    }
}
