using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class hapticFeedback : MonoBehaviour {
    GameObject gameObject;
    public static float vibrateContinuesTime = 0.2f;
    public static float vibrateGapTime = 0.3f;
    public static float vibrateFrequency = 100;
    public static float maxA = 2f;

    void vibrate(int type, SteamVR_Input_Sources hand) {
        if (type == 1) {
            Haptics.Pulse(vibrateContinuesTime, vibrateFrequency, maxA, hand);
        } else if (type == 2) {
            Haptics.Pulse(vibrateContinuesTime, vibrateFrequency, maxA, hand);
        }
    }
}