using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AndroidPlugin : MonoBehaviour
{
    private AndroidJavaClass AndroidPluginClass;
    private AndroidJavaObject _instance;
    TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        AndroidPluginClass = new AndroidJavaClass("com.example.mymodule.AndroidPlugin");
        _instance = AndroidPluginClass.CallStatic<AndroidJavaObject>("instance");

        _text = GetComponent<TextMeshProUGUI>();

        if (_instance != null)
        {
            _text.text = _instance.Call<string>("getPackageName");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void showToast(string message)
    {
        if (_instance != null)
        {
            _instance.Call("showToast", message);
        }
    }
}