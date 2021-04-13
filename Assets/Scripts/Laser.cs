using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8;
    protected float maxHeight = 8; 
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 20f, LayerMask.GetMask("Enemy"));
        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<EnemyDodger>().TryToDodge();
            Debug.Log("enemy detected");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed*Time.deltaTime);
        if (transform.position.y > maxHeight)
        {
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
