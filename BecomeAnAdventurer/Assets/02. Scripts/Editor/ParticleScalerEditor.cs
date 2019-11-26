using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ParticleScaler))]
public class ParticleScalerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ParticleScaler scaler = (ParticleScaler)target;

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("-- 파티클 스케일 변경 툴 --");
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("ScalingMode가 Hierarchy로 되어있어야 Scale 변경이 원활하게 됩니다.");
        EditorGUILayout.LabelField("처음 한번만 실행하면 됩니다.");
        
        if (GUILayout.Button("Hierarchy로 변경"))
        {
            scaler.ParticleScalingModeChange();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        scaler.scaleFactor = EditorGUILayout.FloatField("파티클 크기", scaler.scaleFactor);

        if (GUILayout.Button("파티클 스케일 변경"))
        {
            scaler.ParticleScaleChange();
        }


    }
}
