using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

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
    public int level = 1;
    public int currentXP = 0;
    public int XPToLevelUp = 100;

    public Text healthText;
    public Text manaText;
    public Text levelText;
    public Text xpText;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentMP = maxMP;
        manaBar.SetMaxMP(maxMP);

        gameOver.SetActive(false);
        UpdateUI();
    }
    //private void OnEnable()
    //{
    //    if (ExpManager.Instance != null)
    //    {
    //        ExpManager.Instance.OnExpChange += HandleExperienceChange;
    //    }
    //    else
    //    {
    //        Debug.LogError("ExpManager.Instance is null! Ensure that ExpManager is properly initialized.");
    //    }
    //}
    //private void OnDisable()
    //{
    //    ExpManager.Instance.OnExpChange -= HandleExperienceChange;
    //}
    //private void HandleExperienceChange(int newExp)
    //{
    //    currentExp += newExp;
    //    if(currentExp >= maxExp)
    //    {
    //        LevelUp();
    //    }
    //}
    //private void LevelUp()
    //{
    //    Bullet bullet = GetComponent<Bullet>();
    //    bullet.LevelUp();

    //    maxMP += 10;
    //    currentMP= maxMP;

    //    maxHealth += 20;
    //    currentHealth = maxHealth;

    //    currentLevel++;
    //    currentExp = 0;
    //    maxExp += 100;
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
    public void AddXP(int amount)
    {
        currentXP += amount;

        if (currentXP >= XPToLevelUp)
        {
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        level++;
        currentXP -= XPToLevelUp;
        XPToLevelUp += 100;  // Tùy chọn: Tăng XP yêu cầu để lên cấp tiếp theo
        // Bạn có thể tăng thuộc tính ở đây (sức khỏe, sát thương, v.v.)
        maxHealth += 20;
        currentHealth = maxHealth;
        maxMP += 10;
        currentMP = maxMP;

        //Bullet bulletDamage = GetComponent<Bullet>();
        //bulletDamage.LevelUp();

        Debug.Log("Level Up! New Level: " + level);
    }

    void UpdateUI()
    {
        if (healthText != null)
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
        if (manaText != null)
            manaText.text = $"Mana: {currentMP}/{maxMP}";
        if (levelText != null)
            levelText.text = "Level: " + level;
        if (xpText != null)
            xpText.text = "XP: " + currentXP + "/" + XPToLevelUp;
        //Bullet bulletDamage = GetComponent<Bullet>();
        //if (damageText != null && bulletDamage != null)
        //    damageText.text = "Damage: " + bulletDamage.damage;
    }
}
