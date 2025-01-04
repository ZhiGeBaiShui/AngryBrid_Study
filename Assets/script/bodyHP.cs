using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BodyHP : MonoBehaviour
{
    public int HP = 2;//血量使用HP表示,默认为2
    
    public int damageHP(int x)
    {
        HP -= x;
        //待实现不同血量的受伤效果
        return HP;
    }
    
}
