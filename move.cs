using UnityEngine;

public class move : MonoBehaviour {

    public static Transform chestTransform;
    public GameObject findResult;
    private float r = 0.65f;
    public static Vector3 centralPoint1 = new Vector3(0, 0, 0);
    public static Vector3 centralPoint2 = new Vector3(0, 0, 0);
    public static float halfHandDistance = 0.65f;
    public static float[] distanceIndex = new float[3] { 0.571f, 0.857f , 0.714f };
    public static float alphaOffset = Mathf.PI/3;
    int alphaOrder = 0;
    public static bool needMeasureArmLength = false;
    public static float distanceFromBallsSphere = 0.65f;
    public static float gapBetweenBalls = 0.315f;
    public static float scaleOfBalls = 0.03f; 
    static GameObject[] balls;

    // Use this for initialization
    void Start () {
        balls = new GameObject[50];
        for (int i = 0; i < 25; i++)
        {
            balls[i] = GameObject.Find("Sphere (" + i + ")");
            balls[i].transform.position = centralPoint1 + toRectangular(getSphericalCoordinates(i, halfHandDistance, alphaOffset));
        }
        for (int i = 25; i < 50; i++)
        {
            balls[i] = GameObject.Find("Sphere (" + i + ")");
            balls[i].transform.position = centralPoint2 + toRectangular(getSphericalCoordinates(i, halfHandDistance, alphaOffset));
        }

        chestTransform = GameObject.Find("DeviceTrackerT").transform;
    }

    public static void moveBall(int distanceIndex)
    {
        if (LogOut.fixedMode)
        {
            for (int i = 0; i < 25; i++)
            {
                balls[i].transform.position = centralPoint1 + toRectangular(getSphericalCoordinates(i, halfHandDistance * move.distanceIndex[distanceIndex], alphaOffset));
            }
            for (int i = 25; i < 50; i++)
            {
                balls[i].transform.position = centralPoint2 + toRectangular(getSphericalCoordinates(i, halfHandDistance * move.distanceIndex[distanceIndex], alphaOffset));
            }
        }
        else
        {
            for (int i = 0; i < 25; i++)
            {
                balls[i].transform.position = chestTransform.TransformPoint(centralPoint1 + toRectangular(getSphericalCoordinates(i, halfHandDistance * move.distanceIndex[distanceIndex], alphaOffset)));
            }
            for (int i = 25; i < 50; i++)
            {
                balls[i].transform.position = chestTransform.TransformPoint(centralPoint2 + toRectangular(getSphericalCoordinates(i, halfHandDistance * move.distanceIndex[distanceIndex], alphaOffset)));
            }
        }
    }

    void Update()
    {
        //for (int i = 0; i < 18; i++)
        //{
        //    //balls[i].transform.position = new Vector3(getX(i), getY(i), getZ(i)) + (LogOut.fixedMode ? new Vector3(0,0,0) :cameraTransform.position);
        //    balls[i].transform.position = centralPoint + getPosOffset(i, alphaOrder) + (LogOut.fixedMode ? new Vector3(0, 0, 0) : new Vector3(cameraTransform.position.x,0,cameraTransform.position.z));
        //}
    }

    static Vector3 getSphericalCoordinates(int order, float radius, float alpha)
    {
        float r = 0, theta = 0, phi = 0;

        switch (order % 5)
        {
            case 0:
                phi = -alpha;
                break;
            case 1:
                phi = -alpha/2;
                break;
            case 2:
                phi = 0;
                break;
            case 3:
                phi = alpha / 2;
                break;
            case 4:
                phi = alpha;
                break;
        }

        switch ((order % 25) / 5)
        {
            case 0:
                theta = Mathf.PI / 2 - alpha;
                break;
            case 1:
                theta = Mathf.PI / 2 - alpha / 2;
                break;
            case 2:
                theta = Mathf.PI / 2;
                break;
            case 3:
                theta = Mathf.PI / 2 + alpha / 2;
                break;
            case 4:
                theta = Mathf.PI / 2 + alpha;
                break;
        }

        r = radius;

        if (order / 25 == 0)
        {
            phi = Mathf.PI - phi;
        }

        return new Vector3(r, theta, phi);

    }

    static Vector3 toRectangular(Vector3 spherical)
    {
        float r = spherical.x;
        float theta = spherical.y;
        float phi = spherical.z;
        return new Vector3(r * Mathf.Sin(theta) * Mathf.Cos(phi),
            r * Mathf.Cos(theta),
            r * Mathf.Sin(theta) * Mathf.Sin(phi));
    }
}
