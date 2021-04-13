using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDodger : Enemy
{
    public void TryToDodge()
    {
        Debug.Log("TryToDodge called");
        StartCoroutine(Dodge());
    }
}
