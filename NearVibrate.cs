using UnityEngine;
using Valve.VR;
using HTC.UnityPlugin.Vive;

public class NearVibrate : MonoBehaviour {

    private Vector3[] p2;
    public Transform leftHandPos;
    public Transform rightHandPos;
    private float[] amplitudes;
    public static float maxA = 2f;
    public static float leastRecoganizableAmplitude = 1.1f;
    public static float vibrateContinuesTime = 0.2f;
    public static float vibrateGapTime = 0.3f;
    public static float vibrateFrequency = 100;
    private float timeSinceLastVibrate = 0;
    // Use this for initialization
    void Start () {
        p2 = nearballs.getCenters();
        amplitudes = new float[18];
    }
	
	// Update is called once per frame
	void Update () {
        //lefthand
        timeSinceLastVibrate += Time.deltaTime;
        if (timeSinceLastVibrate >= vibrateGapTime)
        {
            checkVibrate(leftHandPos.position, SteamVR_Input_Sources.LeftHand);
            checkVibrate(rightHandPos.position, SteamVR_Input_Sources.RightHand);
            timeSinceLastVibrate = 0;
        }
    }

    void checkVibrate(Vector3 pos, SteamVR_Input_Sources hand)
    {
        float s = move.scaleOfBalls;
        for (int i = 0; i < 18; i++)
        {
            float r = (pos - p2[i]).magnitude;
            if (r <= s / 2)
                amplitudes[i] = maxA;
            else
            {
                amplitudes[i] = maxA / (2 * (1 + (10 * (r - s / 2) * (r - s / 2)) / (s * s)));
            }
        }
        float realMaxA = 0;
        for (int i = 0; i < 18; i++)
        {
            if (realMaxA < amplitudes[i])
            {
                realMaxA = amplitudes[i];
            }
        }
        if (realMaxA >= leastRecoganizableAmplitude)
        {
            Haptics.Pulse(vibrateContinuesTime, vibrateFrequency, realMaxA, hand);
        }
    }
}
