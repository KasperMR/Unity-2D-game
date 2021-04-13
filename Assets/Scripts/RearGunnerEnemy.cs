using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearGunnerEnemy : Enemy
{
    [SerializeField]
    private GameObject _rearLaser;
    [SerializeField]
    private bool _readyToFire = true;

    
    new private void Update()
    {
        if (transform.position.y < -5.25f)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 7.25f, 0);
        }
        if (_playerTransform != null && _playerTransform.position.y-0.5/*estimated player height*/ > transform.position.y && _readyToFire == true)
        {
            Instantiate(_rearLaser, transform.position, Quaternion.identity);
            _readyToFire = false;
            StartCoroutine(StartCoolCooldown());
        }
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
    }

    private IEnumerator StartCoolCooldown()
    {
        yield return new WaitForSeconds(Random.Range(3f, 7f));
        _readyToFire = true;
    }


}
