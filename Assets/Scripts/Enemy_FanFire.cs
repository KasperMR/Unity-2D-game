using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FanFire : Enemy
{
    [SerializeField]
    private int _volleySize = 60;
    private float _volleyDuration = 3f;
    private float _MinimumVolleyInterval = 6f;
    private GameObject newLaser;

    // Start is called before the first frame update
    new void Start()
    {
        transform.position = new Vector3(transform.position.x, 4.5f, transform.position.z);
        StartCoroutine(FanFireLaserRoutine());
        StartCoroutine(LeftAndRightRoutine());        
    }

    // Update is called once per frame
    new void Update()
    {
        return;
    }

    private IEnumerator FanFireLaserRoutine()
    {
        while (_alive)
        {
            for (int i = 0; i < _volleySize; i++)
            {
                newLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
                newLaser.transform.Rotate(0, 0, i - 30);
                newLaser.transform.Translate(transform.up*-0.1f);
                yield return new WaitForSeconds(_volleyDuration/_volleySize);
            }
            yield return new WaitForSeconds(_MinimumVolleyInterval+Random.Range(0f, 5f));
        }        
    }

    private IEnumerator LeftAndRightRoutine()
    {
        float direction = 1;
        while (_alive)
        {
            transform.Translate(Vector3.right * direction * _speed * Time.deltaTime);
            if (transform.position.x > 8 || transform.position.x < -8)
            {
                direction = direction * -1;
            }
            yield return null;
        }
    }
}
