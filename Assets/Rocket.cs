using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private GameObject target;
    [SerializeField]
    private float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        float nearestDistance = 10000000;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                target = enemy;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            transform.up = target.transform.position - transform.position;

            //transform.LookAt(target.transform.position);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
