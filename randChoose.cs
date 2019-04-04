using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class randChoose : MonoBehaviour {

    public GameObject hintText;
    public GameObject hintTextBack;
    public GameObject progressHintText;
    public SteamVR_Action_Boolean trigger;
    //public Vector3 Offset;

    private int randNum;
    private HashSet<int> numbers;
    private GameObject []findBall;
    private GameObject [] findSideBall;
    private float scaleOfInterfaceBall = 0.01f;
    private float gapBetweenBalls = 0.141f;
    private float gapBetweenCenter = 0.15f;
    private float gapForZ = 0.1f;
    private float Yoffset = -0.35f;
    private float Zoffset = 0.8f;
    private int limit = 0;
    private int triedNum = 1;
    private string hintString = "Hey U r done";
    private string lastHintString;
    private string progressHintString;
    private bool ballsDisappeared = false;
    private bool start = false;
    int[] randomNumbers;
    int hasused = 0;

    static string measureArmLengthLeft = "将双臂平举，并扣下左手中控制器的扳机";
    static string measureArmLengthRight  = "保持双臂平举，并扣下右手中控制器的扳机";
    static string afterFirstHalf = "走向绿色目标点";
    static string afterReachTarget = "返回黄色初始位置";
    static string afterReachOriginPosition = "向后转，将您的正面对准初始朝向";
    static string afterReachOriginDirection = "扣动扳机，完成后半部分实验";
    static string afterLab ="本轮实验结束";

    bool underTree = false;
    bool atOriginPosition = false;
    bool toOriginDirection = false;

    float underTreeJudgeDistance = 0.2f;
    float atOriginPositionJudgeDistance = 0.1f;
    float toOriginDirectionJudgeDistance = 0.1f;

    public Transform cameraTransform;
    public Transform treeTransform;

    int turnNum = 0;


    int labState = 0;//0:初始 1:前半部分 2:前半部分结束，开始走向树下 3:已经走向树下，开始走回原点
                     //4:已经走回原点，开始转身 5:已经转身，开始开启后半部分 6:已经开启后半部分 7:后半部分结束，整个实验结束

    float proTime = 0.0f;
    float NextTime = 0.0f;
    int timer_i = 0;
    // Use this for initialization
    void Start () {

        findBall = new GameObject[18];
        findSideBall = new GameObject[18];
        for(int i=0;i<18;i++)
        {
            findSideBall[i] = GameObject.Find("Sphere (" + i + ")");
            findBall[i] = GameObject.Find("sphere (" + i + ")");
        }
        numbers = new HashSet<int>();
        //hintText = GameObject.Find("New Text");
        initBallInterface();
        init();

        //changeRandNum(); // testing

        cameraTransform = GameObject.Find("VRCamera").transform;
        treeTransform = GameObject.Find("Target").transform;

        labState = 0;
    }

    void timer() {
        timer_i += 1;
        Debug.Log(timer_i);
    }

    // Update is called once per frame
    void Update () {
        //hintText.transform.position += Offset;
        if (trigger.GetStateDown(SteamVR_Input_Sources.Any))
        {
            //if (!start)
            //{
            //    start = true;
            //    changeHint("");
            //    ballsAppear();
            //    labState = 1;
            //}
            //else 
            if (IsInvoking())
                return;
            if (limit > 17)
            {
                if (labState == 1)
                {
                    numbers.Clear();
                    ballsDisappear();
                    hintText.GetComponent<MeshRenderer>().enabled = true;
                    setText();
                    limit = 0;
                }
                else if(labState == 6)
                {
                    ballsDisappear();

                    hintText.GetComponent<MeshRenderer>().enabled = true;
                    setText();
                    limit = 0;
                }
            }
            else if (labState == 1 || labState == 6)
            {
                changeBackColor();
                Invoke("changeRandNum", 2f);
                limit++;
                //Debug.Log(limit + " " + ballsDisappeared.ToString());
            }
            else if(labState == 0 || labState == 5)
            {
                ballsAppear();
                changeBackColor();  
                hintText.GetComponent<MeshRenderer>().enabled = false;
                Invoke("changeRandNum", 2f);
                limit++;
                setText();
            }
            Debug.Log(limit);
            Debug.Log("state : " + labState);
                    
        }
        //if (trigger.GetStateDown(SteamVR_Input_Sources.RightHand))
        //{
        //    if (ballsDisappeared)
        //    {
        //        ballsAppear();
        //    }
        //    changeRandNum();
        //    limit++;
        //}
        //hintText.GetComponent<TextMesh>().text = randNum.ToString();

        float disToTree = new Vector2(cameraTransform.position.x - treeTransform.position.x, cameraTransform.position.z - treeTransform.position.z).magnitude;
        float disToOriPos = new Vector2(cameraTransform.position.x, cameraTransform.position.z).magnitude;
        float disToOriDir = (new Vector4(cameraTransform.rotation.x, cameraTransform.rotation.y, cameraTransform.rotation.z, cameraTransform.rotation.w) - new Vector4(0,0,0,1)).magnitude;

        //Debug.Log("disToTree = " + disToTree + "disToOriPos = " + disToOriPos + "disToOriDir = " + disToOriDir);
        toOriginDirection = disToOriDir <= toOriginDirectionJudgeDistance;
        atOriginPosition = disToOriPos <= atOriginPositionJudgeDistance;
        underTree = disToTree <= underTreeJudgeDistance;

        if(labState == 2 && underTree)
        {
            setText();
        }
        else if (labState == 3 && atOriginPosition)
        {
            setText();
        }
        else if (labState == 4 && toOriginDirection)
        {
            setText();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (labState == 1 || labState == 6)
            {
                limit = 1;
                changeBackColor();
                Invoke("changeRandNum", 2f);
                changeHint("回退成功" + triedNum);
                triedNum++;
                Debug.Log("ddddddddddddddddd");
            }
        }
        updateProgressHint();
    }

    void init() {
        // for initialize the whole flow
        changeHint("请扣下扳机开始实验...");
        ballsDisappear();

        hintText.GetComponent<MeshRenderer>().enabled = true;
    }

    // auto setting coordinate of balls in interface
    void initBallInterface() {
        for (int i = 0; i < 18; i++) {
            findBall[i].transform.SetPositionAndRotation(new Vector3(getX(i), getY(i), getZ(i)), findBall[i].transform.rotation);
            findBall[i].transform.localScale.Set(scaleOfInterfaceBall, scaleOfInterfaceBall,scaleOfInterfaceBall);
        }
    }
    void changeHint(string newhint)
    {
        hintString = newhint;
        hintText.GetComponent<TextMesh>().text = hintString;
        hintTextBack.GetComponent<TextMesh>().text = hintString;
    }

    void updateProgressHint() {
        int temp = limit - 1;
        progressHintString = "已完成 " + temp.ToString() + " / 18";
        progressHintText.GetComponent<TextMesh>().text = progressHintString;
    }

    // 
    void changeRandNum() {
        //if (labState == 6)
        //{
        //    if (turnNum != 2)
        //        turnNum++;
        //    else
        //        return;
        //}
        
        changeBackColor();

        if(randomNumbers == null || hasused == 18)
        {
            randomNumbers = GetRandomUnrepeatArray(0, 17, 18);
            hasused = 0;
        }
        randNum = randomNumbers[hasused];
        hasused++;

        //changeHint("请点击"+randNum+"号小球");


        //numbers.Add(randNum);
        changeColor();
    }

    // change into a new color
    void changeColor() {
        if (ballsDisappeared) {
            return;
        }
        Renderer render1 = findSideBall[randNum].GetComponent<Renderer>();
        Renderer render2 = findBall[randNum].GetComponent<Renderer>();
        render1.material.color = Color.blue;
        render2.material.color = Color.blue;
    }

    // change back to its original color
    void changeBackColor() {
        //if (ballsDisappeared) {
        //    return;
        //}
        for (int i = 0; i < 18; i++)
        {
            Renderer render1 = findSideBall[i].GetComponent<Renderer>();
            Renderer render2 = findBall[i].GetComponent<Renderer>();
            render1.material.color = Color.red;
            render2.material.color = Color.red;
        }
    }

    void ballsDisappear() {
        if (ballsDisappeared) {
            return ;
        }
        changeBackColor();
        for (int i = 0; i < 18; i++) {
            if (findBall[i] != null) {
                findBall[i].GetComponent<MeshRenderer>().enabled = false;
                //findSideBall.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        ballsDisappeared = true;
        progressHintText.GetComponent<MeshRenderer>().enabled = false;


    }

    void ballsAppear() {
        //Debug.Log("ballsAppear started");
        for (int i = 0; i < 18; i++)
        {
            if (findBall[i] != null)
            {
                findBall[i].GetComponent<MeshRenderer>().enabled = true;
                //findSideBall.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        ballsDisappeared = false;
        progressHintText.GetComponent<MeshRenderer>().enabled = true;
    }

    float getSide(int i) {
        return i / 9 == 0 ? -1 : 1;
    }

    float getX(int i) {
        float offset = 2 - (i % 3);
        return offset * getSide(i) * gapBetweenBalls + gapBetweenCenter * getSide(i);
    }

    float getY(int i)
    {
        float offset = 2 - (i / 3) % 3;
        return offset * gapBetweenBalls + gapBetweenCenter + Yoffset;
    }

    float getZ(int i)
    {
        float offset = i % 3;
        return offset * gapForZ + Zoffset;
    }

    void setText() {
        if (labState == 0)
        {
            labState = 1;
        }
        else if (labState == 1)
        {
            changeHint(afterFirstHalf);

            labState = 2;
        }
        else if(labState == 2)
        {
            changeHint(afterReachTarget);

            labState = 3;
        }
        else if(labState == 3)
        {
            changeHint(afterReachOriginPosition);

            labState = 4;
        }
        else if(labState == 4)
        {
            changeHint(afterReachOriginDirection);

            labState = 5;
        }
        else if (labState == 5)
        {
            changeHint("");
            labState = 6;
        }
        else if(labState == 6)
        {
            changeHint(afterLab);
        }
        else
        {
            changeHint("");
        }
    }

    public int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count) { System.Random rnd = new System.Random(); int length = maxValue - minValue + 1; byte[] keys = new byte[length]; rnd.NextBytes(keys); int[] items = new int[length]; for (int i = 0; i < length; i++) { items[i] = i + minValue; } System.Array.Sort(keys, items); int[] result = new int[count]; System.Array.Copy(items, result, count); return result; }
}
