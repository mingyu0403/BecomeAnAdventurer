using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버프를 관리해줍니다.
/// </summary>
public class BuffManager : MonoBehaviour
{
    public List<Buff> MyBuffs = new List<Buff>();

    // PlayerManager.playerInfo의 추가 스탯을 변화시켜줍니다.
    PlayerManager playerManager;
    
    
    // PlayerManager에서 버프를 추가합니다.
    public void AddBuff(Buff buff)
    {
        StartCoroutine(BuffCoroutine(buff));
    }

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    // 버프 적용
    IEnumerator BuffCoroutine(Buff buff)
    {
        MyBuffs.Add(buff);
        GameUiManager.instance.Notice(buff.buffName, false);
        GameUiManager.instance.BuffImg(buff, buff.duration);
        
        // 모두 적용하는 것은 별로일 것 같아서 bool 변수로 한번 확인한 뒤, 분류 별로 적용
        if (buff.changeAttackPower)
        {
            playerManager.playerInfo.attackPowerAdd += buff.bonusStatus.attackPowerAdd;
            playerManager.playerInfo.attackPowerMul *= buff.bonusStatus.attackPowerMul;
        }
        if (buff.changeAttackSpeed)
        {
            playerManager.playerInfo.attackSpeedMul *= buff.bonusStatus.attackSpeedMul;
            playerManager.anim.SetFloat("ATTACK_SPEED", playerManager.playerInfo.attackSpeed);
        }
        if (buff.changeCriticalHit)
        {
            playerManager.playerInfo.criticalHitAdd += buff.bonusStatus.criticalHitAdd;
            playerManager.playerInfo.criticalHitMul *= buff.bonusStatus.criticalHitMul;
        }
        if (buff.changeMoveSpeed)
        {
            playerManager.playerInfo.moveSpeedAdd += buff.bonusStatus.moveSpeedAdd;
            playerManager.playerInfo.moveSpeedMul *= buff.bonusStatus.moveSpeedMul;
            // moveSpeedStatic으로 나눠주는 이유는 원래 속도에 비례해서 빨라져야 하기 때문임.
            playerManager.anim.SetFloat("MOVE_SPEED", playerManager.playerInfo.moveSpeed / playerManager.playerInfo.moveSpeedStatic);
        }
        
        yield return new WaitForSeconds(buff.duration); // 일정 시간 후, 버프 삭제

        // 반대로 계산해서 삭제 시킴
        if (buff.changeAttackPower)
        {
            playerManager.playerInfo.attackPowerAdd -= buff.bonusStatus.attackPowerAdd;
            playerManager.playerInfo.attackPowerMul /= buff.bonusStatus.attackPowerMul;
        }
        if (buff.changeAttackSpeed)
        {
            playerManager.playerInfo.attackSpeedMul /= buff.bonusStatus.attackSpeedMul;
            playerManager.anim.SetFloat("ATTACK_SPEED", playerManager.playerInfo.attackSpeed);
        }
        if (buff.changeCriticalHit)
        {
            playerManager.playerInfo.criticalHitAdd -= buff.bonusStatus.criticalHitAdd;
            playerManager.playerInfo.criticalHitMul /= buff.bonusStatus.criticalHitMul;
        }
        if (buff.changeMoveSpeed)
        {
            playerManager.playerInfo.moveSpeedAdd -= buff.bonusStatus.moveSpeedAdd;
            playerManager.playerInfo.moveSpeedMul /= buff.bonusStatus.moveSpeedMul;
            // moveSpeedStatic으로 나눠주는 이유는 원래 속도에 비례해서 빨라져야 하기 때문임.
            playerManager.anim.SetFloat("MOVE_SPEED", playerManager.playerInfo.moveSpeed / playerManager.playerInfo.moveSpeedStatic);
        }

        MyBuffs.Remove(buff);
    }

}
