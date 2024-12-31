using System.Collections;
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
    private bool isDead = false; // Kiểm tra trạng thái chết
    public float DeadSpeed = 0f;

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
        if (isDead) return; // Nếu Boss đã chết, không thực hiện logic nào khác

        float distanceToPlayerX = Mathf.Abs(transform.position.x - player.position.x);
        float distanceToPlayerY = Mathf.Abs(transform.position.y - player.position.y);

        if (distanceToPlayerX <= shootingRange && distanceToPlayerY <= maxYDifference && !isShooting && IsPlayerInFront())
        {
            StartCoroutine(FireEnergyWave());
        }

        if (!animator.GetBool("Shield"))
        {
            MoveEnemy();
        }
    }

    private bool IsPlayerInFront()
    {
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
        if (transform.position.x >= rightBoundary)
        {
            speed = -Mathf.Abs(speed);
            spriteRenderer.flipX = true;
        }
        else if (transform.position.x <= leftBoundary)
        {
            speed = Mathf.Abs(speed);
            spriteRenderer.flipX = false;
        }

        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }

    private IEnumerator FireEnergyWave()
    {
        isShooting = true;
        Vector3 spawnPosition = transform.position + (spriteRenderer.flipX ? Vector3.left : Vector3.right) * 1f;

        GameObject energyWave = Instantiate(energyWavePrefab, spawnPosition, Quaternion.identity);
        energyWave.GetComponent<Rigidbody2D>().velocity = (spriteRenderer.flipX ? Vector2.left : Vector2.right) * energyWaveSpeed;

        speed = 0f;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(1f);

        speed = 1f;
        isShooting = false;

        Destroy(energyWave, 2f);
    }

    private IEnumerator ShootLaserEvery3Seconds()
    {
        while (true)
        {
            if (isDead) yield break;

            float distanceToPlayerX = Mathf.Abs(transform.position.x - player.position.x);
            float distanceToPlayerY = Mathf.Abs(transform.position.y - player.position.y);

            if (distanceToPlayerX <= shootingRange && distanceToPlayerY <= maxYDifference && !isShooting && IsPlayerInFront())
            {
                StartCoroutine(FireEnergyWave());
            }
            yield return new WaitForSeconds(3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (animator.GetBool("Shield"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            Vector2 bulletDirection = collision.attachedRigidbody.velocity.normalized;

            if (bulletDirection.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (bulletDirection.x < 0)
            {
                spriteRenderer.flipX = false;
            }

            animator.SetBool("Shield", true);
            StartCoroutine(StopShieldAnimation());

            if (hp <= 0)
            {
                StartCoroutine(HandleDeath());
                transform.Translate(Vector3.up * DeadSpeed * Time.deltaTime);
            }
            else
            {
                hp -= 10;
                Debug.Log("-10 HP");
            }
        }
    }

    private IEnumerator StopShieldAnimation()
    {
        speed = 0f;
        yield return new WaitForSeconds(3f);
        speed = 1f;
        animator.SetBool("Shield", false);
    }

    private IEnumerator HandleDeath()
    {
        isDead = true;
        speed = 0f;

        animator.SetTrigger("Die"); // Kích hoạt Animation Die
        yield return new WaitForSeconds(1.5f); // Chờ animation Die hoàn tất

        // Hạ xác Boss xuống 1f
        float targetY = transform.position.y - 1f;
        float duration = 0.5f; // Thời gian di chuyển (0.5s)
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo vị trí cuối cùng chính xác
        transform.position = targetPosition;

        // Boss sẽ không bị xóa, giữ nguyên xác tại vị trí này
    }
}