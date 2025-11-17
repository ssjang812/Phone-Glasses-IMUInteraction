using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CrossDeviceState;

public class PhoneTouchSender : MonoBehaviourPunCallbacks
{
    public void OnPointerDown()
    {
        Debug.Log("OnPointerDown");
        foreach (DeviceInfo deviceInfo in Instance.ConnectedDevicesInfo)
        {
            Player player = Instance.GetPlayerByActorNumber(deviceInfo.ActorNumber);
            if (player != null)
            {
                photonView.RPC("RPC_PointerDown", player);
            }
        }
    }

    public void OnPointerUp()
    {
        Debug.Log("OnPointerUp");
        foreach (DeviceInfo deviceInfo in Instance.ConnectedDevicesInfo)
        {
            Player player = Instance.GetPlayerByActorNumber(deviceInfo.ActorNumber);
            if (player != null)
            {
                photonView.RPC("RPC_PointerUp", player);
            }
        }
    }

    public void OnPhoneSwipeButtonClick()
    {
        Debug.Log("OnPhoneSwipeClick");

        foreach (DeviceInfo deviceInfo in Instance.ConnectedDevicesInfo)
        {
            Player player = Instance.GetPlayerByActorNumber(deviceInfo.ActorNumber);
            if (player != null)
            {
                photonView.RPC("RPC_ObjCntrlModeBtnClick", player, InputMethod.PhoneSwipe);
            }
        }
    }

    public void OnPhoneGyroButtonClick()
    {
        Debug.Log("OnPhoneGyroClick");
        foreach (DeviceInfo deviceInfo in Instance.ConnectedDevicesInfo)
        {
            Player player = Instance.GetPlayerByActorNumber(deviceInfo.ActorNumber);
            if (player != null)
            {
                photonView.RPC("RPC_ObjCntrlModeBtnClick", player, InputMethod.PhoneGyro);
            }
        }
    }

    public void OnGlassesGyroButtonClick()
    {
        Debug.Log("OnGlassesGyroClick");
        foreach (DeviceInfo deviceInfo in Instance.ConnectedDevicesInfo)
        {
            Player player = Instance.GetPlayerByActorNumber(deviceInfo.ActorNumber);
            if (player != null)
            {
                photonView.RPC("RPC_ObjCntrlModeBtnClick", player, InputMethod.GlassesGyro);
            }
        }
    }
}
