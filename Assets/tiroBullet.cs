using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiroBullet : MonoBehaviour
{
   [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float bulletLifetime = 2.0f;
    [SerializeField] private float bulletSpeed = 10.0f;
    [SerializeField] private float fireCooldown = 0.5f;

    private float fireTimer = 0f;

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && fireTimer >= fireCooldown)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = bullet.transform.forward * bulletSpeed;
        Destroy(bullet, bulletLifetime);
    }
}
