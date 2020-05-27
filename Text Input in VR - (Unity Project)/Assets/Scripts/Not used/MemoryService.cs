using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryService : MonoBehaviour
{

    private void Start()
    {
        Resources.UnloadUnusedAssets();
    }

    void Update()
    {
       //Resources.UnloadUnusedAssets();
    }
}
