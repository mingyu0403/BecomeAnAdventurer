using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnSceneLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
