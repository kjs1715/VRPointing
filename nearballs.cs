using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nearballs : MonoBehaviour {

    public static bool near(Vector3 p1, Vector3 p2, float r)
    {
        //Debug.Log("p1=" + p1 + "p2=" + p2 + "r=" + r);
        return (p1 - p2).magnitude <= r;
    }

    public static bool near(Vector3 p1, Vector3 []p2, float r)
    {
        for (int i = 0; i < p2.Length; i++)
        {
            Debug.Log("i="+i);
            if (near(p1, p2[i], r))
                return true;
        }
        return false;
    }

    public static Vector3[] getCenters()
    {
        float d = move.distanceFromBallsSphere;
        float r = move.gapBetweenBalls;
        Vector3[] c = new Vector3[18];
        for (int i = 0; i < 18; i++)
        {
            c[i] = new Vector3(i / 9 == 0 ? -d : d, 
                i % 9 / 3 == 0 ? r : i % 9 / 3 == 1 ? 0 : -r,
                i % 3 == 0 ? -r : i % 3 == 1 ? 0 : r);
        }
        return c;
    }
}
