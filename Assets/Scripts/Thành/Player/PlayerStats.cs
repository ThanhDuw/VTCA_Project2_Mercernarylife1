using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Player_Hpbar healthBar;

    public Animator animator;

    public GameObject gameOver;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            TakeDamage(20);
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
