using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Skill : MonoBehaviour
{
    public GameObject laserPrefab;     // Prefab cho laser
    public Transform firePoint;       // Điểm bắn laser (vị trí trên Player)
    public float laserSpeed = 10f;    // Tốc độ di chuyển của laser
    public float laserLifetime = 2f;  // Thời gian tồn tại của laser
    private float laserCooldown = 20f; // Thời gian hồi chiêu giữa các lần bắn
    private float nextFireTime = 0f;  // Thời điểm có thể bắn lần tiếp theo
    public Animator animator;
    void Update()
    {
        // Kiểm tra nếu nhấn phím bắn (thay "Fire1" bằng phím bạn muốn)
        if (Input.GetButtonDown("Fire2") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + laserCooldown;
            animator.SetBool("IsShooting", true);

            FireLaser();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            animator.SetBool("IsShooting", false);
        }

    }

    void FireLaser()
    {
        // Tạo laser từ prefab tại vị trí firePoint
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);

        // Di chuyển laser theo hướng firePoint
        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * laserSpeed;
        }

        // Hủy laser sau một khoảng thời gian
        Destroy(laser, laserLifetime);
    }
}
