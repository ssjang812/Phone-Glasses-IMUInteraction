using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainScript : MonoBehaviourPunCallbacks, UIControllerInterface
{
    public string MyURL { get; set; }
    public TemplateContainer MyRoot { get; set; }

    // 버튼에 매핑될 다음 페이지 경로
    private string listOfExhibitsUxmlPath = "UXML/MainItems/ListOfExhibits";
    private Button _listOfExhibitsButton;
    private Button _targetConnectButton;
    private Button _testButton;
    private Label _myActorNumLabel;
    private TextField _targetActorNumTextField;
    private Label _imuDataLabel;

    // TebbedMenu에서 메인 페이지에 갈아끼고 본 스크립트의 인스턴스를 생성 및 관리 해주므로 여기서는 내부 버튼 기능만 매핑해주면 됨
    public void Initialize(string url, TemplateContainer root)
    {
        MyURL = url;
        MyRoot = root;
        _listOfExhibitsButton = MyRoot.Q<Button>("ListOfExhibitsButton");
        _listOfExhibitsButton.RegisterCallback<ClickEvent>(OnListOfExhibitsButtonClicked);
        _targetConnectButton = MyRoot.Q<Button>("TargetConnectButton");
        _targetConnectButton.RegisterCallback<ClickEvent>(OnTargetConnectButtonButtonClicked);
        _testButton = MyRoot.Q<Button>("TestButton");
        _testButton.RegisterCallback<ClickEvent>(OnTestButtonButtonClicked);
        _myActorNumLabel = MyRoot.Q<Label>("MyActorNumLabel");
        _targetActorNumTextField = MyRoot.Q<TextField>("TargetActorNumTextField");
        _imuDataLabel = MyRoot.Q<Label>("IMULabel");
    }

    void Update()
    {
        // 연결이 안되도 출력시도하는 문제때문에
        /*
        _imuDataLabel.text = "imuLabel";
        Debug.Log($"acc: {RingIMUreceiver.Instance.ringIMU.acc}, gyr: {RingIMUreceiver.Instance.ringIMU.gyr}");
        _imuDataLabel.text = $"acc: {RingIMUreceiver.Instance.ringIMU.acc}, gyr: {RingIMUreceiver.Instance.ringIMU.gyr}";
        */
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("접속!!!  " + PhotonNetwork.LocalPlayer.ActorNumber);
        _myActorNumLabel.text = $"me : {PhotonNetwork.LocalPlayer.ActorNumber}";
    }

    private void OnListOfExhibitsButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnListOfExhibitsButtonButtonClicked");
        if (GlassesState.Instance == null) // 글래스 연동 안됬을때는 그냥 작동
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(listOfExhibitsUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        } // 연동됬을때는 시선이 객체조작을 하지 않는 상태일때 작동
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(listOfExhibitsUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        }
    }

    private void OnTargetConnectButtonButtonClicked(ClickEvent evt)
    {
        Debug.Log($"OnTargetConnectButtonButtonClicked {_targetActorNumTextField.value}");
        if (!(_targetActorNumTextField.value == ""))
        {
            CrossDeviceState.Instance.CrossDeviceConnect(int.Parse(_targetActorNumTextField.value));
        } 
    }

    private void OnTestButtonButtonClicked(ClickEvent evt)
    {
        Debug.Log($"OnTestButtonButtonClicked");
        CrossDeviceState.Instance.TestConnect();
    }
}
