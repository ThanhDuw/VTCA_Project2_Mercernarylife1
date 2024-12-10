using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;

    public Rigidbody2D rb;
    public GameObject impactEffect;
    void Start()
    {
        rb.velocity = transform.right * speed; 
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {       
        Enemy enemy= hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}