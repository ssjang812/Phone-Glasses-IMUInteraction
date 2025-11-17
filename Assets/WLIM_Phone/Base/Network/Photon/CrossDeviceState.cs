using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using static CrossDeviceState;

// Combining multiple device states and inputs to identify complex states and input intentions.
// This class mainly refers GlassesState, PhoneState classes


public class CrossDeviceState : MonoBehaviourPunCallbacks
{
    public enum DeviceType
    {
        Glasses,
        Phone,
        Watch,
        Ring,
        Hybrid
    }

    public enum InputMethod
    {
        PhoneSwipe,
        PhoneGyro,
        GlassesGyro,
        Null
    }

    [Serializable]
    public class DeviceInfo //개별 기기정보 관리를 위한 클래스
    {
        public int ActorNumber { get; set; }
        public DeviceType DeviceType { get; set; }

        public DeviceInfo(int actorNumber, DeviceType deviceType)
        {
            ActorNumber = actorNumber;
            DeviceType = deviceType;
        }
    }
    private DeviceType deviceType;
    public DeviceType GlaWebviewType { get; set; }  // 글래스-폰 연동시 글래스, 폰 어디로 볼지 선택
    public static CrossDeviceState Instance { get; private set; }
    public DeviceInfo MyDeviceInfo { get; private set; }
    public List<DeviceInfo> ConnectedDevicesInfo { get; private set; }
    private int preparedConnectedDevices;   // 기기가 접속, 객체관리를 위한 모든 준비가 마치면 cnt++;, 모든 기기가 준비되면 동기화 코드 실행
    public InputMethod ControlMode { get; set; }
    public bool IsObjBeingManip { get; set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            deviceType = DeviceType.Phone;  // 본 코드를 사용하는 기기에 맞게 바꾸시오
            GlaWebviewType = DeviceType.Phone;  // glasses 단독 모드 UI는 나중에 업데이트하자, 현재는 phone 연동식만
            ControlMode = InputMethod.PhoneSwipe;
            IsObjBeingManip = false;
            MyDeviceInfo = null;
            ConnectedDevicesInfo = null;

            preparedConnectedDevices = 0;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public override void OnJoinedRoom()
    {
        MyDeviceInfo = new DeviceInfo(PhotonNetwork.LocalPlayer.ActorNumber, deviceType);
        ConnectedDevicesInfo = new List<DeviceInfo> // 자신을 포함한 연결된 기기 목록
            {
                MyDeviceInfo
            };

        // 동적 생성으로 할시, 접속 이전 상호작용 하는 코드들에 문제가 생겨 자신을 관리하는 객체를 씬에 부착했음
        /*
        switch (MyDeviceInfo.DeviceType)
        {
            case DeviceType.Glasses:
                gameObject.AddComponent<GlassesState>();
                break;
            case DeviceType.Phone:
                gameObject.AddComponent<PhoneState>();
                break;
            case DeviceType.Watch:
                break;
            case DeviceType.Ring:
                break;
            default:
                break;
        }
        */
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }

    // connection 요청을 했을때 아래의 함수를 실행
    public void CrossDeviceConnect(int targetActorNumber)
    {
        Debug.Log("CrossDeviceConnect");

        byte[] serializedConnectedDevicesInfo = Serialize<List<DeviceInfo>>(ConnectedDevicesInfo);
        byte[] serializedMyDeviceInfo = Serialize<DeviceInfo>(MyDeviceInfo);
        object[] eventData = new object[] { serializedConnectedDevicesInfo, serializedMyDeviceInfo };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new int[] { targetActorNumber } };
        byte eventCode = 101;

        // 이벤트 전송
        PhotonNetwork.RaiseEvent(eventCode, eventData, raiseEventOptions, SendOptions.SendReliable);
    }

    public void TestConnect()
    {
        Debug.Log("TestSendIntList");
        List<int> intList = new List<int> { 1, 2, 3, 4, 5 }; // 전송하고 싶은 int형 리스트
        int[] intArray = intList.ToArray(); // int형 리스트를 배열로 변환
        object[] eventData = new object[] { intArray }; // object 배열에 int형 배열을 포함시킴
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new int[] { 1 } }; // 목표 actor 설정
        byte eventCode = 102; // 사용할 이벤트 코드

