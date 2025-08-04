using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManage : MonoBehaviour
{
    // 获取分数显示UI
    public TextMeshProUGUI scoreText = null;
    [HideInInspector]
    private int score = 0; // 统计分数
    public void AddScore(int now)
    {
        score += now;
        scoreText.text = $"Score:{score:0}";
    }
    private static GameManage _instance;
    public static GameManage Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManage>();
                if (_instance == null)
                {
                    // 如果场景中没有 GameManage，创建一个新的 GameObject
                    GameObject obj = new GameObject("GameManage");
                    _instance = obj.AddComponent<GameManage>();
                }
            }
            return _instance;
        }
    }
    /// <summary>
    /// 计算跳跃的总时间
    /// </summary>
    private float yJumpV0; // 跳跃的y轴初速度
    private float CalculateJumpTime(float startY, float peakY, float endY, float g)
    {
        // 上升时间
        yJumpV0 = Mathf.Sqrt(2 * g * (peakY - startY));
        float tUp = yJumpV0 / g;

        // 下降时间
        float tDown = Mathf.Sqrt(2 * (peakY - endY) / g);

        // 总时间
        return tUp + tDown;
    }

    private LineRenderer leftLine;  //弹弓左边部分
    private LineRenderer rightLine;  //弹弓右边部分
    private Transform leftPoint;
    private Transform rightPoint;
    public List<Transform> birds; // 此弹弓绑定的小鸟
    [Tooltip("不要修改这个公共变量的数量，显示只是为了方便调试过程。")]
    public int pigCount = 0; //记录存活猪的数量，已经进行了动态的计数，不需要进行设置
    public Vector3 gapBird = new(-0.5f, 0, 0); // 用来设置小鸟之间的间隙，负数表示在弹弓左边
    
    /// <summary>
    /// 此函数用来初始化小鸟最初在的位置
    /// </summary>
    public void InitBirdPosition()
    {
        int cnt = 0;
        foreach(Transform nowBird in birds)
        {
            nowBird.position = singShotStart + cnt * gapBird; // 计算小鸟应该在的位置
            cnt++;
        }
    }

    
    public float slingShotHeight = 1.49735f; // 弹弓与地面的距离
    private Vector3 singShotStart; //小鸟上弹弓的起始位置
    private Vector3 singShotEnd;
    public float xJump = 1f; //小鸟上弹弓的起跳位置
    public float heightJump = 1f; //小鸟起跳超过就绪位置多高
    const float gJump = 9.81f; //重力加速度
    private float timeJump;//小鸟的跳跃时间
    private FollowTarget cameraFollowTarget; //绑定当前场景的相机
    void Awake()
    {
        cameraFollowTarget = Camera.main.GetComponent<FollowTarget>();
        //初始化分数
        AddScore(0);
        if(scoreText == null)
        {
            Debug.Log("Don't find scoreText.Please set it.");
        }
        if(_instance != null)
        {
            Debug.Log("出现两个相同的GameManage的实例对象");
            Destroy(gameObject);
        }
        _instance = this;
        
        singShotStart = transform.position; // 获得弹弓本体的坐标
        singShotStart.y -= slingShotHeight; // 通过计算得到起跳位置
        singShotStart.x -= xJump;

        singShotEnd = transform.position ; // 起点的终点是弹弓本体的中心点

        timeJump = CalculateJumpTime(singShotStart.y, singShotEnd.y + heightJump, singShotEnd.y, gJump);

        InitBirdPosition();

        leftPoint = transform.GetChild(0).GetChild(0);
        rightPoint = transform.GetChild(1).GetChild(0);
        leftLine = leftPoint.GetComponent<LineRenderer>();
        rightLine = rightPoint.GetComponent<LineRenderer>();

        /*
        物体索引格式如下:
        Slingshot
        -Left
        --LeftPoint
        -Right
        --RightPoint
        */

    }
    /// <summary>
    /// 弹簧的绑定操作
    /// </summary>
    private void BindBirdVariables(Bird readyBird) 
    {
        SpringJoint2D sp = readyBird.GetComponent<SpringJoint2D>();
        sp.connectedBody = GetComponent<Rigidbody2D>();
        sp.enabled = true;
        readyBird.Singshot(leftPoint, rightPoint, leftLine, rightLine, transform.GetComponent<Rigidbody2D>(), GetComponent<GameManage>());
        
    }
    private void PlayBirdsReadyAnimation(Bird readyBird, float xCount) // xCount表示小鸟的偏移量，不是所有小鸟都在第一个位置
    {
        Vector3 v0 = new((xJump + xCount) / timeJump, yJumpV0, 0);
        Rigidbody2D rd = readyBird.GetComponent<Rigidbody2D>();
        rd.bodyType = RigidbodyType2D.Dynamic;
        rd.velocity = v0;
        StartCoroutine(FinishReady(readyBird));
        StartCoroutine(BirdFlip(readyBird.GetComponent<Transform>(), timeJump));
    }
    IEnumerator FinishReady(Bird readyBird) // 结束运动
    {
        yield return new WaitForSeconds(timeJump);
        SpringJoint2D SJ = readyBird.GetComponent<SpringJoint2D>();
        Rigidbody2D rd = readyBird.GetComponent<Rigidbody2D>();

        rd.GetComponent<Transform>().position = singShotEnd;
        rd.isKinematic = true;
        BindBirdVariables(readyBird);
        rd.velocity = new Vector3(0, 0, 0);

    }
    private int birdNumber = 0; //记录此时到第几只小鸟飞行了
    public void BirdReadyShot()
    {
        if (pigCount == 0) //如果小猪已经死完了
        {
            OnVictory();
            return;
        }
        if (birdNumber == birds.Count) //如果发现是最后一只小鸟
        {
            OnDefeat();
            return;
        }
        // 在场景开始，和飞出小鸟结束以后调用
        Transform readyBird = birds[birdNumber];
        cameraFollowTarget.SetTarget(birds[birdNumber].transform);
        PlayBirdsReadyAnimation(readyBird.GetComponent<Bird>(), -birdNumber * gapBird.x);
        birdNumber++;

    }
    public GameObject win;
    public GameObject lose;
    private void OnVictory() //游戏胜利函数
    {
        
        win.SetActive(true);
    }
    public int oneStar = 10000;
    public int twoStar = 20000;
    public int threeStar = 40000;
    private void OnDefeat() //游戏失败函数
    {
        lose.SetActive(true);
    }
    void Start()
    {
        BirdReadyShot();
    }
    IEnumerator BirdFlip(Transform readyBird, float timeSum) //小鸟翻跟头,传入准备的小鸟，翻跟头的总时间
    {
        Quaternion startRotation = readyBird.rotation;
        Quaternion midRotation = startRotation * Quaternion.Euler(0, 0, 180f);
        Quaternion endRotation = midRotation * Quaternion.Euler(0, 0, 180f);
        float now = 0;
        timeSum /= 2;
        while(now / timeSum <= 1)
        {
            readyBird.rotation = Quaternion.Slerp(startRotation, midRotation, now / timeSum);
            now += Time.deltaTime;
            yield return null;
        }
        now = 0;
        while(now / timeSum <= 1)
        {
            readyBird.rotation = Quaternion.Slerp(midRotation, endRotation, now / timeSum);
            now += Time.deltaTime;
            yield return null;
        }
        readyBird.rotation = Quaternion.Slerp(startRotation, endRotation, 1);
    }
    void OnDestroy()
    {
        _instance = null;
    }
}
