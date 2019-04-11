using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BallCollision : MonoBehaviour
{
    private int mode = 0; // 1. Haptic  2. Visual  3. Sound

    // 1. Variables for Haptic Feedback
    public static float vibrateContinuesTime = 1f;
    public static float vibrateGapTime = 0.3f;
    public static float vibrateFrequency = 100f;
    public static float maxA = 3000;
    public SteamVR_Action_Vibration Vibration;
    public SteamVR_Action_Boolean boolean;
    public string num;
    


    // 2. Variables for Sound Feedback
    public AudioSource audioType1;
    //public AudioSource audioType2;

    // 3. Variables for Visual Feedback
    public Renderer renderer;
    Color newColor = Color.blue;
    Color oldColor;
    bool isCovered = false;
    int count = 0;


    // Use this for initialization
    void Start()
    {
        num = this.gameObject.name.Remove(0, 8);
        num = num.Remove(num.Length - 1, 1);
        renderer = GameObject.Find("sphere (" + num + ")").GetComponent<Renderer>();
        oldColor = renderer.material.color;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("It`s " + this.gameObject.name);
        Pulse(vibrateContinuesTime, vibrateFrequency, maxA, SteamVR_Input_Sources.LeftHand);
        Pulse(vibrateContinuesTime, vibrateFrequency, maxA, SteamVR_Input_Sources.RightHand);
        // Haptic test
        //vibrate(1, SteamVR_Input_Sources.LeftHand);

        // Sound test
        //playTone(1);

        // Visual test
        //isCovered = true;
        changeHintColor(1);
    }

    void OnTriggerStay(Collider collider)
    {
        Debug.Log("Still touching " + collider.tag);
        changeHintColor(1);
        isCovered = true;
        count++;
        Debug.Log("OnTriggerDebug = " + count);

    }

    void OnTriggerExit(Collider collider)
    {
        Debug.Log("Bye " + collider.tag);
        //playTone(2);
        //vibrate(2, SteamVR_Input_Sources.LeftHand);
        changeHintColor(2);
        //isCovered = false;
    }

    void vibrate(int type, SteamVR_Input_Sources hand)
    {

        if (type == 1)
        {

            Pulse(vibrateContinuesTime, vibrateFrequency, maxA, hand);

        }
        else if (type == 2)
        {

            Pulse(vibrateContinuesTime, vibrateFrequency, maxA, hand);

        }

    }

    public void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        print("make a pulse, duration = " + duration + ", frequency = " + frequency + ", amplitude = " + amplitude);
        Debug.Log(Vibration == null);
        Vibration.Execute(0, duration, frequency, amplitude, source);
        
        
    }

    void playTone(int type)
    {

        if (type == 1)
        {

            audioType1.Play();

        }
        else if (type == 2)
        {

            //audioType2.Play();

        }

    }

    void changeHintColor(int type)
    {
        if (type == 1)
        {
            renderer.material.color = newColor;
        }
        else if (type == 2)
        {
            if(randChoose1.randNum == int.Parse(num) && randChoose1.isWaiting)
            {
                renderer.material.color = Color.red;
                return ;
            }
            renderer.material.color = oldColor;
        }

    }
}
