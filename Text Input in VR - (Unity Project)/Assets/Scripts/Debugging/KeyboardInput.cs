using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyboardInput : MonoBehaviour
{
    string[] keys = { "backspace", "delete", "tab",
                         "clear",
                         "return",
                         "pause",
                         "escape",
                         "space",
                         "up",
                         "down",
                         "right",
                         "left",
                         "insert",
                         "home",
                         "end",
                         "page up",
                         "page down",
                         "f1",
                         "f2",
                         "f3",
                         "f4",
                         "f5",
                         "f6",
                         "f7",
                         "f8",
                         "f9",
                         "f10",
                         "f11",
                         "f12",
                         "f13",
                         "f14",
                         "f15",
                         "numlock",
                         "caps lock",
                         "scroll lock",
                         "right shift",
                         "left shift",
                         "right ctrl",
                         "left ctrl",
                         "right alt",
                         "left alt",
                            "0",
                             "1",
                             "2",
                             "3",
                             "4",
                             "5",
                             "6",
                             "7",
                             "8",
                             "9",
                             "!",
                             "\"",
                             "#",
                             "$",
                             "&",
                             "'",
                             "(",
                             ")",
                             "*",
                             "+",
                             ",",
                             "-",
                             ".",
                             "/",
                             ":",
                             ";",
                             "<",
                             "=",
                             ">",
                             "?",
                             "@",
                             "[",
                             "\\",
                             "]",
                             "^",
                             "_",
                             "`",
                             "a",
                             "b",
                             "c",
                             "d",
                             "e",
                             "f",
                             "g",
                             "h",
                             "i",
                             "j",
                             "k",
                             "l",
                             "m",
                             "n",
                             "o",
                             "p",
                             "q",
                             "r",
                             "s",
                             "t",
                             "u",
                             "v",
                             "w",
                             "x",
                             "y",
                             "z" };

    string inputString;

    public Text text;

    private float cursorTimestamp;
    private float downTimestamp;
    private float firstTimestamp;
    private bool cursor = false;
    private string cursorChar = "";
    private int caretPos;

    private void Start()
    {
        inputString = text.text;
        caretPos = inputString.Length;
    }

    void Update()
    {
        foreach (string key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                firstTimestamp = Time.time;
                if (key == "enter" || key == "return")
                {
                    inputString += "\n";
                }
                else if (key == "backspace" || key == "delete")
                {
                    if (inputString.Length != 0)
                    {
                        inputString = inputString.Substring(0, inputString.Length - 1);
                    }
                }
                else if (key == "space")
                {
                    inputString += " ";
                }
                else if (key.Length == 1)
                {
                    inputString += key;
                }
                else if (key == "left")
                {
                    caretPos = (inputString.Length - caretPos) < 99 ? caretPos - 1 : 0;
                }
                else if (key == "right")
                {
                    caretPos = (inputString.Length - caretPos) > 1 ? caretPos + 1 : inputString.Length;
                }
            }
            else if (Input.GetKey(key))
            {
                if (Time.time - firstTimestamp >= 0.5)
                {
                    if (Time.time - downTimestamp >= 0.08)
                    {
                        if (key == "enter" || key == "return")
                        {
                            inputString += "\n";
                        }
                        else if (key == "backspace" || key == "delete")
                        {
                            if (inputString.Length != 0)
                            {
                                inputString = inputString.Substring(0, inputString.Length - 1);
                            }
                        }
                        else if (key == "space")
                        {
                            inputString += " ";
                        }
                        else if (key.Length == 1)
                        {
                            inputString += key;
                        }
                        else if (key == "left")
                        {
                            caretPos = (inputString.Length - caretPos) < 99 ? caretPos - 1 : 0;
                        }
                        else if (key == "right")
                        {
                            caretPos = (inputString.Length - caretPos) > 1 ? caretPos + 1 : inputString.Length;
                        }
                        downTimestamp = Time.time;
                    }
                }
            }
        }

        if (Time.time - cursorTimestamp >= 0.5)
        {
            cursorTimestamp = Time.time; // Reset the time stamp
            if (cursor == false)
            { //If the cursor is off, enable it
                cursor = true;
                cursorChar += "|";
                inputString = inputString.Insert(caretPos, cursorChar);
            }
            else
            {
                cursor = false;
                if (cursorChar.Length != 0)
                    cursorChar = "";
                inputString = inputString.Replace("|", cursorChar);
                //cursorChar = cursorChar.Substring(0, cursorChar.Length - 1); //Remove the cursor character. cursorChar = ""; would also work
            }
            //inputString = inputString.Insert(caretPos, cursorChar);
        }
        //inputString = inputString.Substring(0, caretPos) + cursorChar;
        text.text = inputString;
        //text.text.Insert(caretPos, cursorChar);
        //text.fontSize = 20;
    }
}