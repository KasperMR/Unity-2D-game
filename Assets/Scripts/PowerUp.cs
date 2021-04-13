using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;

    [SerializeField]
    [Tooltip("0 = tripleShot\n1 = speed\n2 = shield")]
    private int _typeOfPowerUp;
    private bool _moveToPlayer;

    [SerializeField]
    private GameObject _explosion;

    private Transform _playerTransform;

    public bool shotAt = false;

    // Update is called once per frame
    void Update()
    {
        if (_moveToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player)
            {
                //if (typeOfPowerUp == 0)
                //{
                //    player.ActivateTripleShot();
                //}
                //if (typeOfPowerUp == 1)
                //{
                //    Debug.Log("give speed");
                //    //player.ActivateTripleShot();
                //}
                switch (_typeOfPowerUp)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        player.ActivateShield();
                        break;
                    case 3:
                        player.GetAmmo();
                        break;
                    case 4:
                        player.GetHealth();
                        break;
                    case 5:
                        player.ActivateRockets();
                        break;
                    case 6:
                        player.ActivateSnailMode();
                        break;
                    default:
                        Debug.Log("Invalid type of power up assigned");
                        break;
                }
            }
            Destroy(gameObject);
        }
        if (collision.CompareTag("EnemyLaser"))
        {
            GameObject newExplosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            newExplosion.transform.localScale = Vector3.one * 0.2f;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    public void BeginMoveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
        _moveToPlayer = true;

    }
}
