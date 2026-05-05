using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score;

    void Awake()
    {
        // Singleton logic
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        resetScore();
        DontDestroyOnLoad(gameObject); // optional
    }

    private void resetScore()
    {
        score = 0;
    }

    public void IncrementScoreBy(int value)
    {
        score += value;
        Debug.Log("Current score: " + score);
    }

    public int GetScore()
    {
        return score;
        
    }
}