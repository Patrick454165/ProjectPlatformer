using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireController : MonoBehaviour
{

    private Rigidbody2D rb;
    public float bulletForce;
    public GameObject player;
    public Vector2 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        direction = player.GetComponent<PlayerController>().getDirection();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * bulletForce);
        Invoke("Die", 3f);
        
    }

    void Die()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = direction * bulletForce;
    }

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            Destroy(gameObject);
        }
    }
}
