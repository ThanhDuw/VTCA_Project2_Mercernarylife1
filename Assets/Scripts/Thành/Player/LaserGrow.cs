using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGrow : MonoBehaviour
{
    public int damagePerSecond = 10; // Lượng sát thương mỗi giây
    public int damageInterval = 0; // Thời gian giữa mỗi lần gây sát thương (0.2 giây)
    private LayerMask targetLayer;     // Lớp đối tượng bị ảnh hưởng (ví dụ: Enemy)
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        // Kiểm tra nếu đối tượng thuộc tag "Enemy"
        if (collision.CompareTag("Enemy"))
        {
            // Lấy EnemyHealth script từ đối tượng bị trúng
            EnemyMovement enemy = collision.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                // Gây sát thương định kỳ theo thời gian
                enemy.TakeDamage(damagePerSecond * damageInterval);
            }


        }
        if (collision.CompareTag("Boss"))
        {
            BossXanh boss= collision.GetComponent<BossXanh>();
            if( boss != null)
            {
                boss.currentHp -= damagePerSecond*damageInterval;
                boss.hpBar.SetHealth(boss.currentHp);
            }
        }
    }
   public void Damage()
    {
        float takeDamage=damageInterval*damagePerSecond;
    }
}
