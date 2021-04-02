using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : MonoBehaviour
{

    private float _coolDownRemaining = 0f;
    [SerializeField]
    private float _coolDown = 2f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _coolDownRemaining)
        {
            //fire laser
            _coolDownRemaining = Time.time + _coolDown;
        }
    }


    //private bool _coolDownReady = true;
    //[SerializeField]
    //private float _coolDown = 2f;

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space) && _coolDownReady)
    //    {
    //        //fire laser
    //        _coolDownReady = false;
    //        StartCoroutine(CoolDownCoroutine());
    //    }
    //}

    //private IEnumerator CoolDownCoroutine() 
    //{
    //    yield return new WaitForSeconds(_coolDown);
    //    _coolDownReady = true;
    //}

    //private bool _coolDownReady = true;
    //[SerializeField]
    //private float _coolDown = 2f;
    //private float _coolDownRemaining = 0f;

    //// Update is called once per frame
    //void Update()
    //{
    //    if (_coolDownReady == false)
    //    {
    //        _coolDownRemaining -= Time.deltaTime;
    //        if (_coolDownRemaining < 0)
    //        {
    //            _coolDownReady = true;
    //        }
    //    }

    //    if (Input.GetKeyDown(KeyCode.Space) && _coolDownReady)
    //    {
    //        //fire laser
    //        _coolDownReady = false;
    //        _coolDownRemaining = _coolDown;
    //    }
    //}
}
