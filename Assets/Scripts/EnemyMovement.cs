using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    private Rigidbody2D rb2D;
    [SerializeField] private float boundary;
    private float leftBoundary, rightBoundary;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private float hp = 30;

    public int ID { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        leftBoundary = transform.position.x - boundary;
        rightBoundary = transform.position.x + boundary;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        ID = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= leftBoundary)//boundary la moc de cho enemy quay dau lai
        {
            speed = Mathf.Abs(speed);
            animator.SetBool("Walk", true);
            // xoay mat
            spriteRenderer.flipX = false;

        }
        else if (transform.position.x >= rightBoundary)
        {
            speed = -Mathf.Abs(speed);
            animator.SetBool("Walk", true);
            // xoay mat
            spriteRenderer.flipX = true;
        }
        //var leftBoundary = transform.position.x - boundary;
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isAttacking", true);
            //neu nhan vat cham vao phia sau thi doi huong
            //vi tri cua nhan vat va vi tri cua boss
            var playerPositison = collision.gameObject.transform.position;// day la 2 ham khai bao vi tri cua boss va player    
            var bossPosition = transform.position;//
            if (speed > 0 && playerPositison.x < bossPosition.x)
            {
                speed = -speed;
            }
            else if (speed < 0 && playerPositison.x > bossPosition.x)
            {
                speed = -speed;
            }
        }
        //// neu quai cham phai vien dan se chet
        //if (collision.gameObject.CompareTag("Bullet"))
        //{
        //    //tru hp
        //    if (hp <= 0)
        //    {
        //        //hieu ung khi boss bi trung dan                               
        //        Destroy(gameObject);
        //    }
        //    else
        //    {
        //        hp -= 10;//so mau boss mat khi bi danh trung               
        //    }
        //}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // neu nhan vat chay ra khoi pham vi tan cong thi vao lai trang thai di chuyen
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isAttacking", false);
        }
    }
    public void TakeDamage(float damage)
    {
        hp -= damage; 
        if (hp <= 0)
        {
            Die(); 
        }
    }
    private void Die()
    {
        Destroy(gameObject); 
    }
}