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
    public GameObject playerInfoPanel; // Đối tượng panel để hiển thị thông tin
    public KeyCode toggleKey = KeyCode.E;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip pickUpClip;

    void Start()
    {
        shield.SetActive(false);
        shieldAnimator = shield.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

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
        if (Input.GetKeyDown(toggleKey))
        {
            // Chuyển trạng thái hiển thị panel (tắt/mở)
            bool isActive = playerInfoPanel.activeSelf;
            playerInfoPanel.SetActive(!isActive);
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Key")) // Kiểm tra xem đối tượng chạm có phải là Player
        {
            _audioSource.PlayOneShot(pickUpClip);
            
        }

    }

}
