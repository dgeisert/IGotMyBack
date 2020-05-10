using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static Projectile[] enemyProjectiles;
    public static Projectile[] playerProjectiles;
    public static ProjectileManager Instance;

    static int index;
    static int index2;

    public static void PlaceEnemyProjectile(Vector3 position, float rotation, float life = 1000, float speed = 10)
    {
        enemyProjectiles[index].Activate(position, rotation, life, speed);
        index++;
        if (index >= enemyProjectiles.Length)
        {
            index = 0;
        }
    }

    public static void PlacePlayerProjectile(Vector3 position, float rotation, float life = 1000, float speed = 10)
    {
        playerProjectiles[index2].Activate(position, rotation, life, speed);
        index2++;
        if (index2 >= playerProjectiles.Length)
        {
            index2 = 0;
        }
    }

    public Projectile enemyProjectile;
    public Projectile playerProjectile;
    int enemyProjectileCount = 500;
    int playerProjectileCount = 100;

    void Awake()
    {
        Instance = this;
        enemyProjectiles = new Projectile[enemyProjectileCount];
        for (int i = 0; i < enemyProjectileCount; i++)
        {
            enemyProjectiles[i] = Instantiate(enemyProjectile, Vector3.zero, Quaternion.identity, transform);
            enemyProjectiles[i].DestroyProjectile();
        }
        playerProjectiles = new Projectile[playerProjectileCount];
        for (int i = 0; i < playerProjectileCount; i++)
        {
            playerProjectiles[i] = Instantiate(playerProjectile, Vector3.zero, Quaternion.identity, transform);
            playerProjectiles[i].DestroyProjectile();
        }
    }
}