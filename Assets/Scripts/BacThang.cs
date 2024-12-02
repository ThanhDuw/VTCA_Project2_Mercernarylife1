using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacThang : MonoBehaviour
{
    [SerializeField] private float moveDistance = 5f; // Khoảng cách di chuyển lên/xuống
    [SerializeField] private float moveSpeed = 2f; // Tốc độ di chuyển
    [SerializeField] private float pauseDuration = 2f; // Thời gian tạm ngưng khi đạt vị trí

    private Vector3 startPosition; // Vị trí ban đầu của vật thể
    private bool movingUp = true; // Biến kiểm tra hướng di chuyển

    void Start()
    {
        // Lưu vị trí ban đầu
        startPosition = transform.position;

        // Bắt đầu quá trình di chuyển
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true) // Lặp lại thao tác liên tục
        {
            // Di chuyển vật thể
            if (movingUp)
            {
                // Di chuyển lên
                while (transform.position.y < startPosition.y + moveDistance)
                {
                    transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                // Di chuyển xuống
                while (transform.position.y > startPosition.y)
                {
                    transform.position += Vector3.down * moveSpeed * Time.deltaTime;
                    yield return null;
                }
            }

            // Tạm dừng 2 giây
            yield return new WaitForSeconds(pauseDuration);

            // Đổi hướng
            movingUp = !movingUp;
        }
    }
}
