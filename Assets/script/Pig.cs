using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pig : MonoBehaviour
{
    public GameObject boom;
    public GameObject score;
    //死亡得分
    public Vector3 boomLocalScale;
    private Vector3 scoreScale = new Vector3(1, 1, 1);
    //Score分数需要变换的最终大小
    private void Start()
    {
        //分数对象不为空
        if(score != null)
        {
             //计算Score分数最初的大小，由x轴的宽度决定
            Renderer now = GetComponent<Renderer>();
            //保证Renderer存在，避免报错
            if(now != null)
            {
                float width = now.bounds.size.x;
                now = score.GetComponent<Renderer>();
                float scoreWidth = now.bounds.size.x;
                scoreScale = scoreScale * width / scoreWidth * 1.2f;
           }
        }
    }
    public float hurtSpeed = 5f; //默认碰撞发生时，相对速度每相差5就减少一滴血
    private void OnCollisionEnter2D(Collision2D other)
    {
        BodyHP bH = GetComponent<BodyHP>();
        // 用以测试速度取值
        // Rigidbody2D rg = GetComponent<Rigidbody2D>();
        // Debug.Log(rg.velocity);
        // Debug.Log(other.relativeVelocity.magnitude);
        // Debug.Log((int)(other.relativeVelocity.magnitude / hurtSpeed));
        int x = bH.damageHP((int)(other.relativeVelocity.magnitude / hurtSpeed));
        if(x <= 0)
        {
            dieEnd();
        }
    }
    private void dieEnd()
    {
        Destroy(gameObject);
        /*
        以下代码为游戏对象死亡以后会进行的各类处理
        目前仅仅存在两种游戏对象
        pig死亡会产生死亡动画与分数
        为保证动画全部同时进行，采取协程操作
        */
        StartCoroutine(WaitForAllTasksToComplete());
    }
    IEnumerator WaitForAllTasksToComplete()
    {
        var task1 = StartCoroutine(BoomPlay());
        //task1处理爆炸动画
        var task2 = StartCoroutine(ScorePlay());
        //task2处理分数播放
        //等待两个任务都完成
        yield return new WaitUntil(() => task1 == null && task2 == null);
    }
    IEnumerator BoomPlay()
    {
        //未分配爆炸动画时，不进行预制件的实例化
        //未分配爆炸动画一般而言意味其组件主体为bird
        if(boom != null)
        {
            Instantiate(boom, transform.position, Quaternion.identity);
            boom.transform.localScale = boomLocalScale;
        }
        
        yield return new WaitForSeconds(1f);
    }
    IEnumerator ScorePlay()
    {
        //未分配score时，不会进行score的实例化
        //未分配score一般而言意味其组件主体为bird
        if(score != null)
        {
            Instantiate(score, transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
    }
}