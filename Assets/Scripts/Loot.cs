using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootType
{
    HEALTH,
    ATTACKCOUNT,
    PROJECTILESPEED,
    SPEED,
    FIRERATE
}

public class Loot : MonoBehaviour
{
    public LootType lootType;

    void OnTriggerEnter(Collider col)
    {
        Char c = col.GetComponentInParent<Char>();
        if (c != null)
        {
            switch (lootType)
            {
                case LootType.HEALTH:
                    c.UpdateHealth(1);
                    break;
                case LootType.ATTACKCOUNT:
                    c.attackCount++;
                    break;
                case LootType.PROJECTILESPEED:
                    c.projectileSpeed += 4f;
                    break;
                case LootType.SPEED:
                    c.speed += 0.25f;
                    break;
                case LootType.FIRERATE:
                    c.fireRate /= 1.5f;
                    break;
            }
            Destroy(gameObject);
        }
        c.UpgradeInit();
    }
}