using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //Connects to Master photon server.
    }

    public override void OnConnectedToMaster() //Called when app connected to Master photon server
    {
        PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room!");
        Debug.Log($"ActorNum: {PhotonNetwork.LocalPlayer.ActorNumber}, PlayerCnt: {PhotonNetwork.CurrentRoom.PlayerCount}");


        // 접속시 자동 동기화 실행시 아래의 형태로, 본 프로젝트에서는 접속 대상을 지정해야하기때문에 사용하지 않음
        /*
        if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            // 동기화 코드 실행
        }
        */
    }

    public override void OnJoinRandomFailed(short returnCode, string message) //Called when failed to join room (no room)
    {
        CreateRoom();
    }

    void CreateRoom()
    {
        int randomRoomName = Random.Range(0, 10);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) //Called when failed to create room (name duplicate)
    {
        CreateRoom();
    }
}
