using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pig : Animal
{
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
    [Tooltip("请注意，要保证最后一个状态——即濒死状态对应的HP为0。")]
    public List<HealthSpriteMap> hurtMap;
    private SpriteRenderer lookNow;
    void Awake()
    {
        lookNow = GetComponent<SpriteRenderer>();
    }
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
        int cntBack = hurtMap.Count - 1;
        while(cntBack >= 0 && x < hurtMap[cntBack].hp)
        {
            hurtMap.RemoveAt(cntBack);
            cntBack--;
        }
        if(x <= 0 && alive == true)
        {
            alive = false;
            DieEnd();
        }
        if (lookNow != null && hurtMap.Count > 0)
        {
            lookNow.sprite = hurtMap[cntBack].sprite; // 设置精灵
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