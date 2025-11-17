using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController_example : MonoBehaviour
{
    //바텀 시트 부모
    private VisualElement _bottomContainer;
    //열기 버튼
    private Button _openButton;
    //닫기 버튼
    private Button _closeButton;
    //바텀 시트
    private VisualElement _bottomSheet;
    //가림막
    private VisualElement _scrim;

    //소년과 소녀
    private VisualElement _boy;
    private VisualElement _girl;

    //소녀 대사용 레이블
    private Label _message;


    // Start is called before the first frame update
    void Start()
    {
        //UI 도큐먼트에 있는 최상위 비주얼엘리먼트를 참조한다.
        var root = GetComponent<UIDocument>().rootVisualElement;

        //바텀 시트의 부모
        _bottomContainer = root.Q<VisualElement>("Container_Bottom");

        //열기, 닫기 버튼
        _openButton = root.Q<Button>("Button_Open");
        _closeButton = root.Q<Button>("Button_Close");
        //바텀 시트와 가림막
        _bottomSheet = root.Q<VisualElement>("BottomSheet");
        _scrim = root.Q<VisualElement>("Scrim");

        //소년과 소녀
        _boy = root.Q<VisualElement>("Image_Boy");
        _girl = root.Q<VisualElement>("Image_Girl");

        //소녀 대사용 메시지
        _message = root.Q<Label>("Message");

        //시작할 때 바텀 시트 그룹을 감춘다.
        _bottomContainer.style.display = DisplayStyle.None;

        //버튼이 할 일
        _openButton.RegisterCallback<ClickEvent>(OnOpenButtonClicked);
        _closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);

        //소년 애니메이션
        //AnimateBoy();
        //씬을 시작하고 0.5초 뒤에 소년을 애니메이션 한다.
        Invoke("AnimateBoy", .5f);

        //바텀시트가 내려온 다음 그룹은 끈다.
        _bottomSheet.RegisterCallback<TransitionEndEvent>(OnBottomSheetDown);
    }

    private void AnimateBoy()
    {
        _boy.RemoveFromClassList("image--boy--inair");
    }

    private void Update()
    {
        //특정 스타일 클래스가 리스트 안에 있는지 매 프레임 확인한다.
        //Debug.Log(_boy.ClassListContains("image--boy--inair"));
    }


    private void OnOpenButtonClicked(ClickEvent evt)
    {
        //바텀시트 그룹을 보여준다.
        _bottomContainer.style.display = DisplayStyle.Flex;

        //바텀시트와 가림막 애니메이션
        _bottomSheet.AddToClassList("bottomsheet--up");
        _scrim.AddToClassList("scrim--fadein");

        AnimateGirl();
    }

    private void AnimateGirl()
    {
        //소녀 클래스리스트에 있는 image--girl--up을 추가하거나 제거한다.
        _girl.ToggleInClassList("image--girl--up");

        //트랜지션이 끝날 때, image--girl--up을 추가하거나 제거한다.
        _girl.RegisterCallback<TransitionEndEvent>
        (
            evt => _girl.ToggleInClassList("image--girl--up")
        );
    }

    private void OnCloseButtonClicked(ClickEvent evt)
    {
        //바텀시트와 가림막 애니메이션
        _bottomSheet.RemoveFromClassList("bottomsheet--up");
        _scrim.RemoveFromClassList("scrim--fadein");
    }

    private void OnBottomSheetDown(TransitionEndEvent evt)
    {
        // 내려올 때의 트랜지션에서만 바텀시트 그룹을 감추게 한다.
        if (!_bottomSheet.ClassListContains("bottomsheet--up"))
        {
            //바텀시트 그룹을 감춘다.
            _bottomContainer.style.display = DisplayStyle.None;
        }
    }
}
