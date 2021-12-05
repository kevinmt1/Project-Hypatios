using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    public float enemyLeft;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (enemyLeft == 0)
        {
            anim.SetBool("IsOpened", true);
        }
    }
}
