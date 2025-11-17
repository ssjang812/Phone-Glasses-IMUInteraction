using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ListOfExhibitsScript : MonoBehaviour, UIControllerInterface
{
    public string MyURL { get; set; }
    public TemplateContainer MyRoot { get; set; }

    // 버튼에 매핑될 다음 페이지 경로
    private string roverInfoUxmlPath = "UXML/MainItems/ExhibitDetail/RoverInfo";
    private Button roverInfoButton;

    // TebbedMenu에서 메인 페이지에 갈아끼고 본 스크립트의 인스턴스를 생성 및 관리 해주므로 여기서는 내부 버튼 기능만 매핑해주면 됨
    public void Initialize(string url, TemplateContainer root)
    {
        MyURL = url;
        MyRoot = root;
        roverInfoButton = MyRoot.Q<Button>("RoverButton");
        roverInfoButton.RegisterCallback<ClickEvent>(OnRoverInfoButtonClicked);
    }

    private void OnRoverInfoButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnRoverInfoButtonButtonClicked");
        if (GlassesState.Instance == null) // 연동 안됬을때는 그냥 작동
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(roverInfoUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        } // 연동됬을때는 시선이 객체조작을 하지 않는 상태일때 작동
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(roverInfoUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        }
    }
}
