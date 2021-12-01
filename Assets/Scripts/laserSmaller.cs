using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserSmaller : MonoBehaviour
{
    float laserWidth = .4f;
    LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (laserWidth > 0f)
        {
            laserWidth -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
        lr.startWidth = laserWidth;
        lr.endWidth = laserWidth;
    }
}
