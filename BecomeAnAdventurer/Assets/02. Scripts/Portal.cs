using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string SceneName;
    public int portalNumber;

    public bool isMovePortal = true;

    private void OnTriggerEnter(Collider other)
    {
        if (isMovePortal && other.CompareTag("PLAYER"))
        {
            SaveCurrentSceneInfo.instace.SceneChange(SceneName, portalNumber);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            SaveCurrentSceneInfo.instace.canPlayerUsePortal = true;
        }
    }
}
