using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetBirdPosition))]
public class SetBirdPositionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SetBirdPosition setBirdPositionScript = (SetBirdPosition)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("slingShot"));
        // 绘制 "Update Positions" 按钮
        if (GUILayout.Button("Update Positions"))
        {
            // 当点击按钮时调用 UpdatePositions 方法
            setBirdPositionScript.UpdateBirdPosition();        
        }
        // 应用修改并更新 SerializedObject（确保参数更新）
        serializedObject.ApplyModifiedProperties();
    }
}
