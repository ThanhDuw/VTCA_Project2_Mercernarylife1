using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{   

    public int damage = 10;
    //ham xu li va cham
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {            
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject, 0.1f);
        }
        
    }

}