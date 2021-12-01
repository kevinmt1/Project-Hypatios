using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightSniper : MonoBehaviour
{

    public float rotateSpeed = 3;
    public Transform target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var q = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotateSpeed * Time.deltaTime);
    }
}
