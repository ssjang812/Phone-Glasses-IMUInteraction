using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTouchSender_dep : MonoBehaviourPunCallbacks
{
    private EventTrigger trigger;

    private void Start()
    {
        trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { OnButtonUp((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnButtonUp(PointerEventData data)
    {
        photonView.RPC("RPC_ButtonUp", RpcTarget.All, tag);
    }
}
