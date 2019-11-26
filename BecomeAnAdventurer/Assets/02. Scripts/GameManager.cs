using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector] public PlayerManager playerManager;

    private void Awake()
    {
        instance = this;

        // 초기 값
        HideMouseCursor();
        playerManager = GameObject.FindWithTag("PLAYER").GetComponent<PlayerManager>();
    }


    void Start()
    {
        GameUiManager.instance.ShowFadeIn();
    }

    public void ShowMouseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void HideMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
