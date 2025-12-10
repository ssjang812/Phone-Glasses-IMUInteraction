using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This class continuously monitors the status and input of the smartphone.

public enum Obj_Pho
{
    Antenna,
    Headlamp,
    Solarpanel,
    Null
}

public class PhoneState : MonoBehaviour
{
    public static PhoneState Instance { get; private set; }
    public Vector3 SwipeDel { get; set; }
    public Vector3 GyroDel { get; set; }
    public Obj_Pho SelObjForWebview_Pho { get; set; }

    public static UnityEvent event_syncGyroDel;
    public static UnityEvent event_syncSwipeDel;


    public static UnityEvent event_touchDown;
    public static UnityEvent event_touchUp;

    public static UnityEvent event_onCntrlModeBtnClick;

    public static UnityEvent event_onGenBtnClick;

    private void Awake()
    {
        Debug.Log("PhoneState script instantiated");
        if (Instance == null)
        {
            Instance = this;

            if (event_syncGyroDel == null)
                event_syncGyroDel = new UnityEvent();
            if (event_syncSwipeDel == null)
                event_syncSwipeDel = new UnityEvent();

            if (event_touchDown == null)
                event_touchDown = new UnityEvent();
            if (event_touchUp == null)
                event_touchUp = new UnityEvent();

            if (event_onCntrlModeBtnClick == null)
                event_onCntrlModeBtnClick = new UnityEvent();

            if (event_onGenBtnClick == null)
                event_onGenBtnClick = new UnityEvent();

            SelObjForWebview_Pho = Obj_Pho.Null;

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
}