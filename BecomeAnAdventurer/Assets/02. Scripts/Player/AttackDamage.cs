using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    PlayerManager playerManager;

    public int attackStatic; // 기본 데미지

    public int attackPowerAdd = 0;
    public float attackPowerMul = 1f;

    private void Start()
    {
        playerManager = GameManager.instance.playerManager;
    }

    public DamageUIPooler.AttackInfo CalculationAttackDamage()
    {
        DamageUIPooler.AttackInfo attackInfo = playerManager.Attack();

        // 계산식
        attackInfo.damage = Mathf.RoundToInt((attackInfo.damage + attackPowerAdd) * attackPowerMul);
        attackInfo.damage += attackStatic;

        return attackInfo;
    }
}
