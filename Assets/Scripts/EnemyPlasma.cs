using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlasma : MonoBehaviour
{

    //thin plasma beam that appears instantly, shimmers in size slight, is harmless when thin
    //after ~0.5s expands massively and does damage while large

    [SerializeField]
    private float windUpDuration = 0.5f;
    [SerializeField]
    private float fullPowerDuration = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlasmaBeamStart(float zRotation)
    {
        StartCoroutine(PlasmaBeam(zRotation));
    }

    private IEnumerator PlasmaBeam(float zRotation)
    {
        //transform.RotateAround(transform.parent.position, Vector3.forward, );
        float timer = 0f;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        //transform.RotateAround(transform.parent.position, Vector3.forward, zRotation);
        transform.parent.up = GameObject.FindGameObjectWithTag("Player").transform.position - transform.parent.position;

        while (timer < windUpDuration)
        {
            timer += Time.deltaTime;

            transform.localScale = new Vector3(Random.Range(0.25f, 0.75f), transform.localScale.y, transform.localScale.z);
            
            yield return null;
        }
        timer = 0;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        Debug.Log(gameObject.GetComponent<BoxCollider2D>().enabled);
        transform.localScale = new Vector3(20, transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(fullPowerDuration);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Debug.Log(gameObject.GetComponent<BoxCollider2D>().enabled);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                player.GetComponent<Player>().TakeDamage();
            }
        }
    }
}
