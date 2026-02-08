using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BombController : MonoBehaviour
{

    public TextMeshProUGUI timerText;
    
    public int timer;
    public PlayerController player;
    public Boolean Disarmed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("BombTimer");
    }

    // Update is called once per frame
    
    
    IEnumerator BombTimer()
    {
        while (!Disarmed)
        {
            timer--;

            if(timer < 0)
            {
                player.Death();
            }
            else
            {
                if(timer >= 10)
                {
                    timerText.text = "00 : " + timer;
                }
                else
                {
                    timerText.text = "00 : 0" + timer;
                }
                
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void Disarm()
    {
        Disarmed = true;
        timerText.color = Color.green;
        player.Reset();
    }
}
