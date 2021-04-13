using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser_FanFire : EnemyLaser
{
    [SerializeField]
    new private float _speed = 8;
    new private float maxHeight = -8;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * _speed*Time.deltaTime*-1);
        if (transform.position.y < maxHeight)
        {
            Destroy(this.gameObject);
        }
    }

}
