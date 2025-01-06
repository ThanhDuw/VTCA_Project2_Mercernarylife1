using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMove : MonoBehaviour
{
    public PlayerController controller;
    public Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    public GameObject shield;       // Khiên (GameObject)
    private Animator shieldAnimator; // Animator của khiên
    public bool isDefending = false; // Trạng thái phòng thủ
    public PlayerStats playerStats;

    
    void Start()
    {
        shield.SetActive(false);
        shieldAnimator = shield.GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        { 
            jump = true;
            animator.SetBool("IsJumping", true);
        }
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch= false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isDefending = true;
            shield.SetActive(true);
            if (shieldAnimator != null)
            {
                shieldAnimator.SetBool("IsOn", true); // Kích hoạt animation của khiên
                playerStats.UseShield();
            }

        }

        // Khi thả phím F, tắt phòng thủ
        if (Input.GetKeyUp(KeyCode.F))
        {
            isDefending = false;

            if (shieldAnimator != null)
            {
                shieldAnimator.SetBool("IsOn", false); // Ngưng animation của khiên
            }
            shield.SetActive(false);
            playerStats.EndShield();
        }
     
    }
    
    public void OnLanding()
    { 
        animator.SetBool("IsJumping", false);
    }
    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }
    void FixedUpdate()
    {
        //Character Move
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
