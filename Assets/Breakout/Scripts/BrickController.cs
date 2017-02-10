using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour {
    public int points = 10;

    void OnCollisionEnter(Collision c)
    {
        gameObject.SetActive(false);
        GameManager.BrickBroken(points);
    }
       
}
