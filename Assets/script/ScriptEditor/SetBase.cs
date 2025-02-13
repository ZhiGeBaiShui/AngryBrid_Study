using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class SetBase : MonoBehaviour
{
    public List<Transform> grounds; // 获取场景中的地面，由多个Ground游戏对象构成
    public Vector3 startPosition = new (0, 0, 0); // 设置第一个Ground游戏对象的起始位置
    public Vector3 gapVector = new (3.4f, 0, 0); // 设置后一个Ground游戏对象与前一个的距离
    public void UpdatePositions()
    {
        Vector3 startNow = startPosition;
        foreach(Transform groundNow in grounds)
        {
            groundNow.localPosition = startNow;
            startNow += gapVector;
        }
    }
}
