using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour {
    
    void OnCollisionEnter(Collision c)
    {
        gameObject.SetActive(false);
    }
       
}
