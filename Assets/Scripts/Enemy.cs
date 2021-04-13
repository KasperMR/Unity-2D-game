using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    protected float _speed = 4;
    private float _lateralMovementSpeed;
    [SerializeField]
    protected int _scoreValue = 10;
    [SerializeField]
    private bool _finishedAnimating = false;
    [SerializeField]
    protected GameObject _enemyLaser;
    protected bool _alive = true;
    [SerializeField]
    protected bool _shielded = false;

    [SerializeField]
    protected float _rotationSpeed = 0.1f; 
    protected Transform _playerTransform;

    protected void Start()
    {
        _lateralMovementSpeed = _speed * 2;
        StartCoroutine(FireLaserRoutine());
        StartCoroutine(LateralMovementRoutine());
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        CheckForAndFireAtPickups();
        if (transform.position.y < -5.25f)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 7.25f, 0);
        }
        if (_playerTransform != null)
        {
            if (Vector3.SqrMagnitude(transform.position - _playerTransform.position) < 3*3)
            {
                //transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, Time.deltaTime * _speed);
                //transform.up = transform.position - _playerTransform.position;
                //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.up*-1f,(transform.position - _playerTransform.position),_rotationSpeed*Time.deltaTime, 0.0f), Vector3.up);
                //transform.up = Vector3.RotateTowards(transform.up * -1f, (_playerTransform.position - transform.position), _rotationSpeed * Time.deltaTime, 0.0f);
                RotateTowards((Vector2)_playerTransform.position);
            }
            else
            {                
                transform.up = Vector3.up;
            }
            transform.Translate(transform.up * -1 * Time.deltaTime * _speed);
        }
        else //just let the enemies keep moving down the screen after game over
        {
            transform.Translate(transform.up * -1 * Time.deltaTime * _speed);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        if (other.CompareTag("Laser"))
        {
            if (_shielded)
            {
                return;
            }
            _speed = 0;
            animator.SetTrigger("EnemyDeath");
            Destroy(other.gameObject);            
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().ChangeScore(_scoreValue);
            StartCoroutine(DestroyAfterAnimationRoutine());
            gameObject.GetComponent<AudioSource>().Play();
            _alive = false;
            gameObject.tag = "Untagged";
            Destroy(gameObject.GetComponent<Collider2D>());
        }
        if (other.CompareTag("Player"))
        {
            _speed = 0;
            animator.SetTrigger("EnemyDeath");
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage();
            }
            StartCoroutine(DestroyAfterAnimationRoutine());
            gameObject.GetComponent<AudioSource>().Play();
            _alive = false;
            gameObject.tag = "Untagged";
            Destroy(gameObject.GetComponent<Collider2D>());
        }
    }

    private IEnumerator DestroyAfterAnimationRoutine()
    {
        yield return new WaitUntil(() => _finishedAnimating == true);
        {
            Destroy(gameObject);
        }
    }
   
    private IEnumerator FireLaserRoutine()
    {
        while (_alive)
        {
            Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    private IEnumerator LateralMovementRoutine()
    {

        while (_alive)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            
        }
    }

    protected IEnumerator Dodge()
    {
        Debug.Log("dodging");
        if (transform.position.x > 0)
        {
            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                if (_alive)
                {
                    transform.Translate(Vector3.left * _lateralMovementSpeed * Time.deltaTime);
                    yield return null;
                }
            }
        }
        else
        {
            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                if (_alive)
                {
                    transform.Translate(Vector3.right * _lateralMovementSpeed * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }

    private void CheckForAndFireAtPickups()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up * -1,100f, 1 << LayerMask.NameToLayer("PowerUp"));
        if (hit.collider != null)
        {
            Debug.Log("hit.collider.GameObject.name: " + hit.collider.gameObject.name);
            if (hit.collider.CompareTag("PowerUp") && hit.collider.gameObject.GetComponent<PowerUp>().shotAt == false)
            {
                Instantiate(_enemyLaser, transform.position, Quaternion.identity);
                hit.collider.gameObject.GetComponent<PowerUp>().shotAt = true;
                Debug.Log("Shot at powerup");
            }
        }
    }

    private void RotateTowards(Vector2 target) //straight up stolen, only vaguely understand it
    {
        var offset = 90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
    }


}
