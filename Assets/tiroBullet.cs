using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiroBullet : MonoBehaviour
{
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Transform bulletSpawn;
    [SerializeField] public float bulletVida = 2.0f;
    [SerializeField] public float bulletSpeed = 6.0f;

    private float currentTime = 0.0f;
    public float ataqueTime = 2.0f;

    void Update()
    {
        if (currentTime <= ataqueTime)
        {
            currentTime = currentTime + Time.deltaTime;
            Debug.Log(currentTime);
        }
    }

    void FixedUpdate()
    {
        
        if ( ( Input.GetKey(KeyCode.Mouse0 ) ) && ( currentTime >= ataqueTime ) )
        {
            Fire();
            currentTime = 0;                        
        }
    }

    void Fire()
    {
        // Cria um Bullet a partir de BulletPrefab
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Adiciona velocidade a Bullet
        bullet.GetComponent<Rigidbody>().linearVelocity = bullet.transform.forward * bulletSpeed;

        // Destruir Bullet depois de n segundos
        Destroy(bullet, bulletVida);
    }
}
