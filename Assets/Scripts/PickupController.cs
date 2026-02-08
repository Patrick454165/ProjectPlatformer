using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PickupController : MonoBehaviour
{

    public float distance = 3f;
    public float movement = .25f;
    private int counter = 5;
    private int counterDef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        counterDef=counter;
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
            if(counter <= 0)
            {
                movement *= -1;
                counter = counterDef;
            }
            yield return new WaitForSeconds(.1f);
        }
        
        
    }
}
