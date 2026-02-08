using System.Drawing;
using UnityEngine;
using System.Collections;

public class DragonController : MonoBehaviour
{
    public float distance = 3f;
    public float movement = .25f;
    public float speed;
    public float duration;
    public float distanceCheck = 2f;
    public float HoridistanceCheck = .5f;
    public LayerMask ground;
    public int health = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("MoveObject");

    }

    // Update is called once per frame
    void Update()
    {
        //float t = Mathf.PingPong(Time.time * speed, 1f);
        //transform.position = Vector2.Lerp(pointA, pointB, t);
    }
    
    IEnumerator MoveObject()
    {
        while (true)
        {
            transform.Translate(new Vector2(distance * movement, 0));
            
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, distanceCheck, ground);
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, distanceCheck, ground);
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, distanceCheck, ground);
            Debug.DrawRay(transform.position, Vector2.down * distanceCheck, UnityEngine.Color.red, 1f);
            Debug.DrawRay(transform.position, Vector2.left * HoridistanceCheck, UnityEngine.Color.red, 1f);
            Debug.DrawRay(transform.position, Vector2.right * HoridistanceCheck, UnityEngine.Color.red, 1f);

            if(!hitDown || hitLeft|| hitRight)
            {
                movement *= -1;
                
            }
            yield return new WaitForSeconds(.25f);
        }
        
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("fire"))
        {
            health -= 25;

            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
