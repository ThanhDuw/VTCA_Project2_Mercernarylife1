using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Rigidbody2D rigidbody2D;    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();        
        // cho vien dan bien mat sau 3s
        /*Destroy(gameObject, 3);*/
    }

    // Update is called once per frame
    void Update()
    {
        //lay animator

        // bay ngang theo van toc
        /*rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);// lay velocity cua no  */ 
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
    // cai dat van toc cho vien dan vaf phai la public
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    //ham xu li va cham
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }        
    } 
}
