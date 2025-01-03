using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScritp : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float attackRange = 3f; // Phạm vi tấn công theo trục X
    public float bulletSpeed = 5f;
    public int maxHP = 100;
    private int currentHP;
    private bool isAttacking = false;
    private Animator animator;

    void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            float distanceX = Mathf.Abs(player.transform.position.x - transform.position.x);

            if (distanceX <= attackRange && !isAttacking)
            {
                StartCoroutine(Attack(player));
                break;
            }
        }
    }

    private IEnumerator Attack(GameObject target)
    {
        isAttacking = true;
        animator.SetBool("Attack", true);

        FireBullet(target);

        yield return new WaitForSeconds(1f);

        isAttacking = false;
        animator.SetBool("Attack", false);
    }

    private void FireBullet(GameObject target)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Vector2 direction = (target.transform.position.x > transform.position.x) ? Vector2.right : Vector2.left;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }

    private void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log($"Enemy HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Died!");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(10);
            Destroy(collision.gameObject);
        }
    }
}