using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Character/Stats")]
public class HealthSpriteMap : ScriptableObject
{
    public Sprite sprite;
    public int hp;
}
