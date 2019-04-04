using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using HTC.UnityPlugin.Vive;

public class Haptics : MonoBehaviour {
    public static SteamVR_Action_Vibration Vibration;
    public SteamVR_Action_Boolean boolean;
    private Vector3[] p2;
    public Transform leftHandPos;
    public Transform rightHandPos;
    void Start()
    {
        p2 = nearballs.getCenters();
        //for(int i=0;i<18;i++)
        //{
        //    print(p2[i]);
        //}
    }
    //Update is called once per frame
    void Update () {
        if (nearballs.near(leftHandPos.position, p2,0.1f)) {
            Pulse(0.2f, 100, 200, SteamVR_Input_Sources.LeftHand);
        }
        if ((nearballs.near(rightHandPos.position, p2, 0.1f)))
        {
            Pulse(0.2f, 100, 200, SteamVR_Input_Sources.RightHand);
        }
    }

    public static void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        //print("make a pulse, duration = " + duration + ", frequency = " + frequency + ", amplitude = " + amplitude);
        Vibration.Execute(0, duration, frequency, amplitude, source);
    }
}
