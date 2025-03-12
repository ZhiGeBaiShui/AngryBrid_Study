using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pig : Animal
{
    [Tooltip("是否存在眨眼动画。")]
    public bool isBlink = false; 
    // pig类组件一定存在Animator组件
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        if(isBlink == true)
        {
            StartCoroutine(Blink());
        }
    }
    IEnumerator Blink()
    {
        float random = 0f;
        while(true)
        {
            random = Random.Range(5.0f, 10.0f);
            yield return new WaitForSeconds(random);
            animator.SetTrigger("Blink");
        }
    }
    private GameManage instance;
    public GameObject boom;
    public GameObject score;
    [Tooltip("游戏胜利的前提是必须消灭所有value为true的对象。")]
    public bool value = false; // 判断是猪还是可破坏物体
    //死亡得分
    public Vector3 boomLocalScale = new(1f, 1f, 1f);
    [Tooltip("存在分数动画的前提下，这个参数的作用是设置分数动画的缩放大小。")]
    public Vector3 scoreScale = new(1, 1, 1); //用来确定分数的缩放等级
    private bool alive = true;//判断是否活着，避免多次进入死亡处理的函数
    //Score分数需要变换的最终大小
    public float hurtSpeed = 5f; //默认碰撞发生时，相对速度每相差5就减少一滴血
    void Start()
    {
        instance = GameManage.Instance;
        if(value == true)
        {
            instance.pigCount += 1;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)//发生碰撞的时候
    {
        // 用以测试速度取值
        // Rigidbody2D rg = GetComponent<Rigidbody2D>();
        // Debug.Log(rg.velocity);
        // Debug.Log(other.relativeVelocity.magnitude);
        // Debug.Log((int)(other.relativeVelocity.magnitude / hurtSpeed));
        int x = damageHP((int)(other.relativeVelocity.magnitude / hurtSpeed));
        animator.SetInteger("HP", x);
        if(x <= 0 && alive == true)
        {
            alive = false;
            DieEnd();
        }
    }
    public override void DieEnd()
    {
        /*
        以下代码为游戏对象死亡以后会进行的各类处理
        目前仅仅存在两种游戏对象
        pig死亡会产生死亡动画与分数
        */
        ScorePlay();
        BoomPlay();
        Destroy(gameObject);
        if(value == true)
        {
            instance.pigCount -= 1;
        }
    }
    void BoomPlay()
    {
        // 未分配爆炸动画时，不进行预制件的实例化
        // 未分配爆炸动画一般而言意味其组件主体为bird
        if(boom != null)
        {
            Instantiate(boom, transform.position, Quaternion.identity);
            boom.transform.localScale = boomLocalScale;
        }
    }
    public float totalTime = 1f; // 控制score滑动变大的速率，整个过程持续totalTime秒
    public float stopTime = 1f; // 当score分数完成缩放以后，显示stopTime秒再消失
    void ScorePlay()
    {
        // 未分配score时，不会进行score的实例化
        // 未分配score一般而言意味其组件主体为bird
        if(score != null)
        {
            GameObject now = Instantiate(score, transform.position, Quaternion.identity);
            Score count = now.GetComponent<Score>();
            count.Init(scoreScale, scoreScale * 2.5f, totalTime, stopTime);
        }
    }
}