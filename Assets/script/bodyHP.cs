using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyHP : MonoBehaviour
{
    public int HP = 2;//血量使用HP表示,默认为2

    private void Update()
    {
        if(HP <= 0)
        {
            dieEnd();
            return ;
        }
    }

    public void damageHP(int x)
    {
        HP -= x;
    }

    private void dieEnd()
    {
        Destroy(gameObject);
    }
}
