using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3;
    public float damage = 4;
    public int bulletForce = 300;
    public float bulletDamage;

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.GetComponent<Monster>() != null && collision.gameObject.CompareTag("Monster"))
        {
            Destroy(gameObject);
            collision.GetComponent<Monster>().TakeDamage(bulletDamage);

            // knockback
            Vector2 recoilDirection = (collision.transform.position - transform.position).normalized;
            collision.attachedRigidbody.AddForce(recoilDirection * bulletForce);

        }

        if (collision.GetComponent<FlyingMonsters>() != null && collision.gameObject.CompareTag("Monster"))
        {
            Destroy(gameObject);
            collision.GetComponent<FlyingMonsters>().TakeDamage(bulletDamage);

            // knockback
            Vector2 recoilDirection = (collision.transform.position - transform.position).normalized;
            collision.attachedRigidbody.AddForce(recoilDirection * bulletForce);

        }
    }
}