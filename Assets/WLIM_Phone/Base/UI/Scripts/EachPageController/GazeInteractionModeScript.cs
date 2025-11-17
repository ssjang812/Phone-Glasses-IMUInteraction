using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GazeInteractionModeScript : MonoBehaviour, UIControllerInterface
{
    public string MyURL { get; set; }
    public TemplateContainer MyRoot { get; set; }

    private Button phoneSwipeModeButton;
    private Button phoneGyroModeButton;
    private Button glassesGyroModeButton;
    private PhoneTouchSender PhoneTouchSender;

    public void Initialize(string url, TemplateContainer root)
    {
        MyURL = url;
        MyRoot = root;
        PhoneTouchSender = GameObject.FindGameObjectWithTag("PUN2manager").GetComponent<PhoneTouchSender>();
        phoneSwipeModeButton = MyRoot.Q<Button>("PhoneSwipeModeButton");
        phoneSwipeModeButton.RegisterCallback<ClickEvent>(OnPhoneSwipeModeButtonClicked);
        phoneGyroModeButton = MyRoot.Q<Button>("PhoneGyroModeButton");
        phoneGyroModeButton.RegisterCallback<ClickEvent>(OnPhoneGyroModeButtonClicked);
        glassesGyroModeButton = MyRoot.Q<Button>("GlassesGyroModeButton");
        glassesGyroModeButton.RegisterCallback<ClickEvent>(OnGlassesGyroModeButtonClicked);  
    }
    private void OnPhoneSwipeModeButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnPhoneSwipeModeButtonClicked");
        if (GlassesState.Instance == null) // 연동 안됬을때는 RPC 보내지 않음 그냥 작동
        {
        }
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            PhoneTouchSender.OnPhoneSwipeButtonClick();
        }
    }
    private void OnPhoneGyroModeButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnPhoneGyroModeButtonClicked");
        if (GlassesState.Instance == null) // 연동 안됬을때는 RPC 보내지 않음 그냥 작동
        {
        }
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            PhoneTouchSender.OnPhoneGyroButtonClick();
        }
    }
    private void OnGlassesGyroModeButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnGlassesGyroModeButtonClicked");
        if (GlassesState.Instance == null) // 연동 안됬을때는 RPC 보내지 않음 그냥 작동
        {
        }
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            PhoneTouchSender.OnGlassesGyroButtonClick();
        }
    }
}
