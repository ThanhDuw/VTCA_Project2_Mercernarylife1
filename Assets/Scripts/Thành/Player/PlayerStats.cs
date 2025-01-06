using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public float maxMP = 100;
    public float currentMP;
    public float manaDrainRate = 1f;

    private Coroutine manaCoroutine;

    public Player_Hpbar healthBar;
    public MPBar manaBar;

    public Animator animator;

    public GameObject gameOver;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentMP = maxMP;
        manaBar.SetMaxMP(maxMP);

        gameOver.SetActive(false);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.G))
    //    {
    //        TakeDamage(20);
    //    }
    //}
    public void UseShield()
    {
        if (manaCoroutine == null)  // Nếu chưa có Coroutine nào đang chạy
        {
            manaCoroutine = StartCoroutine(DrainMana());  // Bắt đầu Coroutine
        }
    }
    public void EndShield()
    {
        if (manaCoroutine != null)  // Nếu có Coroutine đang chạy
        {
            StopCoroutine(manaCoroutine);  // Dừng Coroutine
            manaCoroutine = null;  // Đặt lại biến Coroutine
        }
    }

    IEnumerator DrainMana()
    {
        while (currentMP > 0)
        {
            currentMP-= manaDrainRate;
            if (currentMP < 0) currentMP = 0;
            manaBar.SetMP(currentMP);
            yield return new WaitForSeconds(0.2f);  // Trừ mana mỗi giây
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        animator.SetTrigger("PlayerHit");
        if (currentHealth <= 0)
        {
            Die();
            gameOver.SetActive(true);
            Time.timeScale = 0;
        }
    }
    private void Die()
    {
        
        Destroy(gameObject);
    }
}
