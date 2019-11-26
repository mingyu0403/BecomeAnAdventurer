using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    [Header("플레이어 정보")]
    public string nickname;

    public int level = 1;

    public int hp
    {
        get
        {
            int result = 100 + ((level-1) * 10); // 기본 값 + 레벨당 추가 값 
            return result;
        }
    }
    public int currHp = 0;

    public int mp
    {
        get
        {
            int result = 100 + ((level-1) * 5); // 기본 값 + 레벨당 추가 값 
            return result;
        }
    }
    public int currMp = 0;

    public int exp
    {
        get
        {
            int result = 30 + (level  * 20); // 기본 값 + 레벨당 추가 값 
            return result;
        }
    }
    public int currExp = 0;

    // 공격 파워
    private int _attackPowerStatic;
    public int attackPowerStatic
    {
        set
        {
            _attackPowerStatic = value;
        }
        get
        {
            int result = _attackPowerStatic + ((level-1) * 5); // 기본 값 + 레벨당 추가 값 
            return result;
        }
    }

    private int _attackPowerAdd = 0;
    public int attackPowerAdd
    {
        set
        {
            _attackPowerAdd = value;
            GameUiManager.instance.SetUiAttackPower(attackPower); // Ui랑 연동
        }
        get
        {
            return _attackPowerAdd;
        } 
    }
    private float _attackPowerMul = 1;
    public float attackPowerMul
    {
        set
        {
            _attackPowerMul = value;
            GameUiManager.instance.SetUiAttackPower(attackPower);
        }
        get
        {
            return _attackPowerMul;
        }
    }
    public int attackPower
    {
        get
        {
            float result = (attackPowerStatic + attackPowerAdd) * attackPowerMul;
            return Mathf.RoundToInt(result); // 반올림
        }
    }

    // 공격 속도
    public float attackSpeedStatic;
    private float _attackSpeedMul = 1;
    public float attackSpeedMul
    {
        set
        {
            _attackSpeedMul = value;
            GameUiManager.instance.SetUiAttackSpeed(attackSpeed);
        }
        get
        {
            return _attackSpeedMul;
        }
    }

    public float attackSpeed
    {
        get { return (attackSpeedStatic * attackSpeedMul); }
    }

    // 이동 속도
    public float _moveSpeedStatic;
    public float moveSpeedStatic
    {
        set
        {
            _moveSpeedStatic = value;
        }
        get
        {
            float result = _moveSpeedStatic + ((level - 1) * 0.3f); // 기본 값 + 레벨당 추가 값 
            return result;
        }
    }
    private float _moveSpeedAdd = 0;
    public float moveSpeedAdd
    {
        set
        {
            _moveSpeedAdd = value;
            GameUiManager.instance.SetUiMoveSpeed(moveSpeed);
        }
        get
        {
            return _moveSpeedAdd;
        }
    }

    private float _moveSpeedMul = 1f;
    public float moveSpeedMul
    {
        set
        {
            _moveSpeedMul = value;
            GameUiManager.instance.SetUiMoveSpeed(moveSpeed);
        }
        get
        {
            return _moveSpeedMul;
        }
    }
    public float moveSpeed
    {
        get { return (moveSpeedStatic + moveSpeedAdd) * moveSpeedMul; }
    }

    // 크리 확률
    public int _criticalHitStatic;
    public int criticalHitStatic
    {
        set
        {
            _criticalHitStatic = value;
        }
        get
        {
            int result = _criticalHitStatic + ((level - 1) * 3); // 기본 값 + 레벨당 추가 값 
            return result;
        }
    }

    private int _criticalHitAdd = 0;
    public int criticalHitAdd
    {
        set
        {
            _criticalHitAdd = value;
            GameUiManager.instance.SetUiCiriticalHit(criticalHit);
        }
        get
        {
            return _criticalHitAdd;
        }
    }
    
    private float _criticalHitMul = 1f;
    public float criticalHitMul
    {
        set
        {
            _criticalHitMul = value;
            GameUiManager.instance.SetUiCiriticalHit(criticalHit);
        }
        get
        {
            return _criticalHitMul;
        }
    }
    
    public int criticalHit
    {
        get
        {
            float cri = (criticalHitStatic + criticalHitAdd) * criticalHitMul;
            int result = Mathf.RoundToInt(cri); // 반올림

            if (result > 100) // 100을 넘을 수 없다!!
            {
                result = 100;
            }

            return result;
        }
    }

    [Header("단축키 정보")]
    public KeyCode SHORTKEY_ATTACK = KeyCode.Q;
    public KeyCode SHORTKEY_JUMP = KeyCode.Space;
    public KeyCode SHORTKEY_SKILL1 = KeyCode.Alpha1;
    public KeyCode SHORTKEY_SKILL2 = KeyCode.Alpha2;
    public KeyCode SHORTKEY_SKILL3 = KeyCode.Alpha3;

}