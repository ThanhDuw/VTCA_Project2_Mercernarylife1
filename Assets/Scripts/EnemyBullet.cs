using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{   

    public int damage = 10;

    public GameObject impactEffect;
    //ham xu li va cham
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {            
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            if (player != null)
            {
                Instantiate(impactEffect, transform.position, transform.rotation);
                player.TakeDamage(damage);
            }

            Destroy(gameObject, 0.1f);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Shield"))
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        Destroy(gameObject, 1f);
    }

}