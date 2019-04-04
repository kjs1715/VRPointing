using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballMove : MonoBehaviour {
    Vector3 target;
    bool shouldMove = false;
    public Transform ballTransform;

    // Update is called once per frame

    public void move(Vector3 target)
    {
        this.target = target;
        shouldMove = true;
    }

    void Update()
    {
        if (shouldMove)
        {
            float step = megneticBall.magneticSpeed * Time.deltaTime;
            ballTransform.position = Vector3.MoveTowards(ballTransform.position, target, step);
            if (ballTransform.position.Equals(target))
                shouldMove = false;
        }
    }
}
