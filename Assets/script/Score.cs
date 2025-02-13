using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// 此脚本无法实现单独运行，调用此脚本的时候需要设置好传入的三个变量值
/// </summary>
public class Score : MonoBehaviour
{
    //为了防止遗忘调用，这里会设置初始值，保证项目整体可以运行
    private Vector3 start = new(0.5f, 0.5f, 0.5f), end = new(1.5f, 1.5f, 1.5f);//表示放大动画的终点
    private float totalTime = 1f;//表示放大动画进行的总时间
    private float stopTime;//放大完成以后分数显示stopTime秒后消失


    //Init 用以初始化分数动画的初始大小和最终大小
    //仅当调用Init函数以后，分数放大动画才会进行
    //start表示缩放动画的起始缩放，end表示终点缩放，tatalTime表示缩放的持续总时间，stopTime放大完成以后分数显示stopTime秒后消失
    public void Init(Vector3 start, Vector3 end, float totalTime, float stopTime)
    {
        //this强调start，end，speed为当前Score类中的变量
        //三个传入参数的具体值需要另外的脚本进行确定
        //使用此脚本时，一定要记住设置传入值
        transform.localScale = start;
        this.start = start;
        this.end = end;
        this.totalTime = totalTime;
        this.stopTime = stopTime;
        StartCoroutine(ChangeScale());
    }
    public IEnumerator ChangeScale()
    {
        float t = 0;
        //滑动缩放的过程因子
        //totalTime表示整个缩放过程持续totalTime秒
        while(transform.localScale != end)
        {
            transform.localScale = Vector3.Lerp(start, end, t);
            t += 1f / totalTime * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(stopTime);
        Destroy(gameObject);
    }
}
