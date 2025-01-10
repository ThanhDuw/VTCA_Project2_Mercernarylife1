using System.Collections;
using UnityEngine;

public class BossXanh : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Tốc độ di chuyển của Boss
    [SerializeField] private float boundary; // Khoảng cách di chuyển tối đa
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int maxHp = 500; // Máu của Boss
    [SerializeField] public int currentHp;
    [SerializeField] private float shootingRange = 4f; // Tầm bắn trên trục X
    [SerializeField] private float maxYDifference = 1.5f; // Chênh lệch tối đa trên trục Y để Boss tấn công
    [SerializeField] private GameObject energyWavePrefab; // Prefab của tia chưởng lực
    [SerializeField] private float energyWaveSpeed = 10f; // Tốc độ của tia chưởng lực

    private float leftBoundary, rightBoundary;
    private Animator animator;
    private Rigidbody2D rb2D;

    private bool isShooting = false; // Kiểm tra trạng thái đang bắn
    private bool isDead = false; // Trạng thái chết
    public float DeadSpeed = 0f;
    private bool isShieldCooldown = false; // Biến kiểm tra xem có đang trong thời gian hồi chiêu khiên không
    private bool isShieldActive = false; // Biến kiểm tra xem khiên có đang được bật không

    public Boss_HpBar hpBar;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        leftBoundary = transform.position.x - boundary;
        rightBoundary = transform.position.x + boundary;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        currentHp = maxHp;
        hpBar.SetMaxHealth(maxHp);
        speed = -Mathf.Abs(speed);
        spriteRenderer.flipX = true; // Quay mặt sang trái

        StartCoroutine(ShootLaserEvery3Seconds());
    }

    void Update()
    {
        if (isDead) return;

        if (!animator.GetBool("Shield") && !isShieldCooldown)
        {
            MoveEnemy();
        }

        // Debug trạng thái khiên
        Debug.Log($"Shield Active: {animator.GetBool("Shield")}, Cooldown: {isShieldCooldown}");
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
        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(1f);

        speed = 1f;
        isShooting = false;
        animator.SetBool("Attack", false);
        Destroy(energyWave, 2f);
    }

    private IEnumerator ShootLaserEvery3Seconds()
    {
        while (true)
        {
            if (isDead) yield break;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                float distanceToPlayerX = Mathf.Abs(transform.position.x - player.transform.position.x);
                float distanceToPlayerY = Mathf.Abs(transform.position.y - player.transform.position.y);

                if (distanceToPlayerX <= shootingRange && distanceToPlayerY <= maxYDifference && !isShooting && IsPlayerInFront(player))
                {
                    StartCoroutine(FireEnergyWave());
                    break;
                }
            }

            yield return new WaitForSeconds(3f);
        }
    }

    private bool IsPlayerInFront(GameObject player)
    {
        if (spriteRenderer.flipX)
        {
            return player.transform.position.x < transform.position.x;
        }
        else
        {
            return player.transform.position.x > transform.position.x;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead || isShooting) return; // Nếu Boss đang chết hoặc bắn, không nhận sát thương

        // Kiểm tra xem Boss có đang trong thời gian chờ khiên không
        if (!isShieldActive && !isShieldCooldown) // Boss không có khiên và không đang trong thời gian chờ
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                currentHp -= 10; // Trừ máu
                hpBar.SetHealth(currentHp); // Cập nhật thanh máu

                if (currentHp <= 0)
                {
                    StartCoroutine(HandleDeath());
                    return;
                }

                // Xác định hướng viên đạn và quay mặt của Boss
                Vector2 bulletDirection = collision.attachedRigidbody.velocity.normalized;
                spriteRenderer.flipX = bulletDirection.x > 0;

                // Sau khi bị bắn, bật khiên ngay lập tức (nếu không trong thời gian chờ)
                StartCoroutine(StopShieldAnimation());
            }
        }
        else if (isShieldCooldown && !isShieldActive) // Boss đang trong thời gian chờ (2 giây sau khi tắt khiên)
        {
            // Nếu Boss đang trong thời gian chờ, nhận sát thương nhưng không bật lại khiên
            if (collision.gameObject.CompareTag("Bullet"))
            {
                currentHp -= 10; // Trừ máu
                hpBar.SetHealth(currentHp); // Cập nhật thanh máu

                if (currentHp <= 0)
                {
                    StartCoroutine(HandleDeath());
                    return;
                }

                // Xác định hướng viên đạn và quay mặt của Boss
                Vector2 bulletDirection = collision.attachedRigidbody.velocity.normalized;
                spriteRenderer.flipX = bulletDirection.x > 0;
            }
        }
    }

    private IEnumerator StopShieldAnimation()
    {
        isShieldActive = true;  // Khiên được bật
        isShieldCooldown = true; // Đang trong thời gian cooldown

        animator.SetBool("Shield", true); // Bật khiên
        speed = 0f; // Dừng di chuyển

        yield return new WaitForSeconds(3f); // Giữ khiên trong 3 giây

        animator.SetBool("Shield", false); // Tắt khiên
        speed = 1f; // Khôi phục di chuyển

        // Sau khi tắt khiên, Boss vào thời gian chờ 2 giây và sẽ nhận sát thương trong thời gian này
        yield return new WaitForSeconds(2f); // Tắt animator trong 2 giây, Boss nhận sát thương

        isShieldActive = false; // Khiên đã tắt hoàn toàn

        yield return new WaitForSeconds(2f); // Thời gian hồi chiêu 2 giây trước khi có thể bật lại khiên
        isShieldCooldown = false; // Hết thời gian hồi chiêu, có thể bật lại khiên
    }

    private IEnumerator HandleDeath()
    {
        isDead = true;
        speed = 0f;

        animator.SetTrigger("Die");
        yield return new WaitForSeconds(1.5f);

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Destroy(boxCollider);
        }

        float targetY = transform.position.y - 1f;
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        KeyScritp keyDrop = GetComponent<KeyScritp>();
        if (keyDrop != null)
        {
            keyDrop.DropKey(transform.position);
        }
        else
        {
            Debug.LogWarning("KeyDrop script is not attached to Boss.");
        }
    }
}