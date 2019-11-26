using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUiManager : MonoBehaviour
{
    public static GameUiManager instance;
    [HideInInspector]
    public GameObject CanvasScreen;

    private void Awake()
    {
        instance = this;
    }

    [HideInInspector] public NoticeUiController noticeUiController;
    [HideInInspector] public BuffUiController buffUiController;
    [HideInInspector] public PlayerInfoUiController playerInfoUiController;
    [HideInInspector] public QuestListUiController questListUiController;

    public GameObject ui_PlayerDeath;
    public GameObject ui_FadeIn;

    public Sprite ICON_AttackSpeed;
    public Sprite ICON_MoveSpeed;
    public Sprite ICON_AttackPower;
    public Sprite ICON_CriticalHit;

    public GameObject Tooltip;


    void Start()
    {
        CanvasScreen = GameObject.FindWithTag("CANVAS_SCREEN");

        noticeUiController = CanvasScreen.GetComponent<NoticeUiController>();
        buffUiController = CanvasScreen.GetComponent<BuffUiController>();
        playerInfoUiController = CanvasScreen.GetComponent<PlayerInfoUiController>();
        questListUiController = CanvasScreen.GetComponent<QuestListUiController>();

        ui_PlayerDeath.SetActive(false);
        ui_FadeIn.SetActive(false);
    }

    // 알림
    public void Notice(string msg, bool isSpecial)
    {
        noticeUiController.StartAnim(msg, isSpecial);
    }
    
    // 버프 UI
    public void BuffImg(Buff buff, float duration)
    {
        buffUiController.StartAnim(buff, duration);
    }

    // 툴팁 ON
    public void ShowTooltip(string str, Vector3 position)
    {
        Tooltip.transform.position = position;
        Tooltip.GetComponentInChildren<Text>().text = str;
        Tooltip.SetActive(true);
    }
    // 툴팁 OFF
    public void HideTooltip()
    {
        Tooltip.SetActive(false);
    }

    // UI 전체 조절
    public void ClearUI()
    {
        CanvasScreen.GetComponent<CanvasGroup>().alpha = 0;
    }
    public void ShowUI()
    {
        CanvasScreen.GetComponent<CanvasGroup>().alpha = 1;
    }

    // 플레이어 사망
    public void ShowPlayerDie()
    {
        ui_PlayerDeath.SetActive(true);
    }
    public void HidePlayerDie()
    {
        ui_PlayerDeath.SetActive(false);
    }

    // 장면 변할 때, FadeIn
    public void ShowFadeIn()
    {
        ui_FadeIn.SetActive(false);
        ui_FadeIn.SetActive(true);
    }
    public void HideFadeIn()
    {
        ui_FadeIn.SetActive(false);
    }

    // PlayerInfo UI 세팅
    public void SetUIs(PlayerInfo playerInfo)
    {
        playerInfoUiController.Setting(playerInfo);
    }
    public void SetUiAttackPower(int attackPower)
    {
        playerInfoUiController.SetAttackPower(attackPower);
    }
    public void SetUiAttackSpeed(float attackSpeed)
    {
        playerInfoUiController.SetAttackSpeed(attackSpeed);
    }
    public void SetUiMoveSpeed(float moveSpeed)
    {
        playerInfoUiController.SetMoveSpeed(moveSpeed);
    }
    public void SetUiCiriticalHit(int criticalHit)
    {
        playerInfoUiController.SetCiriticalHit(criticalHit);
    }
}
