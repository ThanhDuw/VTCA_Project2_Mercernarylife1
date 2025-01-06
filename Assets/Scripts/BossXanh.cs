using System.Collections;
using UnityEngine;

public class BossXanh : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Tốc độ di chuyển của Boss
    [SerializeField] private float boundary; // Khoảng cách di chuyển tối đa
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int maxHp = 500; // Máu của Boss
    [SerializeField] private int currentHp;
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
        // Đặt hướng di chuyển ban đầu sang trái
        speed = -Mathf.Abs(speed);
        spriteRenderer.flipX = true; // Quay mặt sang trái

        // Bắt đầu Coroutine bắn mỗi 3 giây
        StartCoroutine(ShootLaserEvery3Seconds());
    }

    void Update()
    {
        if (isDead) return; // Nếu Boss đã chết, không làm gì cả

        if (!animator.GetBool("Shield"))
        {
            MoveEnemy();
        }
    }

    private void MoveEnemy()
    {
        if (transform.position.x >= rightBoundary)
        {
            speed = -Mathf.Abs(speed); // Di chuyển sang trái
            spriteRenderer.flipX = true;
        }
        else if (transform.position.x <= leftBoundary)
        {
            speed = Mathf.Abs(speed); // Di chuyển sang phải
            spriteRenderer.flipX = false;
        }

        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }

    private IEnumerator FireEnergyWave()
    {
        isShooting = true;

        // Xác định vị trí spawn của tia chưởng lực
        Vector3 spawnPosition = transform.position + (spriteRenderer.flipX ? Vector3.left : Vector3.right) * 1f;

        // Tạo tia chưởng lực
        GameObject energyWave = Instantiate(energyWavePrefab, spawnPosition, Quaternion.identity);
        energyWave.GetComponent<Rigidbody2D>().velocity = (spriteRenderer.flipX ? Vector2.left : Vector2.right) * energyWaveSpeed;

        // Tạm dừng di chuyển trong lúc bắn
        speed = 0f;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(1f); // Thời gian tạm nghỉ khi bắn

        speed = 1f;
        isShooting = false;

        Destroy(energyWave, 2f); // Xóa tia chưởng lực sau 2 giây
    }

    private IEnumerator ShootLaserEvery3Seconds()
    {
        while (true)
        {
            if (isDead) yield break;

            // Tìm tất cả các đối tượng Player trong game
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                float distanceToPlayerX = Mathf.Abs(transform.position.x - player.transform.position.x);
                float distanceToPlayerY = Mathf.Abs(transform.position.y - player.transform.position.y);

                // Kiểm tra nếu Player trong tầm bắn trên trục X và chênh lệch Y không vượt quá maxYDifference
                if (distanceToPlayerX <= shootingRange && distanceToPlayerY <= maxYDifference && !isShooting && IsPlayerInFront(player))
                {
                    StartCoroutine(FireEnergyWave());
                    break; // Chỉ bắn vào một mục tiêu
                }
            }

            yield return new WaitForSeconds(3f); // Chờ 3 giây trước lần bắn tiếp theo
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

            if (currentHp <= 0)
            {
                StartCoroutine(HandleDeath());
                transform.Translate(Vector3.up * DeadSpeed * Time.deltaTime);
            }
            else
            {
                currentHp -= 10;
                hpBar.SetHealth(currentHp);
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