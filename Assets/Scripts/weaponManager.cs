using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponManager : MonoBehaviour
{

    public int selectedWeapon = 0;
    public GameObject weaponHolder;
    public GameObject weaponNumToSwap;
    public GameObject weaponToSwap;
    public Animator anim;
    public int previousWeapon;
    public gunScript gun;

    public Sprite[] weaponSprite;
    public GameObject weaponUI;


    // Start is called before the first frame update
    void Start()
    {
        switchWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        previousWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
            
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }

        if (previousWeapon != selectedWeapon)
        {
            gun.isReloading = false;
            gun.curReloadTime = gun.reloadTime;
            gun.isScoping = false;
            
            switchWeapon();
        }

        findEquippedWeapon();
    }

    void switchWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weaponUI.GetComponent<Image>().sprite = weaponSprite[i];
                weapon.gameObject.SetActive(true);
                anim = weapon.transform.GetChild(0).gameObject.GetComponent<Animator>();
                gun = weapon.transform.GetChild(0).gameObject.GetComponent<gunScript>();
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
        
    }

    void findEquippedWeapon()
    {
        weaponNumToSwap = weaponHolder.transform.GetChild(selectedWeapon).gameObject;
        weaponToSwap = weaponNumToSwap.transform.GetChild(0).gameObject;
    }
}
