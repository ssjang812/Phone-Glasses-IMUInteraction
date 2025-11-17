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
        // 프레임 속도를 60으로 설정
        Application.targetFrameRate = 60;
    }
}
