using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private Transform target;
    [Tooltip("摄像机移动的速度")]
    public float smoothSpeed = 2;
    [Tooltip("摄像机的最左限制")]
    public float limitedLeft = -1.75f;
    [Tooltip("摄像机的最又限制")]
    public float limitedRight = 11f;
    

    public void SetTarget(Transform transform)
    {
        target = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector3 position = transform.position;
            position.x = Mathf.Clamp(target.position.x, limitedLeft, limitedRight);
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 2);
        }
    }
}
