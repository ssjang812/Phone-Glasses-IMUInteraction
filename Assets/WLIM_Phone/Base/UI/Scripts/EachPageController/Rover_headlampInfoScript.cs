using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rover_headlampInfoScript : MonoBehaviour, UIControllerInterface
{
    public string MyURL { get; set; }
    public TemplateContainer MyRoot { get; set; }

    public void Initialize(string url, TemplateContainer root)
    {
        MyURL = url;
        MyRoot = root;
    }
}
