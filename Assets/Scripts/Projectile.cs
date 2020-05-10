using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Collider collider;
    public string textName;
    public Transform visualObject;
    public bool player = false;
    public float spinSpeed = 5;
    public float speed = 10;
    public float damage = 1;
    public float cost;
    public WeaponPickup pickup;
    float spawnTime;
    public GameObject deathParticles;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public IEnumerator DoDestroy(float wait)
    {
        yield return new WaitForSeconds(wait);
        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        transform.position += 1000 * Vector3.up;
        this.enabled = false;
    }

    public void Activate(Vector3 pos, float rot, float life, float speed)
    {
        StopAllCoroutines();
        this.speed = speed;
        this.spawnTime = Time.time;
        this.enabled = true;
        transform.position = pos;
        transform.localEulerAngles = new Vector3(0, rot, 0);
        StartCoroutine(DoDestroy(life));
    }
}