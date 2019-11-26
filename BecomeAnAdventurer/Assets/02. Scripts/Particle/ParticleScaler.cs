using UnityEngine;

/// <summary>
/// 파티클 크기를 조절하는 스크립트
/// </summary>
public class ParticleScaler : MonoBehaviour
{
    [HideInInspector]
    public float scaleFactor = 1.0f; // 파티클 크기
    
    /// <summary>
    /// 자신과 모든 자식 오브젝트의 Scaling Mode를 Hierarchy로 변경합니다.
    /// </summary>
    public void ParticleScalingModeChange()
    {
        ParticleSystem[] particleSystemList = GetComponentsInChildren<ParticleSystem>(true); // true를 하면 비활성화 되어있는 오브젝트도 찾아준다.

        for (int i = 0; i < particleSystemList.Length; i++)
        {
            ParticleSystem.MainModule particle = particleSystemList[i].main;
            particle.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }
    }

    /// <summary>
    /// 자신과 모든 자식 오브젝트의 Scale 값을 scaleFactor값으로 변경합니다.
    /// </summary>
    public void ParticleScaleChange()
    {
        ParticleSystem[] particleSystemList = GetComponentsInChildren<ParticleSystem>(true);
        
        for(int i=0; i< particleSystemList.Length; i++)
        {
            particleSystemList[i].gameObject.transform.localScale = Vector3.one * scaleFactor;
        }
    }

    /// <summary>
    /// 외부 호출에 반응합니다.
    /// </summary>
    /// <param name="scaleFactor"></param>
    public void ParticleScaleChange(Vector3 scaleFactor)
    {
        ParticleSystem[] particleSystemList = GetComponentsInChildren<ParticleSystem>(true);

        for (int i = 0; i < particleSystemList.Length; i++)
        {
            particleSystemList[i].gameObject.transform.localScale = scaleFactor;
        }
    }
}
