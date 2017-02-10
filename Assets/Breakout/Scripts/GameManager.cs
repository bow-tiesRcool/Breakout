using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text livesUI;
    public Text scoreUI;
    public Text highScoreUI;
    public Text gameOverUI;

    public int score = 0;
    public int lives = 3;
    public int highScore = 0;

    void Awake ()
    {
        if (instance == null)
        {
            Debug.Log("yeas!");
            instance = this; 
        }
        else
        {
            Debug.Log("Nooo");
        }
    }
    void Start ()
    {
        livesUI.text = "Lives: " + lives;
        scoreUI.text = "Score: " + score;
        highScoreUI.text = "HighScore: " + highScore;
    } 
    public static void LostBall ()
    {
        instance.lives = instance.lives - 1;
        instance.livesUI.text = "Lives: " + instance.lives;

        if (instance.lives <= 0)
        {
            instance.gameOverUI.text = "You Lose";
            instance.gameOverUI.gameObject.SetActive(true);
        }
        else
        {
            instance.livesUI.text = "Lives: " + instance.lives;
        }
    }
    
    public static void BrickBroken (int points)
    {
        instance.score += points;
        instance.scoreUI.text = "Score: " + instance.score;

        if (instance.score > instance.highScore)
        {
            instance.highScore = points;
            instance.highScoreUI.text = "HighScore: " + instance.score;
        }
    }
}
