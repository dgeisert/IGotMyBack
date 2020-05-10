using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        Projectile proj = col.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.DestroyProjectile();
        }
    }
}