using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Obj_Gla
{
    Antenna,
    Headlamp,
    Solarpanel,
    Null
}

public class GlassesState : MonoBehaviour
{
    public static GlassesState Instance { get; private set; }
    public Obj_Gla SelObjForWebview_Gla { get; set; } = Obj_Gla.Null;
    public Obj_Gla GazeOnObj_Gla { get; set; } = Obj_Gla.Null;

    public static UnityEvent event_onGazeOnObjChange;
    public static UnityEvent event_onIsObjBeingManipChange;
    public static UnityEvent event_onSelObjForWebview;

    private void Awake()
    {
        Debug.Log("GlassesState script instantiated");
        if (Instance == null)
        {
            Instance = this;

            if (event_onGazeOnObjChange == null)
                event_onGazeOnObjChange = new UnityEvent();
            if (event_onIsObjBeingManipChange == null)
                event_onIsObjBeingManipChange = new UnityEvent();
            if (event_onSelObjForWebview == null)
                event_onSelObjForWebview = new UnityEvent();
            
            //Glasses측에서 필요한 스크립트이므로, Phone의 프로젝트에서는 주석 처리
            /*
            if (GetComponent<GlassesState>() != null && GetComponent<PhoneState>() != null)
            {
                if (GetComponent<GlaPhoObjControl>() == null)
                {
                    gameObject.AddComponent<GlaPhoObjControl>();
                }
            }
            */

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 아래 코드는 phone 측에만 필요 (글래스 접속후 본 스크립트가 생성됬을시, 연동)
        if (PageController.Instance != null)
        {
            event_onSelObjForWebview.AddListener(PageController.Instance.LoadNewPageFromGlasses);
        }
    }

    [PunRPC]
    void RPC_GazeOnObjChange(Obj_Gla Obj_Gla)
    {
#if DEBUG_MODE
        Debug.Log("RPC_GazeOnObjChange");
#endif
        Instance.GazeOnObj_Gla = Obj_Gla;
        event_onGazeOnObjChange.Invoke();
    }

    [PunRPC]
    void RPC_IsObjBeingManipChange(bool _bool)
    {
#if DEBUG_MODE
        Debug.Log("RPC_IsObjBeingManipChange");
#endif
        CrossDeviceState.Instance.IsObjBeingManip = _bool;
        event_onIsObjBeingManipChange.Invoke();
    }

    [PunRPC]
    void RPC_SelObjForWebview(Obj_Gla Obj_Gla)
    {
#if DEBUG_MODE
        Debug.Log("RPC_SelObjForWebview");
#endif
        Instance.SelObjForWebview_Gla = Obj_Gla;
        event_onSelObjForWebview.Invoke();
    }
}