        PhotonNetwork.RaiseEvent(eventCode, eventData, raiseEventOptions, SendOptions.SendReliable);
    }

    private void OnEventReceived(EventData eventData)
    {
        Debug.Log("OnEventReceived");
        byte eventCode = eventData.Code;
        if (eventCode == 101)
        {
            object[] data = (object[])eventData.CustomData;
            List<DeviceInfo> connectedDevicesInfo = Deserialize<List<DeviceInfo>>((byte[])data[0]);
            DeviceInfo senderPlayerInfo = Deserialize<DeviceInfo>((byte[])data[1]);

            CompareAndUpdatePlayerInfo(connectedDevicesInfo, senderPlayerInfo);
        }
        else if (eventCode == 102) // 이벤트 코드 102에 대한 처리
        {
            object[] data = (object[])eventData.CustomData;
            int[] intArray = (int[])data[0];
            List<int> intList = new List<int>(intArray); // 배열을 다시 List<int>로 변환

            // 변환된 List<int> 사용
            Debug.Log("Received int list:");
            foreach (int item in intList)
            {
                Debug.Log(item);
            }
        }
    }

    // CrossDeviceConnect를 요청하는 기기에서 자신이 가지고있는 리스트의 정보를 넘겨줌, 받은 측에서는 자신의 정보와 비교, 더 많은 정보쪽으로 업데이트
    // plays에 해당하는 대상에게만 RPC 호출
    void CompareAndUpdatePlayerInfo(List<DeviceInfo> connectedDevicesInfo, DeviceInfo senderPlayerInfo)
    {
        if (connectedDevicesInfo.Count > ConnectedDevicesInfo.Count)
        {
            // 상대의 정보가 나보다 많음, 상대정보 + 나자신 추가로 업데이트
            connectedDevicesInfo.Add(MyDeviceInfo);
            foreach (DeviceInfo deviceInfo in connectedDevicesInfo)
            {
                // ActorNumber를 사용하여 Player 객체 조회
                Player player = GetPlayerByActorNumber(deviceInfo.ActorNumber);
                if (player != null)
                {
                    // DeviceInfo 객체를 직렬화하여 RPC 호출
                    byte[] serializedData = Serialize<List<DeviceInfo>>(connectedDevicesInfo);
                    photonView.RPC("RPC_SyncCrossDeviceState", player, serializedData);
                }
            }
        }
        else
        {
            // 내 정보가 상대보다 많음, 내정보 + 상대 추가로 업데이트
            ConnectedDevicesInfo.Add(senderPlayerInfo);
            foreach (DeviceInfo deviceInfo in ConnectedDevicesInfo)
            {
                // ActorNumber를 사용하여 Player 객체 조회
                Player player = GetPlayerByActorNumber(deviceInfo.ActorNumber);
                if (player != null)
                {
                    // DeviceInfo 객체를 직렬화하여 RPC 호출
                    byte[] serializedData = Serialize<List<DeviceInfo>>(ConnectedDevicesInfo);
                    photonView.RPC("RPC_SyncCrossDeviceState", player, serializedData);
                }
            }
        }
    }


    [PunRPC]
    private void RPC_SyncCrossDeviceState(byte[] serializedConnectedDevicesInfo)
    {
        Debug.Log("RPC_SyncCrossDeviceState");

        List<DeviceInfo> connectedDevicesInfo = Deserialize<List<DeviceInfo>>(serializedConnectedDevicesInfo);

        ConnectedDevicesInfo = connectedDevicesInfo;

        // 디바이스 상태 동기화에 필요한 인스턴스 생성
        foreach (DeviceInfo deviceInfo in ConnectedDevicesInfo)
        {
            switch (deviceInfo.DeviceType)
            {
                case DeviceType.Glasses:
                    AddComponentIfMissing<GlassesState>();
                    break;
                case DeviceType.Phone:
                    AddComponentIfMissing<PhoneState>();
                    break;
                case DeviceType.Watch:
                    break;
                case DeviceType.Ring:
                    break;
                default:
                    break;
            }
        }

        // 2중 RPC문
        foreach (DeviceInfo deviceInfo in ConnectedDevicesInfo)
        {
            Player player = GetPlayerByActorNumber(deviceInfo.ActorNumber);
            if (player != null)
            {
                photonView.RPC("RPC_IncreasePreparedConnectedDevices", player);
            }
        }
    }

    [PunRPC]
    private void RPC_IncreasePreparedConnectedDevices()
    {
        preparedConnectedDevices++;
        Debug.Log($"RPC_IncreasePreparedConnectedDevices - {preparedConnectedDevices}");
        if (preparedConnectedDevices == ConnectedDevicesInfo.Count)
        {
            SyncCrossDeviceState();
        }
    }

    private void SyncCrossDeviceState()
    {
        Debug.Log("SyncCrossDeviceState");
        // 내가 해당 디바이스라면, 내 상태를 기반으로 전체 동기화 실행
        switch (MyDeviceInfo.DeviceType)
        {
            case DeviceType.Glasses:
                // 연결된 대상들한테 글래스 RPC 동기화 호출
                foreach (DeviceInfo deviceInfo in ConnectedDevicesInfo)
                {
                    Player player = GetPlayerByActorNumber(deviceInfo.ActorNumber);
                    if (player != null)
                    {
                        // 동기화할 RPC들 (설정값 등을 동기화)
                    }
                }
                break;
            case DeviceType.Phone:
                // 연결된 대상들한테 폰 RPC 동기화 호출
                foreach (DeviceInfo deviceInfo in ConnectedDevicesInfo)
                {
                    Player player = GetPlayerByActorNumber(deviceInfo.ActorNumber);
                    if (player != null)
                    {
                        // 동기화할 RPC들 (설정값 등을 동기화)
                        photonView.RPC("RPC_ObjCntrlModeBtnClick", player, Instance.ControlMode);
                    }
                }
                break;
            case DeviceType.Watch:
                break;
            case DeviceType.Ring:
                break;
            default:
                break;
        }
    }

    // DeviceInfo 객체를 직렬화하는 메서드
    byte[] Serialize<T>(T obj)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }
    }

    // DeviceInfo 객체를 역직렬화하는 메서드
    T Deserialize<T>(byte[] serializedData)
    {
        using (MemoryStream stream = new MemoryStream(serializedData))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(stream);
        }
    }

    // ActorNumber를 사용하여 Player 객체를 찾는 메서드
    public Player GetPlayerByActorNumber(int actorNumber)
    {
        if (PhotonNetwork.CurrentRoom.Players.TryGetValue(actorNumber, out Player player))
        {
            return player;
        }
        return null;
    }

    private void AddComponentIfMissing<T>() where T : Component
    {
        T component = gameObject.GetComponent<T>();

        if (component == null)
        {
            gameObject.AddComponent<T>();
        }
        else
        {
        }
    }


    // 본 프로젝트에서는 통신을 원하는 대상에게만 전송하기위해 Network Messages(RaiseEvent), RPC 방식을 사용
    /*
    public void InitializeWithOtherPlayer()
    {
        // 로컬 플레이어의 DeviceState를 기존 플레이어의 DeviceState로 동기화
        if (PhotonNetwork.PlayerListOthers.Length > 0)
        {
            Player otherPlayer = PhotonNetwork.PlayerListOthers[0]; // 기존 플레이어 중 한 명 선택
            if (otherPlayer.CustomProperties.TryGetValue("DeviceState", out object deviceStateObj))
            {
                DeviceState otherDeviceState = (DeviceState)deviceStateObj;
                // 기존 플레이어의 DeviceState 정보를 로컬 플레이어의 DeviceState에 복사
                ObjControlMode = otherDeviceState.ObjControlMode;
                SwipeDel = otherDeviceState.SwipeDel;
                GyroDel = otherDeviceState.GyroDel;
                IsObjBeingManip = otherDeviceState.IsObjBeingManip;
                SelectedObjFromPhone = otherDeviceState.SelectedObjFromPhone;
                SelectedObjFromGlasses = otherDeviceState.SelectedObjFromGlasses;
                GazeOnObjFromGlasses = otherDeviceState.GazeOnObjFromGlasses;
            }
        }
    }
    */

        // PhotonNetwork.Instantiate로 생성한 객체에서 OnPhotonSerializeView 방식의 업데이트할시 아래를 사용
        // 본 프로젝트에서는 통신을 원하는 대상에게만 전송하기위해 Network Messages(RaiseEvent), RPC 방식을 사용
        /*
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext((int)ObjControlMode);
                stream.SendNext(SwipeDel);
                stream.SendNext(GyroDel);
                stream.SendNext(IsObjBeingManip);
                stream.SendNext((int)SelectedObjFromPhone);
                stream.SendNext((int)SelectedObjFromGlasses);
                stream.SendNext((int)GazeOnObjFromGlasses);
            }
            else
            {
                ObjControlMode = (ObjControlMode)(int)stream.ReceiveNext();
                SwipeDel = (Vector3)stream.ReceiveNext();
                GyroDel = (Vector3)stream.ReceiveNext();
                IsObjBeingManip = (bool)stream.ReceiveNext();
                SelectedObjFromPhone = (SelectedObjFromPhone)(int)stream.ReceiveNext();
                SelectedObjFromGlasses = (SelectedObjFromGlasses)(int)stream.ReceiveNext();
                GazeOnObjFromGlasses = (GazeOnObjFromGlasses)(int)stream.ReceiveNext();
            }
        }
        */
    }