using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.ColliderEvent;
using Valve.VR;

public class controllerFeedBack : MonoBehaviour, IColliderEventHoverEnterHandler
{
    SteamVR_Action_Vibration action_Vibration;   
    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        //action_Vibration.Execute(0, 1, 150, 75, SteamVR_Input_Sources.Any);
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
