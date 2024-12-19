using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossXanh : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Tốc độ di chuyển của enemy
    [SerializeField] private float boundary; // Khoảng cách di chuyển tối đa
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float hp = 30; // Máu của enemy
    [SerializeField] private float shootingRange = 4f; // Tầm bắn trên trục X
    [SerializeField] private float maxYDifference = 1.5f; // Chênh lệch tối đa trên trục Y để Enemy tấn công
    [SerializeField] private GameObject energyWavePrefab; // Prefab của tia chưởng lực (energy wave)
    [SerializeField] private float energyWaveSpeed = 10f; // Tốc độ của tia chưởng lực
    [SerializeField] private Transform player; // Tham chiếu tới player
    private float leftBoundary, rightBoundary;
    private Animator animator;
    private Rigidbody2D rb2D;

    private bool isShooting = false; // Biến kiểm tra xem Enemy đã phóng tia chưa

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

        // Bắt đầu Coroutine phóng tia laser mỗi 3 giây
        StartCoroutine(ShootLaserEvery3Seconds());
    }

    void Update()
    {
        // Kiểm tra xem Player có trong phạm vi tấn công không
        float distanceToPlayerX = Mathf.Abs(transform.position.x - player.position.x);
        float distanceToPlayerY = Mathf.Abs(transform.position.y - player.position.y);

        if (distanceToPlayerX <= shootingRange && distanceToPlayerY <= maxYDifference && !isShooting && IsPlayerInFront())
        {
            StartCoroutine(FireEnergyWave());
        }

        // Nếu không đang ở trạng thái Shield, thì Enemy sẽ di chuyển
        if (!animator.GetBool("Shield"))
        {
            MoveEnemy();
        }
    }

    private bool IsPlayerInFront()
    {
        // Kiểm tra xem Player có đang ở phía trước mặt của Enemy không
        if (spriteRenderer.flipX)
        {
            return player.position.x < transform.position.x;
        }
        else
        {
            return player.position.x > transform.position.x;
        }
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

    private IEnumerator FireEnergyWave()
    {
        // Đánh dấu Enemy đang phóng tia
        isShooting = true;

        // Tính toán vị trí khởi tạo tia laser cách Enemy 1f về phía trước
        Vector3 spawnPosition = transform.position;
        if (spriteRenderer.flipX)
        {
            spawnPosition += Vector3.left * 1f; // Nếu Enemy quay mặt trái, spawnPosition dịch sang trái 1f
        }
        else
        {
            spawnPosition += Vector3.right * 1f; // Nếu Enemy quay mặt phải, spawnPosition dịch sang phải 1f
        }

        // Tạo energy wave (tia chưởng lực) tại vị trí mới
        GameObject energyWave = Instantiate(energyWavePrefab, spawnPosition, Quaternion.identity);

        // Đặt hướng tia chưởng lực (nếu Enemy nhìn sang trái thì tia sẽ đi sang trái, ngược lại đi sang phải)
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        energyWave.GetComponent<Rigidbody2D>().velocity = direction * energyWaveSpeed;

        // Tạm dừng di chuyển của Enemy trong một khoảng thời gian ngắn sau khi phóng tia
        speed = 0f; // Dừng Enemy
        animator.SetTrigger("Attack"); // Nếu có animation Attack, gọi nó

        // Đợi 1 giây trước khi tiếp tục di chuyển
        yield return new WaitForSeconds(1f);

        // Sau khi phóng tia xong, tiếp tục di chuyển
        speed = 1f;

        // Đánh dấu đã hoàn thành việc phóng tia
        isShooting = false;

        // Hủy tia laser sau 2 giây
        Destroy(energyWave, 2f);
    }

    private IEnumerator ShootLaserEvery3Seconds()
    {
        // Liên tục phóng tia mỗi 3 giây
        while (true)
        {
            float distanceToPlayerX = Mathf.Abs(transform.position.x - player.position.x);
            float distanceToPlayerY = Mathf.Abs(transform.position.y - player.position.y);

            if (distanceToPlayerX <= shootingRange && distanceToPlayerY <= maxYDifference && !isShooting && IsPlayerInFront())
            {
                StartCoroutine(FireEnergyWave());
            }
            yield return new WaitForSeconds(3f); // Chờ 3 giây trước khi phóng tia tiếp theo
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (animator.GetBool("Shield"))
        {
            return; // Nếu đang trong trạng thái Shield, không xử lý va chạm
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Lấy hướng của viên đạn khi va chạm
            Vector2 bulletDirection = collision.attachedRigidbody.velocity.normalized;

            // Xác định hướng của viên đạn để bật trạng thái Shield đúng hướng
            if (bulletDirection.x > 0) // Viên đạn đến từ bên trái
            {
                spriteRenderer.flipX = true; // Enemy quay mặt phải
            }
            else if (bulletDirection.x < 0) // Viên đạn đến từ bên phải
            {
                spriteRenderer.flipX = false; // Enemy quay mặt trái
            }

            animator.SetBool("Shield", true); // Bật animation Shield
            StartCoroutine(StopShieldAnimation()); // Gọi Coroutine để tắt Shield sau một khoảng thời gian
        }
    }

    private IEnumerator StopShieldAnimation()
    {
        speed = 0f; // Dừng Enemy
        yield return new WaitForSeconds(3f); // Đợi 3 giây
        speed = 1f;
        animator.SetBool("Shield", false);
    }

}
