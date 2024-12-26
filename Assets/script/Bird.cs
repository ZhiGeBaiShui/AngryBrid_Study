using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float maxSpringStretch = 1; 
    public Transform point;
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
        }
    }
}
