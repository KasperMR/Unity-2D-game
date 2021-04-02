﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 4;
    [SerializeField]
    private int _scoreValue = 10;
    [SerializeField]
    private bool _finishedAnimating = false;
    [SerializeField]
    private GameObject _enemyLaser;
    private bool _alive = true;

    private void Start()
    {
        StartCoroutine(FireLaser());
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

    private void OnTriggerEnter2D(Collider2D other)
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
   
    private IEnumerator FireLaser()
    {
        while (_alive)
        {
            Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

}
