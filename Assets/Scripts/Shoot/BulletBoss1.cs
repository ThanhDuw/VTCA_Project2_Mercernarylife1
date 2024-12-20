using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject, 0.5f);
        }
    }
}
