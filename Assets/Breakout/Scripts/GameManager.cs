using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BrickController brickPrefab;
    public int rows = 5;
    public int columns = 10;
    public Text livesUI;
    public Text scoreUI;
    public Text highScoreUI;
    public Text gameOverUI;
    public Text WinnerUI;
    
    public int score = 0;
    public int lives = 3;
    public int highScore = 0;
    private int countBricks;

    private GameObject[] getCount;
   
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
        CountBricks();
        CreateBricks();
    }

    void CreateBricks()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                BrickController brick = Instantiate(brickPrefab) as BrickController;
                brick.transform.position = new Vector3(col + 1, row + 1, 0);
            }
        }
    }

    private void Update()
    {
        if (instance.countBricks == 0)
        {
            instance.WinnerUI.text = "WINNER!";
            instance.WinnerUI.gameObject.SetActive(true);
        }
    }

    public static void LostBall ()
    {
        instance.lives = instance.lives - 1;
        instance.livesUI.text = "Lives: " + instance.lives;

        if (instance.lives <= 0)
        {
            instance.gameOverUI.text = "Game Over";
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
        CountBricks();
    }
    
    public static void CountBricks ()
    {
        instance.getCount = GameObject.FindGameObjectsWithTag("Brick");
        instance.countBricks = instance.getCount.Length;
        Debug.Log(instance.countBricks + "Bricks");

    }
}
