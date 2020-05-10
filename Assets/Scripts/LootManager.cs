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
        Instantiate(lootDrops[Mathf.FloorToInt(Random.value * lootDrops.Length)], pos, Quaternion.identity);
    }
}