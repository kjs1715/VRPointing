using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class randChoose1 : MonoBehaviour {

    public GameObject hintText;
    public GameObject hintTextBack;
    public GameObject progressHintText;
    public SteamVR_Action_Boolean trigger;
    public AudioSource audioHint;

    public Transform leftController;
    public Transform rightController;
    private Vector3 leftPos1;
    private Vector3 rightPos1;
    private Vector3 leftPos2;
    private Vector3 rightPos2;
    private Vector3 leftPos1r;
    private Vector3 rightPos1r;
    private Vector3 leftPos2r;
    private Vector3 rightPos2r;
    private Vector3 offset;
    //public Vector3 Offset;

    public static int randNum = -1;
    private HashSet<int> numbers;
    private GameObject []findBall;
    private GameObject [] findSideBall;
    private float scaleOfInterfaceBall = 0.01f;
    private float gapBetweenBalls = 0.04f;
    private float gapBetweenCenter = 0.1f;
    private float gapForY = 0.05f;
    private float Yoffset = -0.25f;
    private float Zoffset = 1f;
    private int limit = 0;
    private int triedNum = 1;
    private string hintString = "Hey U r done";
    private string lastHintString;
    private string progressHintString;
    private bool ballsDisappeared = false;
    private bool start = false;
    public static bool isWaiting = false;
   
    int[] randomNumbers;
    int hasused = 0;

    static string triggerPress = "扣下任意扳机";
    static string measureArmLengthLeft = "双臂水平平举，扣左侧扳机";
    static string measureArmLengthRight  = "双臂水平平举，扣右侧扳机";
    static string measureArmHeightLeft = "双臂竖直上举，扣左侧扳机";
    static string measureArmHeightRight = "双臂竖直上举，扣右侧扳机";
    static string afterTurn = "记忆两侧小球位置，按下空格以继续";
    static string afterLab ="实验完成";

    public Transform cameraTransform;

    int bigTurnNum = 0;


    int labState = 0;//取值0-3
    int smallState = 0;//取值0-4
    int maxLabState = 3;
    int maxSmallState = 4;

    float proTime = 0.0f;
    float NextTime = 0.0f;
    int timer_i = 0;

    float timerGap = 1f;//计时器间隔

    int maxLimit = 50;
    
    float dirLimit = 0.2f;
    float disFromOriDirection = 0;
    public GameObject warningText;

    bool sideBallsDisappeared = false;

    Color backBall = Color.white;
    Color lightBall = Color.red;

    bool waitingForSpace = false;

    // Use this for initialization
    void Start () {

        findBall = new GameObject[50];
        findSideBall = new GameObject[50];
        for(int i=0;i<50;i++)
        {
            findSideBall[i] = GameObject.Find("Sphere (" + i + ")");
            findBall[i] = GameObject.Find("sphere (" + i + ")");
        }
        numbers = new HashSet<int>();
        //hintText = GameObject.Find("New Text");
        initBallInterface();
        init();

        //changeRandNum(); // testing

        cameraTransform = GameObject.Find("Camera").transform;
        
        labState = 0;
        smallState = 0;
        audioHint = this.GetComponent<AudioSource>();
        //audioHint.Play();

        changeHint(triggerPress);
    }

    void timer() {
        timer_i += 1;
        Debug.Log(timer_i);
    }

    // Update is called once per frame
    void Update () {
        //hintText.transform.position += Offset;
        initBallInterface();
        if (trigger.GetStateDown(SteamVR_Input_Sources.Any))
        {
            isWaiting = false;
            if (IsInvoking())
            {
                return;
            }
            if (labState == 0)
            {
                if (smallState == 0)
                {
                    changeHint(measureArmLengthLeft);
                    smallState = 1;
                }
                else if (smallState == 1 && trigger.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    leftPos1 = leftController.position;
                    changeHint(measureArmLengthRight);
                    smallState = 2;
                }
                else if (smallState == 2 && trigger.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    rightPos1 = rightController.position;
                    changeHint(measureArmHeightLeft);
                    smallState = 3;
                }
                else if (smallState == 3 && trigger.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    leftPos2 = leftController.position;
                    changeHint(measureArmHeightRight);
                    smallState = 4;
                }
                else if (smallState == 4 && trigger.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    rightPos2 = rightController.position;
                    Vector3 o = (leftPos1 + rightPos1) / 2;
                    GameObject.Find("OriPos").transform.position = new Vector3(o.x, -1, o.z);
                    leftPos1r = leftPos1 - o;
                    leftPos2r = leftPos2 - o;
                    rightPos1r = rightPos1 - o;
                    rightPos2r = rightPos2 - o;
                    float rex = (Mathf.Abs(leftPos2r.x) + Mathf.Abs(rightPos2r.x)) / 2;
                    float rey = (Mathf.Abs(leftPos1r.y) + Mathf.Abs(rightPos1r.y)) / 2;
                    float rez = (Mathf.Abs(leftPos2r.z) + Mathf.Abs(rightPos2r.z)) / 2;
                    move.centralPoint1 = new Vector3(-rex, rey, rez) + o ;
                    move.centralPoint2 = new Vector3(rex,rey,rez) + o;
                    float rer = ((leftPos1-move.centralPoint1).magnitude + (leftPos2 - move.centralPoint1).magnitude + (rightPos2 - move.centralPoint2).magnitude + (rightPos1 - move.centralPoint2).magnitude) / 4;
                    move.halfHandDistance = rer;
                    LogOut.printLog("radius:" + rer);
                    nextLabState();
                }
            }
            else if(smallState == 0)
            {
                //do nothing
            }
            else if (limit <= maxLimit - 1)//smallState != 0, 已經進入點擊階段
            {
                limit++;
                updateProgressHint();
                changeBackColor();
                Invoke("changeRandNum", timerGap);
            }
            else
            {
                if(smallState >= maxSmallState)
                {
                    nextLabState();
                }
                else
                {
                    waitingForSpace = true;
                    changeHint(afterTurn);
                    ballsDisappear();
                    sideBallsAppear();
                    hintText.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            Debug.Log("limit : " + limit);
            Debug.Log("labState : " + labState);
            Debug.Log("smallState : " + smallState);

        }
        //Debug.Log("???"+waitingForSpace);
        if (Input.GetKeyDown(KeyCode.Space) && waitingForSpace && !(labState == 0))
        {
            sideBallsDisappear();
            LogOut.printLog("round "+labState+
                ", central 1:(" +move.centralPoint1.x+","+
                 +move.centralPoint1.y + "," +
                  +move.centralPoint1.z +")"+ 
                  ", central 2:(" + move.centralPoint2.x + "," +
                 +move.centralPoint2.y + "," +
                  +move.centralPoint2.z + ")"); 
            nextSmallState();
            waitingForSpace = false;
        }

        if(!LogOut.fixedMode && labState != 0)
        {
            move.moveBall(labState - 1);
        }

        disFromOriDirection = new Vector4(cameraTransform.rotation.w-1, cameraTransform.rotation.x, cameraTransform.rotation.y, cameraTransform.rotation.z).magnitude;
        //Debug.Log(disFromOriDirection);
        //Debug.Log(cameraTransform.rotation);

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (labState >=1)
            {
                limit = 1;
                updateProgressHint();
                LogOut.printLog("回退成功");
                changeBackColor();
                Invoke("changeRandNum", timerGap);
                changeHint("回退成功" + triedNum);
                triedNum++;
                Debug.Log("ddddddddddddddddd");
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && smallState == 0) {
            Vector3 offset1 = new Vector3(0, -0.05f, 0);
            for (int i = 0; i < 50; i++)
            {
                findSideBall[i].transform.position += offset1;
            }
            move.centralPoint1 += offset1;
            move.centralPoint2 += offset1;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && smallState == 0)
        {
            Vector3 offset2 = new Vector3(0, 0.05f, 0);
            for (int i = 0; i < 50; i++)
            {
                findSideBall[i].transform.position += offset2;
            }
            move.centralPoint1 += offset2;
            move.centralPoint2 += offset2;
        }
    }

    void nextLabState()
    {
        switch (labState)
        {
            case 3:
                numbers.Clear();
                ballsDisappear();
                sideBallsDisappear();
                hintText.GetComponent<MeshRenderer>().enabled = true;
                changeHint(afterLab);
                break;
            default:
                limit = 0;
                labState++;
                smallState = 0;
                updateProgressHint();
                numbers.Clear();
                ballsDisappear();
                sideBallsAppear();
                move.moveBall(labState - 1);
                hintText.GetComponent<MeshRenderer>().enabled = true;
                changeHint(afterTurn);
                waitingForSpace = true;
                break;
        }
    }

    void nextSmallState()
    {
        if(smallState != maxSmallState)
        {
            smallState++;
            limit = 1;
            updateProgressHint();
            numbers.Clear();
            ballsAppear();
            sideBallsDisappear();
            hintText.GetComponent<MeshRenderer>().enabled = false;
            Invoke("changeRandNum", timerGap);
        }
        else
        {
            sideBallsDisappear();
            nextLabState();   
        }
    }

    void ready()
    {
        ballsAppear();
        sideBallsDisappear();
        changeBackColor();
        hintText.GetComponent<MeshRenderer>().enabled = false;
        Invoke("changeRandNum", timerGap);
        limit++;
        updateProgressHint();
        smallState = 1;
    }

    void init() {
        // for initialize the whole flow
        changeHint(measureArmLengthLeft);
        ballsDisappear();

        hintText.GetComponent<MeshRenderer>().enabled = true;
    }

    // auto setting coordinate of balls in interface
    void initBallInterface() {
        for (int i = 0; i < 50; i++) {
            findBall[i].transform.SetPositionAndRotation(new Vector3(getX(i), getY(i), getZ(i)), findBall[i].transform.rotation);
        }
    }
    void changeHint(string newhint)
    {
        hintString = newhint;
        hintText.GetComponent<TextMesh>().text = hintString;
        hintTextBack.GetComponent<TextMesh>().text = hintString;
        LogOut.printLog("frontHint changed to " + hintString);
    }

    void updateProgressHint() {
        progressHintString = "当前小球 " + limit.ToString() + "/"+ maxLimit + " 当前轮数 " + smallState.ToString() + "/"+maxSmallState.ToString()+ " 当前距离 "+labState.ToString()+"/"+maxLabState.ToString();
        progressHintText.GetComponent<TextMesh>().text = progressHintString;
        LogOut.printLog("progress changed to " + progressHintString);
    }

    // 
    void changeRandNum() {

        changeBackColor();

        if(randomNumbers == null || hasused == maxLimit)
        {
            randomNumbers = GetRandomUnrepeatArray(0, 49, maxLimit);
            hasused = 0;
        }
        randNum = randomNumbers[hasused];
        hasused++;

        //changeHint("请点击"+randNum+"号小球");


        //numbers.Add(randNum);
        changeColor();
        audioHint.Play();

        isWaiting = true;

        LogOut.printLog(randNum + "号小球亮起");
    }

    // change into a new color
    void changeColor() {
        if (ballsDisappeared) {
            return;
        }
        Renderer render1 = findSideBall[randNum].GetComponent<Renderer>();
        Renderer render2 = findBall[randNum].GetComponent<Renderer>();
        render1.material.color = lightBall;
        render2.material.color = lightBall;
    }
    
    void changeBackColor() {
        for (int i = 0; i < 50; i++)
        {
            Renderer render1 = findSideBall[i].GetComponent<Renderer>();
            Renderer render2 = findBall[i].GetComponent<Renderer>();
            render1.material.color = backBall;
            render2.material.color = backBall;
        }
    }

    void ballsDisappear() {
        if (ballsDisappeared) {
            return ;
        }
        changeBackColor();
        //for (int i = 0; i < 50; i++) {
        //    if (findBall[i] != null) {
        //        findBall[i].GetComponent<MeshRenderer>().enabled = false;
        //    }
        //}
        ballsDisappeared = true;
        progressHintText.GetComponent<MeshRenderer>().enabled = false;
    }

    void sideBallsDisappear()
    {
        if (sideBallsDisappeared)
        {
            return;
        }
        for (int i = 0; i < 50; i++)
        {
            if (findSideBall[i] != null)
            {
                findSideBall[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
        sideBallsDisappeared = true;
    }

    void sideBallsAppear()
    {
        if (!sideBallsDisappeared)
        {
            return;
        }
        for (int i = 0; i < 50; i++)
        {
            if (findSideBall[i] != null)
            {
                findSideBall[i].GetComponent<MeshRenderer>().enabled = true;
            }
        }
        sideBallsDisappeared = false;
    }


    void ballsAppear() {
        for (int i = 0; i < 50; i++)
        {
            if (findBall[i] != null)
            {
                findBall[i].GetComponent<MeshRenderer>().enabled = true;
            }
        }
        ballsDisappeared = false;
        progressHintText.GetComponent<MeshRenderer>().enabled = true;
    }

    float getSide(int i) {
        return i / 25 == 0 ? -1 : 1;
    }

    float getX(int i) {
        float offset = 4 - (i % 5);
        return offset * getSide(i) * gapBetweenBalls + gapBetweenCenter * getSide(i);
    }

    float getY(int i)
    {
        float offset = 4 - (i / 5) % 5;
        return offset * gapBetweenBalls * 1.414f + gapBetweenCenter + Yoffset;
    }

    float getZ(int i)
    {
        float offset = i % 5;
        return offset * gapBetweenBalls + Zoffset;
    }

    public int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count) { System.Random rnd = new System.Random(); int length = maxValue - minValue + 1; byte[] keys = new byte[length]; rnd.NextBytes(keys); int[] items = new int[length]; for (int i = 0; i < length; i++) { items[i] = i + minValue; } System.Array.Sort(keys, items); int[] result = new int[count]; System.Array.Copy(items, result, count); return result; }
}
