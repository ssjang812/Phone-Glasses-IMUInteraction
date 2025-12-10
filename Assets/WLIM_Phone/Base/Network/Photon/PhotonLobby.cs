// PhotonLobby.cs
// Brief: Lightweight Photon PUN matchmaking helper used by this project.
// - Connects to the Photon Master server on Start and attempts to join a random room.
// - If joining fails, it creates a new room with basic RoomOptions.
// - Enables automatic scene synchronization via PhotonNetwork.AutomaticallySyncScene.
// - Acts as a small central entry point for multiplayer session setup.
// Where to look next:
// - Other networking scripts: `Assets/WLIM_Phone/Base/Network/`
// - Photon setup & settings: inspect the `PhotonServerSettings` asset in the Unity project.
// Notes:
// - This class derives from `MonoBehaviourPunCallbacks` to receive Photon lifecycle callbacks.
// - Adjust room creation options and naming strategy if you need deterministic room allocation.

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


        // If you want to auto-execute sync logic when more players join, use a pattern like this.
        // In this project we typically target specific recipients so this block is unused.
        /*
        if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom.PlayerCount >1)
        {
            // synchronization code here
        }
        */
    }

    public override void OnJoinRandomFailed(short returnCode, string message) //Called when failed to join room (no room)
    {
        CreateRoom();
    }

    void CreateRoom()
    {
        int randomRoomName = Random.Range(0,10);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers =10 };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) //Called when failed to create room (name duplicate)
    {
        CreateRoom();
    }
}
