using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public float fadeDuration = 1.0f; // Thời gian để lồng giam mờ dần
    public GameObject victoryText;   // Đối tượng hiển thị thông báo chiến thắng

    private SpriteRenderer spriteRenderer;
    private bool isFading = false;
    [SerializeField] private AudioSource backgroundMusic;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ẩn thông báo Victory ban đầu
        if (victoryText != null)
        {
            victoryText.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isFading) // Kiểm tra Player chạm vào
        {
            StartCoroutine(FadeOutAndDestroy()); // Gọi hiệu ứng mờ dần
        }
    }

    IEnumerator FadeOutAndDestroy()
    {
        isFading = true; // Đảm bảo không chạy hiệu ứng nhiều lần
        float elapsedTime = 0f;

        // Lấy màu hiện tại của Sprite
        Color originalColor = spriteRenderer.color;

        // Hiệu ứng mờ dần
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // Giảm Alpha từ 1 xuống 0
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Xóa lồng giam sau khi mờ dần
        Destroy(gameObject);

        // Hiển thị thông báo Victory
        if (victoryText != null)
        {
            victoryText.SetActive(true);
            Time.timeScale=0f;
            if (backgroundMusic != null && backgroundMusic.isPlaying)
            {
                backgroundMusic.Pause(); // Hoặc backgroundMusic.Stop();
            }
        }
    }
}
