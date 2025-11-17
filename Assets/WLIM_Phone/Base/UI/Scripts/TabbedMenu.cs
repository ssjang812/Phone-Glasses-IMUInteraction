using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TabbedMenu : MonoBehaviour
{
    public static TabbedMenu Instance { get; private set; }
    //앱시작 페이지에 로드할 경로
    private string mainUxmlPath = "UXML/Main";
    private string settingUxmlPath = "UXML/Setting";

    //이벤트 처리 모듈화 
    private TabbedMenuController tabbedMenuController;
    private PageController pageController;
    //탭 전환에 따라 보여질 메인페이지와 셋팅페이지, 메인페이지의 경우 이벤트에 따라 로드되는 페이지가 달라짐
    private VisualElement mainContent;
    private VisualElement settingContent;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // 탭 메뉴에 따른 화면 전환이 작동하도록 컨트롤러 등록
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;

            tabbedMenuController = new TabbedMenuController(root);
            pageController = gameObject.AddComponent<PageController>();

            tabbedMenuController.RegisterTabCallbacks();

            // 메인 화면에 띄울 컨텐츠 로드
            mainContent = root.Q<VisualElement>("mainContent");
            settingContent = root.Q<VisualElement>("settingContent");

            ReflectToMainContent(pageController.LoadNewPage(mainUxmlPath, PageController.UICategory.UI));
            ReflectToSettingContent(pageController.LoadNewPage(settingUxmlPath, PageController.UICategory.Setting));
        }
        else
        {
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape");
            // 탭이 두개이므로 활성화 된 곳에서만 입력받도록 처리
            if(tabbedMenuController.CurTab == TabbedMenuController.TabName.mainTab)
            {
                TemplateContainer root;
                if((root=pageController.LoadPrevPage(PageController.UICategory.UI)) != null)
                {
                    Debug.Log("Escape2");
                    ReflectToMainContent(root);
                }
            }
            else if (tabbedMenuController.CurTab == TabbedMenuController.TabName.settingTab)
            {
                TemplateContainer root;
                if ((root = pageController.LoadPrevPage(PageController.UICategory.Setting)) != null)
                {
                    Debug.Log("Escape3");
                    ReflectToSettingContent(root);
                }
            }
        }
    }

    public void ReflectToMainContent(TemplateContainer root)
    {
        mainContent.Clear();
        mainContent.Add(root);
    }

    public void ReflectToSettingContent(TemplateContainer root)
    {
        settingContent.Clear();
        settingContent.Add(root);
    }
}
