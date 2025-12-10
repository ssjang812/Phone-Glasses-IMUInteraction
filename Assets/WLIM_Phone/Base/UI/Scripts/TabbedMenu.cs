// TabbedMenu.cs
// Brief: Simple controller that manages a tabbed UI built with Unity UIElements (UIDocument + UXML).
// - Loads initial UXML pages for the main and settings tabs and reflects TemplateContainers into the
// tab content areas.
// - Delegates tab interaction logic to `TabbedMenuController` and page navigation to `PageController`.
// - Helpful files to inspect next: `PageController` (page/stack management), `TabbedMenuController`
 // (UXML tab selection logic), and the `UXML/` folder for the UI layouts.
// Usage notes:
// - Attach this component to the same GameObject as a `UIDocument` that defines `mainContent` and
// `settingContent` VisualElements.
// - The class persists across scene loads and exposes global access via `TabbedMenu.Instance`.
// - Change `mainUxmlPath` and `settingUxmlPath` to point to different UXML resources if you rework
// the UI layout.

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
    // Path to the UXML files that will be loaded as initial pages for each tab
    private string mainUxmlPath = "UXML/Main";
    private string settingUxmlPath = "UXML/Setting";

    // Controllers: Tab interaction is handled by TabbedMenuController, page stacks by PageController
    private TabbedMenuController tabbedMenuController;
    private PageController pageController;
    // VisualElements that are swapped when the active page changes
    private VisualElement mainContent;
    private VisualElement settingContent;


    void Awake()
    {
        // Singleton pattern: keep one persistent TabbedMenu across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize UIElements controllers using the UIDocument on this GameObject
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;

            // TabbedMenuController encapsulates tab click/selection behavior
            tabbedMenuController = new TabbedMenuController(root);
            // PageController handles pushing/popping TemplateContainer pages for each category
            pageController = gameObject.AddComponent<PageController>();

            tabbedMenuController.RegisterTabCallbacks();

            // Grab the containers where pages will be reflected and load initial pages
            mainContent = root.Q<VisualElement>("mainContent");
            settingContent = root.Q<VisualElement>("settingContent");

            ReflectToMainContent(pageController.LoadNewPage(mainUxmlPath, PageController.UICategory.UI));
            ReflectToSettingContent(pageController.LoadNewPage(settingUxmlPath, PageController.UICategory.Setting));
        }
        else
        {
            // If a second instance is created, it is ignored/destroyed by the caller if desired
        }
    }

    void Update()
    {
        // Handle Android/Back/Escape behavior to navigate to previous page in the active tab
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape");
            // Only handle the currently active tab's back stack
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

    // Replace the contents of the main tab's container with the provided TemplateContainer
    public void ReflectToMainContent(TemplateContainer root)
    {
        mainContent.Clear();
        mainContent.Add(root);
    }

    // Replace the contents of the setting tab's container with the provided TemplateContainer
    public void ReflectToSettingContent(TemplateContainer root)
    {
        settingContent.Clear();
        settingContent.Add(root);
    }
}
