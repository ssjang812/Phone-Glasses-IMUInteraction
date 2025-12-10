using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitializeApp();
    }

    void InitializeApp()
    {
        // Set frame speed to 60 FPS
        Application.targetFrameRate = 60;
    }
}
