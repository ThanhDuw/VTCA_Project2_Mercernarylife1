using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 1f; // Tốc độ di chuyển của enemy
    [SerializeField] private float boundary; // Khoảng cách di chuyển tối đa
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float hp = 30; // Máu của enemy
    [SerializeField] private float shootingRange = 4f; // Tầm bắn
    [SerializeField] private float fireRate = 1f; // Tốc độ bắn
    [SerializeField] private GameObject bulletPrefab; // Prefab của đạn
    [SerializeField] private float bulletSpeed = 10f; // Tốc độ của đạn
    [SerializeField] private Transform player; // Tham chiếu tới player

    private float leftBoundary, rightBoundary;
    private float fireCooldown = 0f;
    private Animator animator;
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        leftBoundary = transform.position.x - boundary;
        rightBoundary = transform.position.x + boundary;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Đặt hướng di chuyển ban đầu sang trái
        speed = -Mathf.Abs(speed);
        spriteRenderer.flipX = true; // Quay mặt trái
    }

    void Update()
    {
        // Di chuyển enemy
        MoveEnemy();

        // Bắn đạn nếu player trong tầm bắn và cùng hướng
        TryShoot();
    }

    private void MoveEnemy()
    {
        // Đổi hướng di chuyển khi đạt tới ranh giới
        if (transform.position.x >= rightBoundary)
        {
            speed = -Mathf.Abs(speed);
            spriteRenderer.flipX = true; // Quay mặt trái
            animator.SetBool("Walk", true);
        }
        else if (transform.position.x <= leftBoundary)
        {
            speed = Mathf.Abs(speed);
            spriteRenderer.flipX = false; // Quay mặt phải
            animator.SetBool("Walk", true);
        }

        // Di chuyển theo trục X
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }

    private void TryShoot()
    {
        // Kiểm tra khoảng cách giữa enemy và player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Kiểm tra nếu player trong tầm bắn và cùng hướng với enemy
        bool isPlayerInDirection = (speed > 0 && player.position.x > transform.position.x) ||
                                   (speed < 0 && player.position.x < transform.position.x);

        if (distanceToPlayer <= shootingRange && isPlayerInDirection)
        {
            fireCooldown -= Time.deltaTime; // Giảm thời gian hồi chiêu

            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = 1f / fireRate; // Đặt lại thời gian hồi chiêu
            }
        }
    }

    private void Shoot()
    {
        // Tạo viên đạn tại vị trí hiện tại của enemy
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Gắn Rigidbody2D vào đạn để di chuyển
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Xác định hướng bắn dựa trên hướng di chuyển của enemy
            Vector2 shootDirection = speed > 0 ? Vector2.right : Vector2.left;
            rb.velocity = shootDirection * bulletSpeed;
        }

        // Hủy viên đạn sau 5 giây để tránh tràn bộ nhớ
        Destroy(bullet, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isAttacking", true);
        }

        // Giảm máu nếu bị trúng đạn
        if (collision.gameObject.CompareTag("VienDan"))
        {
            hp -= 10; // Trừ máu khi bị trúng đạn
            if (hp <= 0)
            {
                Destroy(gameObject); // Hủy enemy nếu máu <= 0
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isAttacking", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Hiển thị tầm bắn trong Scene
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}