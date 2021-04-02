using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 60;
    [SerializeField]
    private GameObject _explosionPrefab;
    private GameObject _ownExplosion;
    [SerializeField]
    private SpawnManager spawnManager;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _rotationSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            _ownExplosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            StartCoroutine(CleanUpExplosionRoutine());
            spawnManager.StartSpawning();
        }
    }

    private IEnumerator CleanUpExplosionRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2.40f);
        Destroy(_ownExplosion);
        Destroy(gameObject);
    }
}
