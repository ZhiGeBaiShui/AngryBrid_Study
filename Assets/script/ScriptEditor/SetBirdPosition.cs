using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class SetBirdPosition : MonoBehaviour
{
    private List<Transform> birds; // 获取游戏中的Bird游戏对象
    [Tooltip("将一个装在了GameMnanage的弹弓游戏对象放入。\n按钮可以为此弹弓所拥有的所有小鸟自动排序。")]
    public GameManage slingShot; // 拿到弹弓的游戏管理组件
    public void UpdateBirdPosition()
    {
        Vector3 startPosition = slingShot.GetComponent<Transform>().position - new Vector3(slingShot.xJump, slingShot.slingShotHeight, 0); // 设置第一个鸟的起始位置
        // slingShot.xJump表示第一只鸟在弹弓左边多远，slingShot.slingShotHeight表示弹弓和地面的高度

        Vector3 gapVector = slingShot.gapBird; // 设置后一个鸟与前一个鸟之间的距离，负数表示在左边
        birds = slingShot.birds;
        Vector3 startNow = startPosition;
        foreach(Transform nowBird in birds)
        {
            nowBird.position = startNow;
            startNow += gapVector;
        }
    }
}
