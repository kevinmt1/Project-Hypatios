using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWeaponScript : MonoBehaviour
{

    public int weaponNum;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject weaponHolder = GameObject.FindGameObjectWithTag("GunHolder");
            weaponHolder.GetComponent<weaponManager>().addWeapon(weaponNum);
            Destroy(gameObject);
        }
    }
}
