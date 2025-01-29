using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public abstract class Animal : MonoBehaviour , IDieEnd
{
    public int HP = 2;//血量使用HP表示,默认为2
    
    public int damageHP(int x)
    {
        HP -= x;
        //待实现不同血量的受伤效果
        return HP;
    }
    public abstract void DieEnd(); // DieEnd在其子类之中实现，敌对动物（猪）和友善动物（小鸟）的死亡过程不同
    
}
