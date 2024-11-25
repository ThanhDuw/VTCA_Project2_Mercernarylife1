using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{

    private Rigidbody2D rigidbody2D;
    [SerializeField] private float health = 30f;
    [SerializeField] private GameObject GameOver;

    private float countdownTime = 40f; // Thời gian đếm ngược (40 giây)
    [SerializeField] private TextMeshProUGUI countdownText; // Text UI hiển thị thời gian

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        /*GameOver.SetActive(false);*/
    }

    void Update()
    {
        // Nếu GameOver đang active thì không thực hiện gì
        /*if (GameOver.activeSelf) return;*/

        // Giảm thời gian đếm ngược
        countdownTime -= Time.deltaTime;

        // Giới hạn thời gian không nhỏ hơn 0
        if (countdownTime < 0) countdownTime = 0;

        // Cập nhật UI nếu có
        if (countdownText != null)
        {
            countdownText.text = FormatTime(countdownTime);
        }

        // Nếu thời gian hết, thực hiện xử lý player chết
        if (countdownTime <= 0)
        {
            PlayerDeath();
        }
    }

    // Xử lý player chết
    void PlayerDeath()
    {
        /*GameOver.SetActive(true);
        Debug.Log("Player is dead.");
        Time.timeScale = 0; // Tạm dừng game*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyShoot")) /*&& !GameOver.activeSelf)*/
        {
            if (health <= 0)
            {
                /*Time.timeScale = 0;
                GameOver.SetActive(true);
                Debug.Log("Player is dead due to no health.");*/
                Destroy(gameObject);
            }
            else
            {
                health -= 10; // Số máu player bị mất khi trúng đạn
            }
        }
    }

    // Hàm format thời gian thành phút:giây
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
