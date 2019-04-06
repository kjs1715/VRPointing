using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class auditoryFeedback : MonoBehaviour {
    GameObject gameObject;
    AudioSource audioType1;
    AudioSource audioType2;

    void playTone(int type) {
        if (type == 1) {
            audioType1.Play();
        } else if (type == 2) {
            audioType2.Play();
        }
    }

    bool cover() {

    }

    bool coverOn() {

    }

    bool coverOff() {
        
    }
}