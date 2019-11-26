using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트가 활성화 될 시, DelayTime 후 비활성화
/// </summary>
public class InvisibleObject : MonoBehaviour
{
    private GameObject obj;
    public float delayTime;

    public InvisibleObject (float delayTime){
        this.delayTime = delayTime;
    }

    private void Awake()
    {
        obj = this.gameObject;
    }

    private void OnEnable()
    {
        Invoke("invisible", delayTime);
    }

    private void invisible()
    {
        obj.SetActive(false);
    }
}
