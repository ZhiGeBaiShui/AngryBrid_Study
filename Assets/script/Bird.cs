using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Bird : Animal
{
    //小鸟上弹弓的绑定操作，左右部分用于配合SpringJoint2D组件
    private LineRenderer leftLine;  //弹弓左边部分
    private LineRenderer rightLine;  //弹弓右边部分
    private Transform leftPoint;  //弹弓左边部分中心点
    private Transform rightPoint;  //弹弓右边部分中心点
    private bool singShotCheck = false; //检测小鸟此时是否处于弹弓就绪状态
    private Transform point;  //Bird物体所在的中心点
    private GameManage manage;
    public void Singshot(Transform leftPoint, Transform rightPoint, LineRenderer leftLine, LineRenderer rightLine, Rigidbody2D sinShotPoint, GameManage manage)
    {
        //上弹弓操作，所有传入参数由GameManager传入，此函数的也应只在GameManager类中调用
        sp.connectedBody = sinShotPoint; //确定连接的刚体，即确定弹弓
        this.leftLine = leftLine;
        this.rightLine = rightLine;
        this.leftPoint = leftPoint;
        this.rightPoint = rightPoint;
        this.manage = manage;
        point = manage.GetComponent<Transform>();
        singShotCheck = true;
    }
    public float birdWeight = 0.2f;
    public float maxSpringStretch = 1;
    private bool isClick = false;
    private SpringJoint2D sp;
    private Rigidbody2D rg;
    private TrailRenderer tr;
    private void Awake()
    {
        sp = GetComponent<SpringJoint2D>();
        rg = GetComponent<Rigidbody2D>();
        tr = transform.GetChild(0).GetComponent<TrailRenderer>();
    }
    private void OnMouseDown() //按下鼠标
    {
        // 仅当处于弹弓就绪状态的时候，按下鼠标才有效果
        if(singShotCheck == false)
        {
            return ;
        }
        isClick = true;
        rg.isKinematic = true;    
    }
    private void OnMouseUp() //抬起鼠标
    {
        if(singShotCheck == false)
        {
            return ;
        }
        singShotCheck = false; //小鸟只能飞行一次
        isClick = false;
        rg.isKinematic = false;
        tr.enabled = true;
        Invoke("Fly",0.1f);
        InitLine(); //恢复弹弓弹簧到原来的位置
    }
    private void Fly()
    {
        sp.enabled = false; //断开弹簧连接处，通过断开产生的弹力实现飞行
        Invoke("DieEnd", 10f); //小鸟销毁处理
    }
    public override void DieEnd()
    {
        manage.BirdReadyShot();
        Destroy(gameObject);
    }
    private void Update()
    {
        if(isClick)
        {
            Vector3 now = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10) - point.position;
            if(now.magnitude >= maxSpringStretch)
            {
                now = now / now.magnitude * maxSpringStretch;
            }
            transform.position = point.position + now;
            Line(point.position + now + now / now.magnitude * birdWeight);
        }
    }

    //恢复弹弓弹簧到原来的位置
    private void InitLine()
    {
        rightLine.SetPosition(0,rightPoint.position);
        rightLine.SetPosition(1,point.position);
        leftLine.SetPosition(0,leftPoint.position);
        leftLine.SetPosition(1,point.position);
    }

    //划线操作，让弹弓弹簧跟随小鸟
    private void Line(Vector3 now) //now用来表示与弹簧中心相对的边缘点
    {
        rightLine.SetPosition(0,rightPoint.position);
        rightLine.SetPosition(1,now);
        leftLine.SetPosition(0,leftPoint.position);
        leftLine.SetPosition(1,now);
        
    }
    private void OnCollisionEnter2D(Collision2D other) //发生碰撞的时候
    {
        if(tr != null && tr.enabled == true)
        {
            Destroy(tr);
            tr = null;
        }
    }
}
