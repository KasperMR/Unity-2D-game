using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    protected float _speed = 4;
    [SerializeField]
    protected int _scoreValue = 10;
    [SerializeField]
    private bool _finishedAnimating = false;
    [SerializeField]
    protected GameObject _enemyLaser;
    protected bool _alive = true;

    private void Start()
    {
        StartCoroutine(FireLaserRoutine());
        StartCoroutine(LateralMovementRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        if (transform.position.y < -5.25f)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 7.25f, 0);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        if (other.CompareTag("Laser"))
        {
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
        float lateralMovementSpeed = _speed * 2;
        while (_alive)
        {
            yield return new WaitForSeconds(1f);
            if (Random.Range(0f,1f) > 0.5f)
            {
                if (transform.position.x > 0)
                {
                    for (float t = 0; t < 1; t += Time.deltaTime)
                    {
                        transform.Translate(Vector3.left * lateralMovementSpeed * Time.deltaTime);
                        yield return null;
                    }
                }
                else
                {
                    for (float t = 0; t < 1; t += Time.deltaTime)
                    {
                        transform.Translate(Vector3.right * lateralMovementSpeed * Time.deltaTime);
                        yield return null;
                    }
                }

            }
        }
    }

}
