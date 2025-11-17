using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingScript : MonoBehaviour, UIControllerInterface
{
    public string MyURL { get; set; }
    public TemplateContainer MyRoot { get; set; }

    private string gazeInteractionModeUxmlPath = "UXML/SettingItems/GazeInteractionMode";
    private Button gazeInteractionModeButton;
    private Button webVizModeButton;
    public void Initialize(string url, TemplateContainer root)
    {
        MyURL = url;
        MyRoot = root;
        gazeInteractionModeButton = MyRoot.Q<Button>("GazeInteractionModeButton");
        gazeInteractionModeButton.RegisterCallback<ClickEvent>(OnGazeInteractionModeButtonClicked);
        gazeInteractionModeButton = MyRoot.Q<Button>("WebVizModeButton");
        gazeInteractionModeButton.RegisterCallback<ClickEvent>(OnWebVizModeButtonClicked);
    }
    private void OnGazeInteractionModeButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnGazeInteractionModeButtonClicked");
        if (GlassesState.Instance == null) // 글래스 연동 안됬을때는 그냥 작동
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(gazeInteractionModeUxmlPath, PageController.UICategory.Setting);
            gameObject.GetComponent<TabbedMenu>().ReflectToSettingContent(root);
        } // 글래스 연동 됬을때는 시선이 객체 컨트롤로 사용 안되고있을때만 작동
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(gazeInteractionModeUxmlPath, PageController.UICategory.Setting);
            gameObject.GetComponent<TabbedMenu>().ReflectToSettingContent(root);
        }
    }

    private void OnWebVizModeButtonClicked(ClickEvent evt)
    {
        // 이거 버튼과 페이지식으로 말고, 한 창에서 선택 할 수 있는 UI로 바꿔야 할듯..?
        // 일단 연동 되는걸 기본으로 해두고 테스트 완료 후 업그레이드 하자
    }
}
