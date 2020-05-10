using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public TMPro.TextMeshPro infoDisplay;
    public Projectile weapon;
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            infoDisplay.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            infoDisplay.gameObject.SetActive(false);
        }
    }
}