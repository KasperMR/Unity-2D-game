using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


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
    private float _speedBoostSpeedMult = 8.5f;
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
    [SerializeField]
    private AudioSource _powerDownSound;

    //==========================speed

    [SerializeField]
    private float _BaseSpeed = 5;
    private bool _lShiftDown = false;
    [SerializeField]
    private float _lShiftSpeedMult = 1.2f;
    [SerializeField]
    private float _snailSpeedMult = 0.75f;
    private bool _snailMode = false;

    //=============cooldown IEnumerators

    private IEnumerator speedBoostCooldown;
    private IEnumerator tripleShotCooldown;
    private IEnumerator rocketCooldown;
    private IEnumerator snailCooldown;

    //===========laser

    [SerializeField]
    private GameObject _laserPrefab;
    private float _canFire = 0f;
    private int _shotsRemaining = 15;
    private int _maxShots = 30;
    [SerializeField]
    private GameObject _laserCounter;

    //=============Rockets
    [SerializeField]
    private GameObject _rocketPrefab;
    private bool _rocketsEnabled = false;
    [SerializeField]
    private AudioSource _rocketSound;

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
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && _lShiftDown == true)
        {
            _lShiftDown = false;
        }


        Movement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _shotsRemaining > 0)
        {
            _canFire = Time.time + _cooldown;
            if (_rocketsEnabled)
            {
                FireRocket();
            }
            else
            {
                FireLaser();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _shotsRemaining <= 0)
        {
            _laserCounter.GetComponent<LaserCounter>().OutOfAmmoAnimControl();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PullInPowerUps();
        }
    }

    private void Movement()
    {

        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * _BaseSpeed * GetSpeedBoostedSpeed() * GetLShiftSpeed() * GetSnailSpeed());

        transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * _BaseSpeed * GetSpeedBoostedSpeed() * GetLShiftSpeed() * GetSnailSpeed());
        //Debug.Log("total speed: " + _BaseSpeed * GetSpeedBoostedSpeed() * GetLShiftSpeed() * GetSnailSpeed());

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

        //warp the player if they go too far left or right
        if (transform.position.x > 10.5f || transform.position.x < -10.5f)
        {
            pos.x = pos.x * -1;
            transform.position = pos;
        }
    }

    private float GetSpeedBoostedSpeed()
    {
        if (_speedBoosted == true)
        {
            return _speedBoostSpeedMult;
        }
        else
        {
            return 1f;
        }
    }

    private float GetLShiftSpeed()
    {
        if (_lShiftDown == true)
        {
            return _lShiftSpeedMult;
        }
        else
        {
            return 1;
        }
    }

    private float GetSnailSpeed()
    {
        if (_snailMode == true)
        {
            return _snailSpeedMult;
        }
        else
        {
            return 1;
        }
    }

    private void FireLaser()
    {
        if (_tripleLaserActive)
        {
            GameObject newLaser = Instantiate(_tripleLaserPrefab);
            newLaser.transform.position = transform.position + Vector3.up * 0.8f;
        }
        else
        {
            GameObject newLaser = Instantiate(_laserPrefab);
            newLaser.transform.position = transform.position + Vector3.up * 0.8f;
        }
        _laserSound.PlayOneShot(_laserSound.clip);
        _shotsRemaining--;
        _laserCounter.GetComponent<LaserCounter>().UpdateAmmoCount(_shotsRemaining, _maxShots);
    }

    private void FireRocket()
    {
        GameObject newRocket = Instantiate(_rocketPrefab);
        _rocketSound.PlayOneShot(_rocketSound.clip);
        _shotsRemaining--;
        _laserCounter.GetComponent<LaserCounter>().UpdateAmmoCount(_shotsRemaining, _maxShots);
    }

    public void TakeDamage()
    {
        Camera.main.GetComponent<CameraShake>().CameraShakeInit();
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

        UpdateLivesVisual();

        if (_lives <= 0)
        {
            Debug.Log("player dead");
            _spawnManager.StopSpawning();
            _explosionSound.Play();
            Destroy(gameObject);
        }
    }

    private void UpdateLivesVisual()
    {
        foreach (GameObject engine in _burningEngines)
        {
            engine.SetActive(true);
        }
        for (int i = 0; i < _lives-1; i++)
        {
            _burningEngines[i].SetActive(false);
        }
        //if (_lives - 1 >= 0)
        //{
        //    if (_burningEngines[_lives - 1] != null)
        //    {
        //        _burningEngines[_lives - 1].SetActive(true);
        //    }
        //}

        uIManager.UpdateLives(_lives);
    }

    IEnumerator TripleShotCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleLaserActive = false;
    }

    IEnumerator SpeedBoostCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoosted = false;
    }

    IEnumerator RocketCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        _rocketsEnabled = false;
    }

    IEnumerator SnailModeCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        _snailMode = false;
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

    public void GetAmmo()
    {
        _shotsRemaining += 15;
        if (_shotsRemaining > _maxShots)
        {
            _shotsRemaining = _maxShots;
        }
        _laserCounter.GetComponent<LaserCounter>().UpdateAmmoCount(_shotsRemaining, _maxShots);
    }

    public void GetHealth()
    {
        if (_lives < 3)
        {
            _lives++;
            UpdateLivesVisual();
        }
    }

    public void ActivateRockets()
    {
        Debug.Log("Rockets enabled");
        _rocketsEnabled = true;
        if (rocketCooldown != null)
        {
            StopCoroutine(rocketCooldown);
        }
        rocketCooldown = RocketCooldown();
        StartCoroutine(rocketCooldown);
        _powerUpSound.PlayOneShot(_powerUpSound.clip);
    }

    public void ActivateSnailMode()
    {
        _snailMode = true;
        if (snailCooldown != null)
        {
            StopCoroutine(snailCooldown);
        }
        snailCooldown = SnailModeCooldown();
        StartCoroutine(snailCooldown);
        _powerDownSound.PlayOneShot(_powerDownSound.clip);
    }

    private void PullInPowerUps()
    {
        GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject pickUp in pickUps)
        {
            pickUp.GetComponent<PowerUp>().BeginMoveToPlayer();
        }
    }
}
