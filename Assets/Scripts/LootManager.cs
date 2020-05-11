using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance;

    public Loot[] lootDrops;

    void Start()
    {
        Instance = this;
    }

    public void DropLoot(Vector3 pos)
    {
        Loot loot = lootDrops[Mathf.FloorToInt(Random.value * (lootDrops.Length - (Char.Instance.health >= Char.Instance.maxHealth ? 1 : 0)))];
        Instantiate(loot, pos, Quaternion.identity);
    }
}