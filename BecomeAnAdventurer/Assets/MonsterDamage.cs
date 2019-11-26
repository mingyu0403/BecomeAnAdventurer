using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int attackDamage;

    public DamageUIPooler.AttackInfo CalculationAttackDamage()
    {
        DamageUIPooler.AttackInfo attackInfo;
        attackInfo.damage = this.attackDamage;
        attackInfo.damageType = DamageUIPooler.DamageType.BASIC;

        return attackInfo;
    }
}
