<<<<<<< Updated upstream:Assets/Scripts/PlayerMove.cs
﻿using System.Collections;
=======
﻿using JetBrains.Annotations;
using System.Collections;
>>>>>>> Stashed changes:Assets/Scripts/Player/PlayerMovement.cs
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    //public Slider healthBar;
    private bool isFacingRight = true;
    [Header("Shooting")]
    public GameObject bulletPrefab;
   
    //[Header("Interaction")]
    //public float interactRange = 2f;
    //public LayerMask npcLayer;

    [Header("Animation")]
    private Animator animator;
    [SerializeField] private Rigidbody2D rb2D;
    private float horizontal;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        // Initialize health
        currentHealth = maxHealth;
        //healthBar.maxValue = maxHealth;
        //healthBar.value = currentHealth;
    }

    void Update()
    {


        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal > 0)
        {
            _spriteRenderer.flipX = false; // Quay mặt sang phải
            _animator.SetBool("IsWalking", true);
            isFacingRight = true;
        }
        else if (horizontal < 0)
        {
            _spriteRenderer.flipX = true; // Quay mặt sang trái
            _animator.SetBool("IsWalking", true);
            isFacingRight = false;
        }
        else
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetBool("IsIdle", true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
<<<<<<< Updated upstream:Assets/Scripts/PlayerMove.cs
            isGrounded = false;
            _animator.SetBool("IsJump", true);
        }


        if (Input.GetMouseButtonDown(0)) // Shoot
        {
            _animator.SetTrigger("Shoot");
            Vector2 spawnPosition = transform.position;
            //van toc vien dan
            float bulletSpeed = 10f;
            if (isFacingRight)
            {
                spawnPosition += new Vector2(1, 0);
                bulletSpeed = 10;
            }
            else
            {
                spawnPosition += new Vector2(-1, 0);
                bulletSpeed = -10;
=======
            _animator.SetBool("IsJumping", true); // Bắt đầu animation Jumping
            isGrounded = false; // Đặt trạng thái chưa chạm đất
        }

        //Khi chạm đất, chuyển về Idle
        if (isGrounded)
        {
            _animator.SetBool("IsJumping", false);

        }


        if (Input.GetMouseButtonDown(0)) // Shoot
        {
            Vector2 spwamPosition = transform.position;
            _animator.SetBool("IsShooting", true);
            //van toc vien dan
            float bombSpeed = 10f;
            if (isFacingRight)
            {
                spwamPosition += new Vector2(1, 0);
                bombSpeed = 10;
            }
            else
            {
                spwamPosition += new Vector2(-1, 0);
                bombSpeed = -10;
>>>>>>> Stashed changes:Assets/Scripts/Player/PlayerMovement.cs
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }

            GameObject bomb = Instantiate(bulletPrefab,
                                spawnPosition, Quaternion.identity);
            //lay component bomb
            Bullet bombComponent = bomb.GetComponent<Bullet>();

            bombComponent.setSpeed(bulletSpeed);
        }



        //if (Input.GetKeyDown(KeyCode.E)) // Interact with NPC
        //{
        //    InteractWithNPC();
        //}
    }

<<<<<<< Updated upstream:Assets/Scripts/PlayerMove.cs


=======
>>>>>>> Stashed changes:Assets/Scripts/Player/PlayerMovement.cs
    //void InteractWithNPC()
    //{
    //    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, npcLayer);

    //    foreach (var hit in hits)
    //    {
    //        if (hit.CompareTag("NPC"))
    //        {
    //            Debug.Log("Interacted with " + hit.name);
    //            hit.GetComponent<NpcInteraction>().Interact();
    //            break;
    //        }
    //    }
    //}

    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(horizontal * moveSpeed, rb2D.velocity.y);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
<<<<<<< Updated upstream:Assets/Scripts/PlayerMove.cs

        //if (collision.collider.CompareTag("Enemy"))
=======
        //if (collision.collider.CompareTag("EnemyBullet"))
>>>>>>> Stashed changes:Assets/Scripts/Player/PlayerMovement.cs
        //{
        //    TakeDamage(10);
        //}
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        // Add death animation or reload scene logic
        Destroy(gameObject);
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, interactRange);
    //}
}
