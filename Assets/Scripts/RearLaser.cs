﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearLaser : EnemyLaser
{
    //new protected float maxHeight = 8;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed*Time.deltaTime);
        if (transform.position.y > maxHeight)
        {
            //if (transform.parent)
            //{
            //    Destroy(transform.parent.gameObject);
            //}
            Destroy(this.gameObject);
        }
    }
}
