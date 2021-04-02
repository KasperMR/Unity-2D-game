using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private float _cooldown = 1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _tripleLaserActive;
    [SerializeField]
    private float _speedBoostSpeed = 8.5f;
    [SerializeField]
    private bool _speedBoosted = false;
    [SerializeField]
    private bool _shielded = false;

    [SerializeField]
    private GameObject _shieldVisual;

    [SerializeField]
    private UIManager uIManager;

    [SerializeField]
    private GameObject[] _burningEngines;

    [SerializeField]
    private AudioSource _laserSound;
    [SerializeField]
    private AudioSource _explosionSound;
    [SerializeField]
    private AudioSource _powerUpSound;



    private IEnumerator speedBoostCooldown;
    private IEnumerator tripleShotCooldown;

    private float _canFire = 0f;
    // Start is called before the first frame update
    void Start()
    {
        //reset player start position to 0,0,0
        gameObject.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _cooldown;
            FireLaser();
        }
    }

    private void Movement()
    {

        if (_speedBoosted == false)
        {
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * _speed);

            transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * _speed);
        }
        else
        {
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * _speedBoostSpeed);

            transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * _speedBoostSpeed);
        }


        //stop the player going below the screen
        //if position y < -3.5
        //position y = -3.5
        var pos = transform.position;
        if (transform.position.y < -3.5f)
        {
            pos.y = -3.5f;
            transform.position = pos;
        }
        else if (transform.position.y > 0)
        {
            pos.y = 0;
            transform.position = pos;
        }

        //wrap the player if they go too far left or right
        if (transform.position.x > 10.5f || transform.position.x < -10.5f)
        {
            pos.x = pos.x * -1;
            transform.position = pos;
        }
    }

    private void FireLaser()
    {
        if (_tripleLaserActive)
        {
            var newLaser = Instantiate(_tripleLaserPrefab);
            newLaser.transform.position = transform.position + Vector3.up * 0.8f;
        }
        else
        {
            var newLaser = Instantiate(_laserPrefab);
            newLaser.transform.position = transform.position + Vector3.up * 0.8f;
        }
        _laserSound.PlayOneShot(_laserSound.clip);
    }
    public void TakeDamage()
    {
        if (_shielded)
        {
            _shielded = false;
            _shieldVisual.SetActive(false);
            return;

        }

        _lives += -1;
        if (_lives - 1 >= 0)
        {
            if (_burningEngines[_lives-1] != null)
            {
                _burningEngines[_lives-1].SetActive(true);
            }        
        }

        uIManager.UpdateLives(_lives);
        if (_lives <= 0)            
        {            
            Debug.Log("player dead");
            _spawnManager.StopSpawning();
            _explosionSound.Play();
            Destroy(gameObject);
        }
    }

    IEnumerator TripleShotCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleLaserActive = false;
    }

    IEnumerator SpeedBoostCooldown()
    {
        Debug.Log("star count at: " + Time.time);
        yield return new WaitForSeconds(5.0f);
        Debug.Log("end count at : " + Time.time);
        _speedBoosted = false;
    }


    public void ActivateSpeedBoost()
    {
        _speedBoosted = true;
        if (speedBoostCooldown != null)
        {
            StopCoroutine(speedBoostCooldown);
        }
        speedBoostCooldown = SpeedBoostCooldown();
        StartCoroutine(speedBoostCooldown);
        _powerUpSound.PlayOneShot(_powerUpSound.clip);
    }
    public void ActivateTripleShot()
    {
        _tripleLaserActive = true;
        if (tripleShotCooldown != null)
        {
            StopCoroutine(tripleShotCooldown);
        }
        tripleShotCooldown = TripleShotCooldown();
        StartCoroutine(tripleShotCooldown);
        _powerUpSound.PlayOneShot(_powerUpSound.clip);
    }

    public void ActivateShield()
    {
        _shielded = true;
        _shieldVisual.SetActive(true);
        _powerUpSound.PlayOneShot(_powerUpSound.clip);
    }

}
