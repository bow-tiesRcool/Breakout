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
    public float edgePadding = 0.1f;
    public float bottomPadding = 0.4f;
    BrickController[] brickArray;

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
        CreateBricks();
    }

    void CreateBricks()
    {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(edgePadding, bottomPadding, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1 - edgePadding, 1 - edgePadding, 0));
        bottomLeft.z = 0;
        float w = (topRight.x - bottomLeft.x) / (float)columns;
        float h = (topRight.y - bottomLeft.y) / (float)rows;


        brickArray = new BrickController[rows * columns];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                BrickController brick = Instantiate(brickPrefab) as BrickController;
                brick.transform.position = bottomLeft + new Vector3((col + 0.5f) * w, (row + 0.5f) * h, 0);
                brickArray[row * columns + col] = brick;
            }
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

        bool hasWon = true;
        for (int i = 0; i < instance.brickArray.Length; i++)
        {
            BrickController brick = instance.brickArray[i];
            if (brick.gameObject.activeSelf)
            {
                hasWon = false;
                break;
            }
        }
        if (instance.score > instance.highScore)
        {
            instance.highScore = points;
            instance.highScoreUI.text = "HighScore: " + instance.score;
        }
        if (hasWon)
        {
            instance.WinnerUI.text = "WINNER!";
            instance.WinnerUI.gameObject.SetActive(true);
        }
    }
}
