using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public int lives = 3;
    void Awake()
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
    
    public static void LostBall()
    {
        instance.lives = instance.lives - 1;
    }
}
