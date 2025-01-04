using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float birdWeight = 0.2f;
    public Transform leftPoint;
    public Transform rightPoint;
    public LineRenderer left;  //弹弓左边部分
    public LineRenderer right;  //弹弓右边部分
    public float maxSpringStretch = 1; 
    public Transform point;  //Bird物体所在的中心点
    private bool isClick = false;
    private SpringJoint2D sp;
    private Rigidbody2D rg;
    private void Awake()
    {
        sp = GetComponent<SpringJoint2D>();
        rg = GetComponent<Rigidbody2D>();
    }
    private void OnMouseDown() //按下鼠标
    {
        isClick = true;
        rg.isKinematic = true;
    }
    private void OnMouseUp() //抬起鼠标
    {
        isClick = false;
        rg.isKinematic = false;
        Invoke("Fly",0.1f);
        InitLine();
    }

    private void Fly()
    {
        sp.enabled = false;
    }
    private void Update()
    {
        if(isClick)
        {
            Vector3 now = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10) - point.position;
            if(now.magnitude >= maxSpringStretch)
            {
                now = (now / now.magnitude) * maxSpringStretch;
            }
            transform.position = point.position + now;
            Line(point.position + now + now / now.magnitude * birdWeight);
        }
    }
    private void InitLine()
    {
        right.SetPosition(0,rightPoint.position);
        right.SetPosition(1,point.position);

        left.SetPosition(0,leftPoint.position);
        left.SetPosition(1,point.position);
    }
    //划线操作
    private void Line(Vector3 now) //now用来表示与弹簧中心相对的边缘点
    {
        right.SetPosition(0,rightPoint.position);
        right.SetPosition(1,now);

        left.SetPosition(0,leftPoint.position);
        left.SetPosition(1,now);
        
    }
}
