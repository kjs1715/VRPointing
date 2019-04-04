using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Valve.VR;
using HTC.UnityPlugin.Vive;

public class LogOut : MonoBehaviour
{
    public SteamVR_Action_Boolean boolean;
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    public Transform chestTransform;
    public GameObject progressHint;
    public GameObject frontHint;
    string progressText;
    string frontText;
    string lastProgress;
    string lastFront;
    static string logFileName;
    private Vector3[] p2;
    public static bool fixedMode=true;
    // Use this for initialization
    void Start () {
        //File.AppendAllText("C:/Users/RJ/Desktop/log.txt","logsample\n",Encoding.UTF8);
        System.DateTime gameStartTime = System.DateTime.Now;
        logFileName = "Log_VRPointing_" + gameStartTime.Year + "_" + gameStartTime.Month + "_" + gameStartTime.Day + "_" + gameStartTime.Hour + "_" + gameStartTime.Minute + "_" + gameStartTime.Second;
        p2 = nearballs.getCenters();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 leftPos, rightPos;
        if (fixedMode)
        {
            leftPos = leftHandTransform.position;
            rightPos = rightHandTransform.position;
        }
        else
        {
            leftPos =  chestTransform.InverseTransformPoint(leftHandTransform.position);
            rightPos = chestTransform.InverseTransformPoint(rightHandTransform.position);
        }

        if (boolean.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (boolean.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                printLog( "left trigger down at (" + leftPos.x + "," + leftPos.y + "," + leftPos.z + ")");
            }
            if (boolean.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                printLog( "right trigger down at (" + rightPos.x + "," + rightPos.y + "," + rightPos.z + ")");
            }
        }
        //Debug.Log(Time.time);
    }

    public static void printLog(string e)
    {
        File.AppendAllText("C:/Users/RJ/Desktop/Log_VRPointing/"+ logFileName + ".txt", "[" + Time.time.ToString() + "] "+e+"\n", Encoding.UTF8);
    }
}
