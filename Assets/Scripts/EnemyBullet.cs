using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
<<<<<<< Updated upstream
{
    [SerializeField] private float speed = 1f;
    private Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        // cho vien dan bien mat sau 3s
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        //lay animator

        // bay ngang theo van toc
        /*rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);// lay velocity cua no   */
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
    // cai dat van toc cho vien dan vaf phai la public
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
=======
{   
    public int damage = 10;
>>>>>>> Stashed changes
    //ham xu li va cham
    private void OnTriggerEnter2D(Collider2D collision)
    {
<<<<<<< Updated upstream
        Destroy(gameObject, 3);
=======
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(gameObject, 3);
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("BossBullet"))
        {
            Destroy(gameObject, 3);
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
>>>>>>> Stashed changes
    }
}