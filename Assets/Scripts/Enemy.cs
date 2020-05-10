using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool stutterStep;
    public float health;
    public Scarf healthScarf;
    float hitTime;
    public Projectile attack;
    public float range;
    public float attackCooldown;
    public int attackCount;
    public float attackShift = 20;
    float lastAttack;
    public Transform attackSpawnLoc;
    public float speed;
    public GameObject deathParticles;
    int patrolPoint;
    float wanderTime;
    float wanderDir;
    public float size = 1;
    Transform target;
    public AudioClip alertAudio;
    public AudioClip ouchAudio;
    AudioSource audioSource;
    public Reward reward;
    int collisionFrameSkipper = 0;
    int projectileLayerMask = 1 << 11;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        transform.eulerAngles = Vector3.up * Random.value * 360;
    }

    // Update is called once per frame
    void Update()
    {
        target = Char.Instance.transform;
        if (Vector3.Distance(transform.position, target.position) <= range)
        {
            Face();
            if (lastAttack + attackCooldown < Time.time)
            {
                RaycastHit hit;
                Physics.Raycast(transform.position,
                    (Char.Instance.transform.position - transform.position),
                    out hit);
                Attack();
            }
            else if (stutterStep && lastAttack + (attackCooldown * 3 / 4) > Time.time && lastAttack + (attackCooldown * 1 / 4) < Time.time)
            {
                Move();
            }
        }
        else
        {
            Face();
            Move();
        }
        Knockback();
        CheckCollisions();
    }

    void Face()
    {
        transform.LookAt(target.position);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    void Move()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    void Attack()
    {
        for (int i = 0; i < attackCount; i++)
        {
            float shift = i * attackShift - (attackCount - 1) / 2;
            ProjectileManager.PlaceEnemyProjectile(attackSpawnLoc.position, attackSpawnLoc.eulerAngles.y + shift);
            lastAttack = Time.time;
        }
    }

    public void UpdateHealth(float amount)
    {
        health += amount;
        if (amount < 0)
        {
            hitTime = Time.time;
            audioSource.clip = ouchAudio;
            audioSource.Play();
            if (health <= 0)
            {
                Die();
            }
        }
        if (healthScarf != null)
        {
            healthScarf.SetLength(health);
        }
    }

    public void Knockback()
    {
        if (hitTime + 0.1f > Time.time && speed > 0)
        {
            transform.position -= transform.forward * Time.deltaTime * 4;
        }
    }

    void CheckCollisions()
    {
        collisionFrameSkipper++;
        if (collisionFrameSkipper >= 1)
        {
            collisionFrameSkipper = 0;
            foreach (Collider col in Physics.OverlapSphere(transform.position, size, projectileLayerMask, QueryTriggerInteraction.Collide))
            {
                Projectile proj = col.GetComponent<Projectile>();
                if (proj != null)
                {
                    UpdateHealth(-proj.damage);
                    proj.DestroyProjectile();
                }
            }
        }
    }

    void Die()
    {
        if (deathParticles != null)
        {
            Destroy(Instantiate(deathParticles, transform.position, transform.rotation), 2);
        }
        if (reward != null)
        {
            reward.Activate(transform.position);
        }
        if (Game.Instance.enemies.Count == 1)
        {
            LootManager.Instance.DropLoot(transform.position);
        }
        Game.Instance.enemies.Remove(this);
        Destroy(gameObject);
    }
}