using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getHelmetTransform : MonoBehaviour {

    public Transform helmetTransform;
    float timeCount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeCount = timeCount + Time.deltaTime;
        if (timeCount>=2)
        {
            timeCount = 0;
            Debug.Log("position: " + helmetTransform.position + "rotation:" + helmetTransform.rotation);
        }
	}
}
