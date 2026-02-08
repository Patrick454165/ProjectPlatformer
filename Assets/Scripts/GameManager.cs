using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private int lives = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void DecreaseLives()
    {
        lives--;
    }
    public int GetLives()
    {
        return lives;
    }
}
