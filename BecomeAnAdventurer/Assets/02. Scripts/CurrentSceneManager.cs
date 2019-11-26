using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSceneManager : MonoBehaviour
{
    public string mapDescription;

    // 순간 이동 장소 지정
    public Transform[] PortalList;

    private void Start()
    {
        Invoke("MapDescription", 1f);
    }

    public void MapDescription()
    {
        if (mapDescription == "")
        {
            return;
        }
        GameUiManager.instance.Notice(mapDescription, false);
    }
}
