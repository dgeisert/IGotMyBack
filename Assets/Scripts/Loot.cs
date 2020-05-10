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
        if (Char.Instance.health >= Char.Instance.maxHealth && lootType == LootType.HEALTH)
        {
            lootType = LootType.FIRERATE;
        }
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
                    c.projectileSpeed *= 1.2f;
                    break;
                case LootType.SPEED:
                    c.speed *= 1.2f;
                    break;
                case LootType.FIRERATE:
                    c.fireRate /= 1.2f;
                    break;
            }
            Destroy(gameObject);
        }
    }
}