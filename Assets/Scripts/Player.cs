using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;

    [SerializeField]
    private float _cooldown = 1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private SpawnManager _spawnManager;
    //=============================power ups
    [SerializeField]
    private bool _tripleLaserActive;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private float _speedBoostSpeed = 8.5f;
    [SerializeField]
    private bool _speedBoosted = false;

    //=============================shield
    private int _shieldLives = 0;
    [SerializeField]
    private bool _shielded = false;

    [SerializeField]
    private GameObject _shieldVisual;

    //=============UI

    [SerializeField]
    private UIManager uIManager;

    //==================damage visualization

    [SerializeField]
    private GameObject[] _burningEngines;

    //=============================sounds

    [SerializeField]
    private AudioSource _laserSound;
    [SerializeField]
    private AudioSource _explosionSound;
    [SerializeField]
    private AudioSource _powerUpSound;

    private bool _lShiftDown = false;
    [SerializeField]
    private float _lShiftSpeedMult = 1.2f;
    private float _defaultSpeedMult = 1;
    [SerializeField]
    private float _currentSpeedMult = 1;

    //=============cooldown IEnumerators

    private IEnumerator speedBoostCooldown;
    private IEnumerator tripleShotCooldown;

    //===========laser

    [SerializeField]
    private GameObject _laserPrefab;
    private float _canFire = 0f;
    private int _shotsRemaining = 15;
    [SerializeField]
    private GameObject _laserCounter;

    // Start is called before the first frame update
    void Start()
    {
        //reset player start position to 0,0,0
        gameObject.transform.position = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _lShiftDown = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _lShiftDown == false)
        {
            _lShiftDown = true;
            _currentSpeedMult = _lShiftSpeedMult;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && _lShiftDown == true)
        {
            _lShiftDown = false;
            _currentSpeedMult = _defaultSpeedMult;
        }


        Movement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _shotsRemaining > 0)
        {
            _canFire = Time.time + _cooldown;
            FireLaser();
        }
        else if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _shotsRemaining <= 0)
        {
            _laserCounter.GetComponent<LaserCounter>().OutOfAmmoAnimControl();
        }
    }

    private void Movement()
    {

        if (_speedBoosted == false)
        {
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * _speed * _currentSpeedMult);

            transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * _speed * _currentSpeedMult);
        }
        else
        {
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * _speedBoostSpeed * _currentSpeedMult);

            transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * _speedBoostSpeed * _currentSpeedMult);
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
        _shotsRemaining--;
        _laserCounter.GetComponent<LaserCounter>().UpdateAmmoCount(_shotsRemaining);
    }
    public void TakeDamage()
    {
        if (_shielded)
        {
            _shieldLives--;
            if (_shieldLives <= 0)
            {
                _shielded = false;
                _shieldVisual.SetActive(false);
            }
            else
            {
                if (_shieldLives == 2)
                {
                    _shieldVisual.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 255);
                }
                if (_shieldLives == 1)
                {
                    _shieldVisual.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
                }
                 
            }      
            
            return;

        }

        _lives += -1;
        if (_lives - 1 >= 0)
        {
            if (_burningEngines[_lives - 1] != null)
            {
                _burningEngines[_lives - 1].SetActive(true);
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
        _shieldLives = 3;
        _shieldVisual.SetActive(true);
        _powerUpSound.PlayOneShot(_powerUpSound.clip);
    }

}
