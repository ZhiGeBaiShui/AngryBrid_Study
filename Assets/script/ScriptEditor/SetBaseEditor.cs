using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetBase))]
public class SetBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SetBase setBaseScript = (SetBase)target;

        // 绘制 `grounds` 列表字段（手动绘制，以便控制顺序）
        EditorGUILayout.PropertyField(serializedObject.FindProperty("grounds"), true);

        // 绘制 "Update Positions" 按钮
        if (GUILayout.Button("Update Positions"))
        {
            // 当点击按钮时调用 UpdatePositions 方法
            setBaseScript.UpdatePositions();
        }

        // 绘制 `startPosition` 和 `gapVector` 字段
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startPosition"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gapVector"));

        // 应用修改并更新 SerializedObject（确保参数更新）
        serializedObject.ApplyModifiedProperties();
    }
}