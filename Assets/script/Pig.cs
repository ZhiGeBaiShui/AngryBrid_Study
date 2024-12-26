using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pig : MonoBehaviour
{
    public float hurtSpeed = 5f; //默认碰撞发生时，相对速度每相差5就减少一滴血
    private void OnCollisionEnter2D(Collision2D other)
    {
        bodyHP bH = GetComponent<bodyHP>();
        // 用以测试速度取值
        // Rigidbody2D rg = GetComponent<Rigidbody2D>();
        // Debug.Log(rg.velocity);
        // Debug.Log(other.relativeVelocity.magnitude);
        // Debug.Log((int)(other.relativeVelocity.magnitude / hurtSpeed));
        bH.damageHP((int)(other.relativeVelocity.magnitude / hurtSpeed));
        
    }
}