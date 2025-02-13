using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarShow : MonoBehaviour
{
    private void Show()
    {
        GameManage.Instance.ShowStar();
    }
}
