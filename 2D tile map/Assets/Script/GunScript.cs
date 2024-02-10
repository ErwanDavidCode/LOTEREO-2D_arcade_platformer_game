using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun2D : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 30;
    public SpriteRenderer sr;
    private Transform transform_gun;
    public bool bullet_right = true;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform_gun = transform;
    }

    void Update()
    {
        // Permet l'instanciation de la balle et lui donne une vitesse
        if (Input.GetButtonDown("Fire1"))
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            SoundEffects.Instance.MakeShootSound();
            if (bullet_right)
            {
                bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawnPoint.right * bulletSpeed;
            }
            else
            {
                bullet.GetComponent<Rigidbody2D>().velocity = -bulletSpawnPoint.right * bulletSpeed;
            }
        }
    }
}