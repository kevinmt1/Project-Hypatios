using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageReceiver : MonoBehaviour
{
    public SpiderScript spiderScript;

    public void Attacked(float damage)
    {
        Debug.Log("Test attcekd");
        spiderScript.Attacked(damage);
    }
}
