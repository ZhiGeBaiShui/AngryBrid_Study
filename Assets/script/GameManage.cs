using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    //计算跳跃的总时间
    private float yJumpV0; // 跳跃的y轴初速度
    float CalculateJumpTime(float startY, float peakY, float endY, float g)
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
    public List<Bird> birds; // 此弹弓绑定的小鸟
    public List<Pig> pigs;
    private int pigCount; //记录存活猪的数量

    
    public float birdSpacing = 0.5f; // 用来设置小鸟之间的间隙，在弹弓左边
    
    // 此函数用来初始化小鸟最初在的位置
    private void InitBirdPosition()
    {
        int cnt = 0;
        foreach(Bird nowBird in birds)
        {
            Transform coordinate = nowBird.GetComponent<Transform>(); //坐标操作
            coordinate.position = singShotStart - new Vector3(cnt * birdSpacing ,  0, 0); // 计算小鸟应该在的位置
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
    void Awake()
    {
        pigCount = pigs.Count;
        
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
    private void BindBirdVariables(Bird readyBird) // 弹簧的绑定操作
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
        if(pigCount == 0) //如果小猪已经死完了
        {
            OnVictory();
            return ;
        }
        if(birdNumber == birds.Count) //如果发现是最后一只小鸟
        {
            OnDefeat();
            return ;
        }
        // 在场景开始，和飞出小鸟结束以后调用
        Bird readyBird = birds[birdNumber];
        PlayBirdsReadyAnimation(readyBird, birdNumber * birdSpacing);
        birdNumber++;

    }
    private void OnVictory() //游戏胜利函数
    {

    }
    private void OnDefeat() //游戏失败函数
    {

    }
    void Start()
    {
        BirdReadyShot();
    }
    IEnumerator BirdFlip(Transform readyBird, float timeSum) //小鸟翻跟头
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
}
