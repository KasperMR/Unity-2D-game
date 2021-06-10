using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private GameObject _plasmaBeam;
    [SerializeField]
    private GameObject _laserBeam;
    [SerializeField]
    private int _volleySize = 10;
    [SerializeField]
    private float _speed = 2;
    private GameObject newLaser;
    [SerializeField]
    private GameObject _explosion;

    [SerializeField]
    private int _bossHealth = 3;
    [SerializeField]
    private float _attackCooldown;
    private bool alive = true;

    [SerializeField]
    private int numberOfExplosionsOnDeath=10;

    private GameObject _player;

    //thin plasma beam that appears instantly, shimmers in size slight, is harmless when thin
    //after ~0.5s expands massively and does damage while large

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        transform.Rotate(new Vector3(0, 180, 0));
        //move down to the center of the screen
        StartCoroutine(MoveToCenterScreen());

    }

    private void ShotGun()
    {
        for (int i = 0; i < _volleySize; i++)
        {
            newLaser = Instantiate(_laserBeam, transform.position, Quaternion.identity);
            newLaser.transform.Rotate(0, 0, i - _volleySize/2);
            newLaser.transform.Translate(transform.up * -0.1f);
        }
    }

    private IEnumerator BossBehaviour()
    {
        while (alive)
        {
            float RNG = Random.value;
            if (RNG > 0.5f)
            {
                if (_player)
                {
                    _plasmaBeam.GetComponent<EnemyPlasma>().PlasmaBeamStart(Vector3.Angle(transform.position, _player.transform.position));
                }
                
            }
            else
            {
                ShotGun();
            }
            yield return new WaitForSeconds(_attackCooldown);
        }
    }

    private IEnumerator MoveToCenterScreen()
    {
        while (Vector3.Distance(transform.position, Vector3.up*4) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.up * 4, _speed*Time.deltaTime);
            yield return null;
        }
        StartCoroutine(BossBehaviour());
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            _bossHealth--;
            Destroy(collision.gameObject);
            Debug.Log("boss hit");
            GameObject newExplosion = Instantiate(_explosion, collision.transform.position, Quaternion.identity);
            newExplosion.transform.localScale = Vector3.one * 0.2f;
            if (_bossHealth <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    private IEnumerator Die()
    {
        alive = false;
        //spawn a bunch of small explosions
        for (int i = 0; i < numberOfExplosionsOnDeath; i++)
        {
            Vector3 explosionDisplacement = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), 0);
            GameObject deathExplosion = Instantiate(_explosion, transform.position + explosionDisplacement, Quaternion.identity);
            deathExplosion.transform.localScale = Vector3.one * 0.2f;
            yield return new WaitForSeconds(0.5f);
        }
        Instantiate(_explosion, transform.position, Quaternion.identity);

        Debug.Log("YOU WON");
        Destroy(gameObject);
    }





}
