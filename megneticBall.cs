using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using HTC.UnityPlugin.Vive;

public class megneticBall : MonoBehaviour {
    private Vector3[] p2;
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    int leftHandLastZone = -1;
    int rightHandLastZone = -1;
    float maxMagneticDistance = 0;
    public static float magneticSpeed = 1f;
    GameObject[] balls;
    Component[]ballMoveComponent;
    Vector3 []target;
    bool []shouldMove;
    public SteamVR_Action_Vibration Vibration;
    // Use this for initialization
    void Start () {
        p2 = nearballs.getCenters();
        maxMagneticDistance = getMaxMagneticDistance(move.distanceFromBallsSphere, move.scaleOfBalls / 2);
        ballMoveComponent = new Component[p2.Length];
        balls = new GameObject[p2.Length];
        target = new Vector3[p2.Length];
        shouldMove = new bool[18];
        for (int i=0;i<p2.Length;i++)
        {
            balls[i] = GameObject.Find("Sphere (" + i + ")");
            //ballMoveComponent[i] = balls[i].GetComponent<ballMove>();
            shouldMove[i] = false;
        }
	}

    void moveBall(int ballNum, Vector3 t)
    {
        target[ballNum] = t;
        shouldMove[ballNum] = true;
    }

    // Update is called once per frame
    void Update () {
        int leftHandThisZone = getZone(leftHandTransform.position, p2, maxMagneticDistance);
        int rightHandThisZone = getZone(rightHandTransform.position, p2, maxMagneticDistance);

        //处理左手
        if(leftHandLastZone == -1 && leftHandThisZone == -1)
        {
            //区域外移动，什么也不做
        }
        else if(leftHandLastZone == -1 && leftHandThisZone != -1)
        {
            //进入某球的磁性区域
            intoZone(leftHandTransform.position, leftHandThisZone, SteamVR_Input_Sources.LeftHand);
        }
        else if( leftHandLastZone != -1 && leftHandThisZone == -1)
        {
            //退出某区域
            outZone(leftHandLastZone, SteamVR_Input_Sources.LeftHand);
        }
        else if(leftHandLastZone != -1 && leftHandThisZone != -1 && (leftHandThisZone == leftHandLastZone))
        {
            //同一区域内移动
            inZoneMove(leftHandTransform.position, leftHandThisZone, SteamVR_Input_Sources.LeftHand);
        }
        else if(leftHandLastZone != -1 && leftHandThisZone != -1 && (leftHandThisZone != leftHandLastZone))
        {
            //迅速从一个区域移动到另一区域
            outZone(leftHandLastZone, SteamVR_Input_Sources.LeftHand);
            intoZone(leftHandTransform.position, leftHandThisZone, SteamVR_Input_Sources.LeftHand);
        }
        else
        {
            //不可能出现
        }

        //处理右手
        if (rightHandLastZone == -1 && rightHandThisZone == -1)
        {
            //区域外移动，什么也不做
        }
        else if (rightHandLastZone == -1 && rightHandThisZone != -1)
        {
            //进入某球的磁性区域
            intoZone(rightHandTransform.position, rightHandThisZone, SteamVR_Input_Sources.RightHand);
        }
        else if (rightHandLastZone != -1 && rightHandThisZone == -1)
        {
            //退出某区域
            outZone(rightHandLastZone, SteamVR_Input_Sources.RightHand);
        }
        else if (rightHandLastZone != -1 && rightHandThisZone != -1 && (rightHandThisZone == rightHandLastZone))
        {
            //同一区域内移动
            inZoneMove(rightHandTransform.position, rightHandThisZone, SteamVR_Input_Sources.RightHand);
        }
        else if (rightHandLastZone != -1 && rightHandThisZone != -1 && (rightHandThisZone != rightHandLastZone))
        {
            //迅速从一个区域移动到另一区域
            outZone(rightHandLastZone, SteamVR_Input_Sources.RightHand);
            intoZone(rightHandTransform.position, rightHandThisZone, SteamVR_Input_Sources.RightHand);
        }
        else
        {
            //不可能出现
        }

        //进行lastZone的更新
        leftHandLastZone = leftHandThisZone;
        rightHandLastZone = rightHandThisZone;
        float step = megneticBall.magneticSpeed * Time.deltaTime;

        for (int i=0;i<18;i++)
        {
            if (shouldMove[i])
            {
                Debug.Log(step);
                balls[i].transform.position = Vector3.MoveTowards(balls[i].transform.position, target[i], step);
                if (balls[i].transform.position.Equals(target[i]))
                    shouldMove[i] = false;
            }
        }
    }

    float getMaxMagneticDistance(float distanceBetweenBalls, float ballsRadius)
    {
        return Mathf.Max(distanceBetweenBalls / 4, ballsRadius);
    }

    int getZone(Vector3 pos, Vector3 []centers, float maxMagneticDistance)
    {
        for(int i=0;i<centers.Length;i++)
        {
            if(Mathf.Abs(pos.x-centers[i].x)<= maxMagneticDistance && 
                Mathf.Abs(pos.y - centers[i].y) <= maxMagneticDistance && 
                Mathf.Abs(pos.z - centers[i].z) <= maxMagneticDistance)
            {
                if((pos-centers[i]).magnitude<=maxMagneticDistance)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    void intoZone(Vector3 pos, int zoneNum, SteamVR_Input_Sources whichHand)
    {
        //吸附
        //Debug.Log("ball " + zoneNum + " is magneted to " + (whichHand.Equals(SteamVR_Input_Sources.LeftHand) ? "lefthand." : "righthand"));
        //ballMoveComponent[zoneNum]
        //StartCoroutine(MoveObject(ballInThisZone.transform.position, pos, (ballInThisZone.transform.position - pos).magnitude / magneticSpeed));
        moveBall(zoneNum,pos);

        Pulse(0.3f, 200, 2f, whichHand);


        //ballInThisZone.transform.position = Vector3.MoveTowards(ballInThisZone.transform.position, pos, magneticSpeed * Time.deltaTime);

        //播放声音

        //振动（？）
    }

    void outZone(int zoneNum, SteamVR_Input_Sources whichHand)
    {
        //小球归位
        //Debug.Log("ball " + zoneNum + " return back to its center ");
        moveBall(zoneNum, p2[zoneNum]);
        Pulse(0.1f, 200, 0.5f, whichHand);
        //ballInThisZone.transform.position = Vector3.MoveTowards(ballInThisZone.transform.position, p2[zoneNum], magneticSpeed * Time.deltaTime);

        //(?)播放声音(?)
    }

    void inZoneMove(Vector3 moveto, int zoneNum, SteamVR_Input_Sources whichHand)
    {
        //小球跟随移动
        moveBall(zoneNum, moveto);
    }


    //在time时间内移动物体

    private IEnumerator MoveObject(Vector3 startPos, Vector3 endPos, float time)
    {
        var dur = 0.0f;
        while (dur <= time)
        {
            dur += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, dur / time);
            yield return null;
        }
    }
    public void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        //print("make a pulse, duration = " + duration + ", frequency = " + frequency + ", amplitude = " + amplitude);
        Vibration.Execute(0, duration, frequency, amplitude, source);
    }
}
