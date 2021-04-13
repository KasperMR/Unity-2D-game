using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraShakeInit()
    {
        StartCoroutine(CameraShakeRoutine());
    }

    private IEnumerator CameraShakeRoutine()
    {
        float timer = 0.2f;
        float counter = 0f;
        Vector3 newPos = new Vector3(0,0,-10);
        while (timer > counter)
        {
            newPos.x = Random.Range(-0.2f, 0.2f);
            newPos.y = Random.Range(-0.2f, 0.2f);
            transform.position = newPos;
            counter += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(0, 0, -10);
    }
}
