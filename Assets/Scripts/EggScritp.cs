using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScritp : MonoBehaviour
{
    public Transform player; // Gắn Transform của Player
    public GameObject bulletPrefab; // Prefab của viên đạn
    public float attackRange = 3f; // Phạm vi tấn công theo trục X
    public float bulletSpeed = 5f; // Tốc độ của viên đạn
    public int maxHP = 100; // Máu tối đa của Enemy
    private int currentHP; // Máu hiện tại của Enemy
    private bool isAttacking = false;
    private Animator animator;

    void Start()
    {
        // Khởi tạo máu ban đầu
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Kiểm tra khoảng cách giữa Enemy và Player theo trục X
        float distanceX = Mathf.Abs(player.position.x - transform.position.x);

        if (distanceX <= attackRange && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("Attack", true);
        // Tấn công: Bắn viên đạn
        FireBullet();

        // Thời gian tạm nghỉ trước khi tấn công tiếp (giả sử 1 giây)
        yield return new WaitForSeconds(1f);

        isAttacking = false;
        animator.SetBool("Attack", false);
    }

    private void FireBullet()
    {
        // Tạo viên đạn tại vị trí hiện tại của Enemy
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Tìm hướng bắn viên đạn (theo hướng Player)
        Vector2 direction = (player.position.x > transform.position.x) ? Vector2.right : Vector2.left;

        // Thêm lực cho viên đạn để nó di chuyển
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }

    private void TakeDamage(int damage)
    {
        // Trừ máu khi bị bắn
        currentHP -= damage;

        Debug.Log($"Enemy HP: {currentHP}");

        // Kiểm tra nếu máu <= 0
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Died!");
        Destroy(gameObject); // Xóa Enemy khỏi game
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu Enemy va chạm với viên đạn có Tag "Bullet"
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(10); // Trừ 10 HP
            Destroy(collision.gameObject); // Xóa viên đạn sau khi va chạm
        }
    }
}
