using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneGyroSender : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 0.0167f;
    }

    void Update()
    {
        if (CrossDeviceState.Instance.ControlMode == CrossDeviceState.InputMethod.PhoneGyro && CrossDeviceState.Instance.IsObjBeingManip)
        {
            photonView.RPC("RPC_SyncGyroDel", RpcTarget.All, RotDelYXtoScrDelXZ());
        }
    }

    // RPC를 통해 보내줄 부분
    public Vector3 RotDelYXtoScrDelXZ()
    {
        return new Vector3(Input.gyro.rotationRateUnbiased.y * Time.deltaTime, 0, -Input.gyro.rotationRateUnbiased.x * Time.deltaTime);
    }
}
