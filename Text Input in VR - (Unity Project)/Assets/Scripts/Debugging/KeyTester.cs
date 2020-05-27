using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTester : MonoBehaviour
{
    int leftcount = 0;
    int rightcount = 0;
    int upcount = 0;
    int downcount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (char key in Input.inputString)
        {
            Debug.Log("KeyTester - Input Char - " + key);
        }

        if (Input.GetKey(KeyCode.RightArrow))
            Debug.Log("KeyTester - RightArrow - " + rightcount++);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Debug.Log("KeyTester - LeftArrow - " + leftcount++);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Debug.Log("KeyTester - UpArrow - " + upcount++);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            Debug.Log("KeyTester - DownArrow - " + downcount++);
    }
}
