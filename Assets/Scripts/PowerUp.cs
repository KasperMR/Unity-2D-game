using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;

    [SerializeField]
    [Tooltip("0 = tripleShot\n1 = speed\n2 = shield")]
    private int typeOfPowerUp;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
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
                switch (typeOfPowerUp)
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
                    default:
                        Debug.Log("Invalid type of power up assigned");
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}
