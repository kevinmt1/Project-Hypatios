using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObjectDeactivator : MonoBehaviour
{

    public float timer = 0.1f;
    private float currentTimer = 0.1f;

    private bool isStart = false;

    public void OnEnable()
    {
        if (isStart == false)
        {
            isStart = true;
        }
        else
        {
            currentTimer = timer;
        }


    }

    private void Update()
    {
        if (currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
