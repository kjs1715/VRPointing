using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class stickLoggger : MonoBehaviour {

    // Use this for initialization
    private int count; 

    public Transform lefthand;
    public Transform rightHand;
    static string logFileName;
    string buffer;
    void Start () {
        count = 0;
        buffer = "";
        System.DateTime gameStartTime = System.DateTime.Now;
        logFileName = "LogHandStick_" + +gameStartTime.Year + "_" + gameStartTime.Month + "_" + gameStartTime.Day + "_" + gameStartTime.Hour + "_" + gameStartTime.Minute + "_" + gameStartTime.Second;
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(count);

        count++;
        buffer += "[" + Time.time.ToString() + "] " + "left (" + lefthand.position.x + "," + lefthand.position.y + "," + lefthand.position.z + ")" + "\n" + "[" + Time.time.ToString() + "] " + "right(" + rightHand.position.x + "," + rightHand.position.y + "," + rightHand.position.z + ")" + "\n";
        if (count == 100) {
            printlog();
            count = 0;
            buffer = "";
        }
	}


    public void printlog()
    {
        File.AppendAllText("C:/Users/RJ/Desktop/Log_VRPointing/LogForHand/" + logFileName + ".txt", buffer, Encoding.UTF8);
    }
}
