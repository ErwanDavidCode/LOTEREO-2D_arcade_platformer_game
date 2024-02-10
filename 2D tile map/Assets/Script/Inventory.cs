
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public int contentCurrentIndex = 0;
    public GameObject gun;

    public GameObject sword;
    public GameObject pickaxe;
    public GameObject slot_sword;
    public GameObject slot_gun;
    public GameObject slot_pickaxe;

    void Start()
    {
        GetNextWeapon();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GetNextWeapon();
        }

    }
    public void GetNextWeapon()
    {
        contentCurrentIndex++;
        if (contentCurrentIndex > 2)
        {
            contentCurrentIndex = 0;
        }

        if (contentCurrentIndex == 0)
        {
            //gun
            gun.SetActive(true);
            slot_gun.SetActive(true);

            //sword
            sword.SetActive(false);
            slot_sword.SetActive(false);

            //pickaxe
            pickaxe.SetActive(false);
            slot_pickaxe.SetActive(false);
        }

        else if (contentCurrentIndex == 1)
        {
            gun.SetActive(false);
            slot_gun.SetActive(false);

            sword.SetActive(true);
            slot_sword.SetActive(true);

            pickaxe.SetActive(false);
            slot_pickaxe.SetActive(false);
        }

        else if (contentCurrentIndex == 2)
        {
            gun.SetActive(false);
            slot_gun.SetActive(false);

            sword.SetActive(false);
            slot_sword.SetActive(false);

            pickaxe.SetActive(true);
            slot_pickaxe.SetActive(true);
        }
    }

}



