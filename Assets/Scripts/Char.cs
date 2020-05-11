using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Char : MonoBehaviour
{
    public static Char Instance;
    public CameraFollow cam;
    Vector3 mov;
    public float invicibleAfterHit = 0.2f;
    float lastHit;
    public Scarf healthScarf;

    public Transform projectileSpawnPoint, cloneProjectileSpawnPoint;
    public Transform clone;
    float lastFire;

    public bool dash = false;
    public float dashCost;
    public float dashCooldown;
    public float dashDuration;
    public float dashSpeed;
    float lastDash;
    bool canRecharge;
    AudioSource audioSource;
    public GameObject upgrade, shoot;
    public AudioClip dashAudio;
    public AudioClip ouchAudio;
    public bool shooting;
    int projectileLayerMask = 1 << 9;
    int collisionFrameSkipper = 0;

    public float projectileSpeed = 10;
    public float fireRate = 1;
    public float health = 5;
    public float maxHealth = 5;
    public float speed;
    public int attackCount = 1;

    WeaponPickup activePickup;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateHealth(0);
        SceneManager.sceneLoaded += OnSceneLoaded;
        Setup();
        clone.SetParent(null);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Setup();
    }

    void Setup()
    {
        Time.timeScale = 1;
        Game.Instance.active = true;
        transform.position = Vector3.zero;
        cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
        {
            cam.target = transform;
            cam.Setup();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Game.Instance.active)
        {
            return;
        }
        //Player movement
        mov = Vector3.zero;
        if (Controls.Forward && !Controls.Back)
        {
            mov += Vector3.forward;
        }
        else if (Controls.Back && !Controls.Forward)
        {
            mov -= Vector3.forward;
        }
        if (Controls.Right && !Controls.Left)
        {
            mov += Vector3.right;
        }
        else if (Controls.Left && !Controls.Right)
        {
            mov -= Vector3.right;
        }

        shooting = Controls.Shoot;
        clone.gameObject.SetActive(shooting);
        if (shooting)
        {
            Shoot();
        }
        else
        {
            clone.position = transform.position;
        }

        Move(mov.normalized);

        collisionFrameSkipper++;
        if (collisionFrameSkipper >= 10)
        {
            collisionFrameSkipper = 0;
            foreach (Collider col in Physics.OverlapSphere(transform.position, 0.4f, projectileLayerMask, QueryTriggerInteraction.Collide))
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

    public void Move(Vector3 dir)
    {
        transform.position += dir * Time.unscaledDeltaTime * speed;
        clone.position -= dir * Time.unscaledDeltaTime * speed;
        if (!shooting)
        {
            transform.LookAt(transform.position + dir);
            clone.LookAt(clone.position - dir);
        }
    }
    public void Dash()
    {
        audioSource.clip = dashAudio;
        audioSource.Play();
        dash = true;
        lastDash = Time.time;
    }
    public void Shoot()
    {
        if (lastFire + fireRate < Time.time)
        {
            lastFire = Time.time;
            shoot.SetActive(false);
            shoot.SetActive(true);
            for (int i = 0; i < attackCount; i++)
            {
                float shift = i * 10 - (attackCount - 1) / 2;
                ProjectileManager.PlacePlayerProjectile(projectileSpawnPoint.position, projectileSpawnPoint.eulerAngles.y + shift, 100, projectileSpeed);
                ProjectileManager.PlacePlayerProjectile(cloneProjectileSpawnPoint.position, cloneProjectileSpawnPoint.eulerAngles.y + shift, 100, projectileSpeed);
            }
        }
    }

    public void UpdateHealth(float amount)
    {
        if (amount < 0 && lastHit + invicibleAfterHit > Time.time)
        {
            return;
        }
        health += amount;
        if (amount < 0)
        {
            lastHit = Time.time;
            audioSource.clip = ouchAudio;
            audioSource.Play();
            if (cam != null)
            {
                cam.Shake();
            }
            if (health < 0)
            {
                Game.Instance.GameOver();
            }
        }
        if (InGameUI.Instance != null)
        {
            InGameUI.Instance.SetHealth(health);
        }
        healthScarf.SetLength(health);
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Pickup")
        {
            activePickup = collider.GetComponent<WeaponPickup>();
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Pickup")
        {
            if (activePickup == collider.GetComponent<WeaponPickup>())
            {
                activePickup = null;
            }
        }
    }
    public void UpgradeInit()
    {
        upgrade.SetActive(false);
        upgrade.SetActive(true);
    }
}