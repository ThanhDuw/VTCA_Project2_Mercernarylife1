using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float speed = 2f;       // Tốc độ bay lên
    public float maxHeight = 10f; // Chiều cao tối đa thang có thể bay
    private bool isActivated = false; // Trạng thái kích hoạt
    private bool isReturning = false; // Trạng thái thang quay về vị trí
    private Vector3 startPosition; // Vị trí ban đầu của thang

    void Start()
    {
        // Lưu lại vị trí ban đầu của thang
        startPosition = transform.position;
    }

    void Update()
    {
        // Nếu được kích hoạt, di chuyển thang lên
        if (isActivated && transform.position.y < startPosition.y + maxHeight)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        // Quay về
        if (isReturning && transform.position.y > startPosition.y)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);

            // Đảm bảo thang không đi quá thấp
            if (transform.position.y <= startPosition.y)
            {
                transform.position = startPosition;
                isReturning = false; // Ngừng quay về
            }
        }
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isActivated = true;
            isReturning = false;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        // Tùy chọn: Ngừng di chuyển khi người chơi rời thang
        if (collision.gameObject.CompareTag("Player"))
        {
            isActivated = false;
            isReturning = true;
        }
    }
    
}
