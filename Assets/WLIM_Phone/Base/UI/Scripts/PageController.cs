using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using static CrossDeviceState;

public class PageController : MonoBehaviour
{
    public static PageController Instance { get; private set; }

    //event_onSelObjForWebview 발생시 로드할 경로
    private string rover_antennaInfoUxmlPath = "UXML/MainItems/ExhibitDetail/RoverDetail/Rover_antennaInfo";
    private string rover_headlampInfoUxmlPath = "UXML/MainItems/ExhibitDetail/RoverDetail/Rover_headlampInfo";
    private string rover_solarpanelnfoUxmlPath = "UXML/MainItems/ExhibitDetail/RoverDetail/Rover_solarpanelInfo";
    private TabbedMenu tabbedMenu;

    // 뒤로가기 구현을 위한 스택
    private Stack<UIControllerInterface> UIStack = new Stack<UIControllerInterface>();
    private Stack<UIControllerInterface> SettingStack = new Stack<UIControllerInterface>();

    public enum UICategory
    {
        UI,
        Setting
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
        }
    }

    private void Start()
    {
        tabbedMenu = GetComponent<TabbedMenu>();
        // 스마트글래스에서의 페이지 생성 트리거 객체 선택시 사용될 함수
        // Phone 이벤트에 따라 Glasses UI가 생성되는 부분은 동적으로 되게 바꿔야함

        // 동적으로 생성되는 GlassState 함수로 인해 해당 함수는 GlassState가 생성되는 시점에 외부에서 수행, 정적으로 수행할때는 간단히 아래 방식으로
        // GlassesState.event_onSelObjForWebview.AddListener(LoadNewPageFromGlasses);
    }

    // UI로드
    public TemplateContainer LoadNewPage(string path, UICategory category)
    {
        //Debug.Log("PageController:LoadNewPage - " + path);
        VisualTreeAsset uxml = Resources.Load<VisualTreeAsset>(path);
        if (uxml != null)
        {
            TemplateContainer root = uxml.CloneTree();
            SetNewPage(path, root, uxml.name, category);
            return root;
        }
        else
        {
            Debug.LogError("Page Asset not found at path: " + path);
            return null;
        }
    }

    // UI별 기능 매핑(UI와 동일한 이름의 스크립트를 인스턴스화, Initialize를 실행해 기능 매핑과 stack에 보관될시를 위해 TemplateContainer 정보 저장)
    private void SetNewPage(string path, TemplateContainer root, string uxmlName, UICategory category)
    {
        string scriptName = $"{typeof(PageController).Namespace}.{uxmlName}Script";
        Type scriptType = Type.GetType(scriptName);

        if (scriptType != null && typeof(UIControllerInterface).IsAssignableFrom(scriptType))
        {
            UIControllerInterface UIController = gameObject.AddComponent(scriptType) as UIControllerInterface;
            if (UIController != null)
            {
                UIController.Initialize(path, root);
            }
            else
            {
                Debug.LogError("UIController is null" + scriptName);
            }

            //ToDo, 현재 활성화 되어있는 UI에 해당하는 컨트롤러만 살리고 나머지는 다 disable해둬야 입력에 대해 처리가 겹치지 않을듯

            if (UIController != null)
            {
                if (category == UICategory.UI)
                {
                    // 이전 UI가 있을시 직전 UI의 코드를 비활성화
                    if (UIStack.Count > 0)
                    {
                        UIControllerInterface topUI = UIStack.Peek();
                        MonoBehaviour monoBehaviour = topUI as MonoBehaviour;
                        if (monoBehaviour != null)
                        {
                            monoBehaviour.enabled = false;
                        }
                    }
                    UIStack.Push((UIControllerInterface)UIController);
                    //Debug.Log("SetNewPage UIStack: " + UIStack.Count);
                }
                else if(category == UICategory.Setting)
                {
                    if (SettingStack.Count > 0)
                    {
                        UIControllerInterface topUI = SettingStack.Peek();
                        MonoBehaviour monoBehaviour = topUI as MonoBehaviour;
                        if (monoBehaviour != null)
                        {
                            monoBehaviour.enabled = false;
                        }
                    }
                    SettingStack.Push((UIControllerInterface)UIController);
                    //Debug.Log("SetNewPage SettingStack: " + SettingStack.Count);
                }           
            }
            else
            {
                Debug.LogError("Failed to add script: " + scriptName);
            }
        }
        else
        {
            Debug.LogError("해당 UXML을 위한 컨트롤러가 Script가 정의되지 않았거나 UIControllerInterface를 상속하지 않았습니다.");
        }
    }

    //처음 만들때는 UI먼저 붙히고 기능을 붙히지만, 이미 만들걸 불러낼때는 꺼놓은 컴포넌트를 먼저 활성화시키고 거기에 저장되있는 경로의 UI를 불러옴
    public TemplateContainer LoadPrevPage(UICategory category)
    {
        UIControllerInterface UIController;
        if (category == UICategory.UI)
        {
            if (UIStack.Count > 1)
            {
                UIController = UIStack.Peek();
                MonoBehaviour monoBehaviour;
                monoBehaviour = UIController as MonoBehaviour;
                Destroy(monoBehaviour);
                UIStack.Pop();
                UIController = UIStack.Peek();
                monoBehaviour = UIController as MonoBehaviour;
                monoBehaviour.enabled = true;
                //Debug.Log("LoadPrevPage UIStack: " + UIStack);
                return UIController.MyRoot;  
            }
            return null;
        }
        else if (category == UICategory.Setting)
        {
            if (SettingStack.Count > 1)
            {
                UIController = SettingStack.Peek();
                MonoBehaviour monoBehaviour;
                monoBehaviour = UIController as MonoBehaviour;
                Destroy(monoBehaviour);
                SettingStack.Pop();
                UIController = SettingStack.Peek();
                monoBehaviour = UIController as MonoBehaviour;
                monoBehaviour.enabled = true;
                //Debug.Log("LoadPrevPage SettingStack: " + SettingStack);
                return UIController.MyRoot;
            }
            return null;
        }
        else
            return null;
    }

    public void LoadNewPageFromGlasses()
    {
        string url;
        switch (GlassesState.Instance.SelObjForWebview_Gla)
        {
            case Obj_Gla.Antenna:
                url = rover_antennaInfoUxmlPath;
                break;
            case Obj_Gla.Headlamp:
                url = rover_headlampInfoUxmlPath;
                break;
            case Obj_Gla.Solarpanel:
                url = rover_solarpanelnfoUxmlPath;
                break;
            default:
                url = "";
                break;
        }
        // 이전 UI가 있을시 새로 요청받은 페이지와 중복되는지 체크
        if (UIStack.Count > 0)
        {
            UIControllerInterface topUI = UIStack.Peek();
            if (url == topUI.MyURL)
                return;
        }
        if(url != "")
        {
            tabbedMenu.ReflectToMainContent(LoadNewPage(url, PageController.UICategory.UI));
        }
    }
}
