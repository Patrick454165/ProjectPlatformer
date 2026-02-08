using System.Drawing;
using UnityEngine;
using System.Collections;

public class BatScript : MonoBehaviour
{
    public AudioSource audioSource;
    public float distance = 3f;
    public float movement = .25f;
    public float speed;
    public LayerMask ground;
    public int health = 100;
    public int moveBounds = 3;
    public int counter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("MoveObject");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator MoveObject()
    {
        while (true)
        {
            transform.Translate(new Vector2(0, distance * movement));
            
            counter--;
            

            if(counter<=0)
            {
                movement *= -1;
                counter=moveBounds;
                
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
                gameObject.transform.Translate(0, -999, 0);
                audioSource.Play();
                Invoke("Despawn", 2f);
            }
        }
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}
