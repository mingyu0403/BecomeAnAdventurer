using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfoUiController : MonoBehaviour
{
    public TextMeshProUGUI text_AttackPower;
    public TextMeshProUGUI text_AttackSpeed;
    public TextMeshProUGUI text_MoveSpeed;
    public TextMeshProUGUI text_CriticalHit;

    // 전체 세팅
    public void Setting(PlayerInfo playerInfo)
    {
        SetAttackPower(playerInfo.attackPower);
        SetAttackSpeed(playerInfo.attackSpeed);
        SetMoveSpeed(playerInfo.moveSpeed);
        SetCiriticalHit(playerInfo.criticalHit);
    }

    public void SetAttackPower(int attackPower)
    {
        text_AttackPower.text = attackPower + "";
    }
    public void SetAttackSpeed(float attackSpeed)
    {
        text_AttackSpeed.text = attackSpeed.ToString("F2"); // 소수점 2자리까지

    }
    public void SetMoveSpeed(float moveSpeed)
    {
        text_MoveSpeed.text = moveSpeed.ToString("F2");

    }
    public void SetCiriticalHit(int criticalHit)
    {
        text_CriticalHit.text = criticalHit + "%";
    }
}
