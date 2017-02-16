using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForLoop : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		for ( int i = 0; i < 100; ++i)
        {
            Debug.Log(i);
        }
	}
	
}
