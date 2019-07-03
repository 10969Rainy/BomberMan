using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour {

    public float dUp;
    public float dRight;

    public bool enter;
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.W))    //按下键的时候
        {
            dUp = 1.0f;
        }
        else if (Input.GetKeyUp(KeyCode.W)) //松开键的时候
        {
            dUp = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            dUp = -1.0f;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            dUp = -0.5f;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            dRight = -1.0f;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            dRight = -0.5f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            dRight = 1.0f;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            dRight = 0.5f;
        }

        enter = Input.GetKeyDown(KeyCode.Space);
    }
}
