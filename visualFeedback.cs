using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class visualFeedback : MonoBehaviour {
    GameObject obj;
    Transform leftController;
    Transform rightController;
    private int radius = 1; // 设置半径
    private bool covered = false;

    void changeColor(Color color) {
        Renderer render = gameObject.GetComponent<Renderer>();
        render.color = color;
    }

    bool cover() {
        float dx = Mathf.Abs(obj.transform.position.x) - Mathf.Abs(leftController.position.x);
        float dy = Mathf.Abs(obj.transform.position.y) - Mathf.Abs(leftController.position.y);
        float dz = Mathf.Abs(obj.transform.position.z) - Mathf.Abs(leftController.position.z);
        float d = Math.Pow(dx, 2) + Math.Pow(dy, 2) + Math.Pow(dz, 2);
        float dis = Math.Sqrt(d);
        if (dis <= radius) {
            return true;
        }
        return false;
    }

    bool coverOn() {
        float dx = Mathf.Abs(obj.transform.position.x) - Mathf.Abs(leftController.position.x);
        float dy = Mathf.Abs(obj.transform.position.y) - Mathf.Abs(leftController.position.y);
        float dz = Mathf.Abs(obj.transform.position.z) - Mathf.Abs(leftController.position.z);
        float d = Math.Pow(dx, 2) + Math.Pow(dy, 2) + Math.Pow(dz, 2);
        float dis = Math.Sqrt(d);
        if (!covered && dis < radius) {
            covered = true;
            return true;
        }
        return false;
    }

    bool coverOff() {
        float dx = Mathf.Abs(obj.transform.position.x) - Mathf.Abs(leftController.position.x);
        float dy = Mathf.Abs(obj.transform.position.y) - Mathf.Abs(leftController.position.y);
        float dz = Mathf.Abs(obj.transform.position.z) - Mathf.Abs(leftController.position.z);
        float d = Math.Pow(dx, 2) + Math.Pow(dy, 2) + Math.Pow(dz, 2);
        float dis = Math.Sqrt(d);
        if (covered && dis > radius) {
            covered = false;
            return true;
        }
        return false;
    }
}