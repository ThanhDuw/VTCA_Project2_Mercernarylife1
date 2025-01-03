using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1f; // Tốc độ di chuyển của enemy
    [SerializeField] private float boundary; // Khoảng cách di chuyển tối đa
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float hp = 30; // Máu của enemy
    [SerializeField] private float shootingRange = 2f; // Tầm bắn theo trục X
    [SerializeField] private float fireRate = 1f; // Tốc độ bắn
    [SerializeField] private GameObject bulletPrefab; // Prefab của đạn
    [SerializeField] private float bulletSpeed = 10f; // Tốc độ của đạn

    private float leftBoundary, rightBoundary;
    private float fireCooldown = 0f;
    private Animator animator;
    private Rigidbody2D rb2D;

    public GameObject destructionFX;

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

        // Bắn đạn nếu tìm thấy player trong phạm vi bắn
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
        // Tìm tất cả các đối tượng có Tag "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            float distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);

            // Kiểm tra nếu player trong tầm bắn (theo trục X) và cùng hướng với enemy
            bool isPlayerInDirection = (speed > 0 && player.transform.position.x > transform.position.x) ||
                                       (speed < 0 && player.transform.position.x < transform.position.x);

            if (distanceToPlayer <= shootingRange && isPlayerInDirection)
            {
                fireCooldown -= Time.deltaTime; // Giảm thời gian hồi chiêu

                if (fireCooldown <= 0f)
                {
                    Shoot();
                    fireCooldown = 1f / fireRate; // Đặt lại thời gian hồi chiêu
                }
                break; // Ngừng kiểm tra nếu đã bắn
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

    void OnDrawGizmosSelected()
    {
        // Hiển thị tầm bắn trong Scene
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(destructionFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}