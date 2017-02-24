using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public BrickController brickPrefab;
    public BrickController brick1Prefab;
    public BrickController brick2Prefab;
    public BrickController brick3Prefab;
    public BrickController brick4Prefab;
    public GameObject[] powerUps;
    [Range(0, 1)]public float powerUpChance = 0.1f;
    public int rows = 5;
    public int columns = 10;
    public float edgePadding = 0.1f;
    public float bottomPadding = 0.4f;
    List<BrickController> brickList = new List<BrickController>();
    public ParticleSystem pointParticle;
    public AudioSource sound;
    public AudioClip music;
    public AudioClip gameOver;
    public AudioClip winner;
    public Text livesUI;
    public Text scoreUI;
    public Text highScoreUI;
    public Text gameOverUI;
    public Text WinnerUI;
    public int level = 1;
    public int leveltype;
    
    
    public int score = 0;
    public int lives = 3;
    public int highScore;
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
        instance.highScoreUI.text = "HighScore: " + PlayerPrefs.GetInt("highScore");
        sound = GetComponent<AudioSource>();
        livesUI.text = "Lives: " + lives;
        scoreUI.text = "Score: " + score;
        instance.sound.clip = instance.music;
        instance.sound.Play();
        CreateBricks();
    }

    private void Update()
    {
        HighScore();
    }

    void CreateBricks()
    {
        if (leveltype == 0)
        {
            brickPrefab = brick1Prefab;
        }
        else if (leveltype == 1)
        {
            brickPrefab = brick2Prefab;
            instance.rows = Random.Range(3, 6);
            instance.columns = Random.Range(5, 10);
        }
        else if (leveltype == 2)
        {
            brickPrefab = brick3Prefab;
            instance.rows = Random.Range(3, 6);
            instance.columns = Random.Range(5, 10);
        }
        else
        {
            brickPrefab = brick4Prefab;
            instance.rows = 1;
            instance.columns = 1;
        }

        for (int i = brickList.Count - 1; i >= 0; i--)
        {
            Destroy(brickList[i].gameObject);
        }
        brickList.Clear();

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(edgePadding, bottomPadding, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1 - edgePadding, 1 - edgePadding, 0));
        bottomLeft.z = 0;
        float w = (topRight.x - bottomLeft.x) / (float)columns;
        float h = (topRight.y - bottomLeft.y) / (float)rows;

        for (int row = 0; row < instance.rows; row++)
        {
            for (int col = 0; col < instance.columns; col++)
            {
                BrickController brick = Instantiate(brickPrefab) as BrickController;
                brick.transform.position = bottomLeft + new Vector3((col + 0.5f) * w, (row + 0.5f) * h, 0);
                brickList.Add(brick);
            }
        }
    }

    public static void LostBall ()
    {
        instance.lives = instance.lives - 1;
        instance.livesUI.text = "Lives: " + instance.lives;

        if (instance.lives <= 0)
        {
            instance.sound.Stop();
            instance.sound.clip = instance.gameOver;
            instance.sound.Play();
            instance.gameOverUI.text = "Game Over";
            instance.gameOverUI.gameObject.SetActive(true);
            HighScoreSaver();
        }
        else
        {
            instance.livesUI.text = "Lives: " + instance.lives;
        }
    }

    public static void BrickBroken(int points)
    {
        instance.score += points;
        instance.pointParticle.Play();
        instance.scoreUI.text = "Score: " + instance.score;
        
        if (Random.value < instance.powerUpChance)
        {
            instance.DropPowerUp();
        }

        bool hasWon = true;
    
        for (int i = 0; i < instance.brickList.Count; i++)
        {
            BrickController brick = instance.brickList[i];
            if (brick.gameObject.activeSelf)
            {
                hasWon = false;
                break;
            }
        }

        if (hasWon)
        {
            instance.sound.Stop();
            instance.sound.clip = instance.winner;
            instance.sound.Play();
            instance.level++;
            instance.leveltype = instance.level % 4;
            instance.CreateBricks();
        }
    }
    public static void HighScore()
    {
        if (instance.score > instance.highScore)
        {
            instance.highScore = instance.score;
        }
        if (instance.highScore > PlayerPrefs.GetInt("highScore"))
        {
            instance.highScoreUI.text = "highScore: " + instance.highScore;
        }
    }
    public static void HighScoreSaver()
    {
        if (PlayerPrefs.HasKey("highScore") == true)
        {
            if (instance.highScore > PlayerPrefs.GetInt("highScore"))
            {
                int newHighScore = instance.highScore;
                PlayerPrefs.SetInt("highScore", newHighScore);
                PlayerPrefs.Save();
            }
        }
        else
        {
            int newHighScore = instance.highScore;
            PlayerPrefs.SetInt("highScore", newHighScore);
            PlayerPrefs.Save();
        }
    }
void DropPowerUp()
    {
        GameObject power = Instantiate(powerUps[Random.Range(0,powerUps.Length)]);
        power.transform.position = GameObject.FindGameObjectWithTag("Ball").transform.position;
    }

    public static void AddLife()
    {
        instance.lives = instance.lives + 1;
        instance.livesUI.text = "Lives: " + instance.lives;
    }
}

