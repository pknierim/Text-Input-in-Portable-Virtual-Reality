using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardwareController : MonoBehaviour
{
    public Texture2D cursorTexture;

    void Start()
    {
        Cursor.visible = false;
        if (Application.isMobilePlatform){
            Texture2D cursorTexture = new Texture2D(1, 1);
            cursorTexture.SetPixel(0, 0, Color.clear);
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }

    private void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}
