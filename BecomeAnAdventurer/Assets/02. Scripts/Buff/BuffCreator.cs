using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuffCreator : MonoBehaviour
{
    private const int PERCENT_ATTACKPOWER = 25;
    private const int PERCENT_ATTACKSPEED = 50;
    private const int PERCENT_MOVESPEED = 75;
    private const int PERCENT_CRITICALHIT = 100;

    public Action BuffDestroyEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            other.gameObject.GetComponent<PlayerManager>().AddBuff(CreateBuff());
            Destroy(this.gameObject);

            if (BuffDestroyEvent != null)
            {
                BuffDestroyEvent();
            }
        }
    }

    public Buff CreateBuff()
    {
        Buff buff = new Buff();

        // 버프 타입
        int RandomBuffType = UnityEngine.Random.Range(1, 101); // 1 ~ 100

        if (RandomBuffType <= PERCENT_ATTACKPOWER)
        {
            buff = AttackPowerBuff();
        }
        else if (RandomBuffType <= PERCENT_ATTACKSPEED)
        {
            buff = AttackSpeedBuff();
        }
        else if (RandomBuffType <= PERCENT_MOVESPEED)
        {
            buff = MoveSpeedBuff();
        }
        else // if (RandomBuffType <= PERCENT_CRITICALHIT)
        {
            buff = CriticalHitBuff();
        }

        return buff;
    }

    // 색 바꾸기    
    private string ChanageColorMsg(string msg)
    {
        string newColorMsg = "<color=#ff0000ff>" + msg + "</color>";
        return newColorMsg;
    }
    private string ChanageColorMsg_Green(string msg)
    {
        string newColorMsg = "<color=#00ff7bff>" + msg + "</color>";
        return newColorMsg;
    }
    private string ChanageColorMsg_Green(float msg)
    {
        string newColorMsg = "<color=#ff0000ff>" + msg + "</color>";
        return newColorMsg;
    }
    private string ChanageColorMsg(float msg)
    {
        string newColorMsg = "<color=#ff0000ff>" + msg + "</color>";
        return newColorMsg;
    }

    private Buff AttackPowerBuff()
    {
        Buff buff = new Buff();
        buff.duration = UnityEngine.Random.Range(8, 16); // 8 ~ 15
       
        int RandomUpgradeValue = UnityEngine.Random.Range(3, 11); // 3 ~ 10

        buff.buffName = ChanageColorMsg(buff.duration) + "초 동안 " + ChanageColorMsg_Green("공격력") + " " + (RandomUpgradeValue * 10) + "% 증가";
        buff.buffExplain = ChanageColorMsg(buff.duration) + "초 동안 " + ChanageColorMsg_Green("공격력") + "이 " + (RandomUpgradeValue * 10) + "% 만큼 증가합니다.";

        buff.buffImage = GameUiManager.instance.ICON_AttackPower;

        buff.changeAttackPower = true;
        buff.bonusStatus.attackPowerMul = 1 + RandomUpgradeValue / 10.0f;
        
        return buff;
    }

    private Buff AttackSpeedBuff()
    {
        Buff buff = new Buff();
        buff.duration = UnityEngine.Random.Range(8, 16); // 8 ~ 15

        int RandomUpgradeValue = UnityEngine.Random.Range(3, 7); // 3 ~ 7

        buff.buffName = ChanageColorMsg(buff.duration) + "초 동안 " + ChanageColorMsg_Green("공격 속도") + " " + (RandomUpgradeValue * 10) + "% 증가";
        buff.buffExplain = ChanageColorMsg(buff.duration) + "초 동안 " + ChanageColorMsg_Green("공격 속도") + "가 " + (RandomUpgradeValue * 10) + "% 만큼 증가합니다.";

        buff.buffImage = GameUiManager.instance.ICON_AttackSpeed;

        buff.changeAttackSpeed = true;
        buff.bonusStatus.attackSpeedMul = 1 + RandomUpgradeValue / 10.0f;

        return buff;
    }

    private Buff MoveSpeedBuff()
    {
        Buff buff = new Buff();
        buff.duration = UnityEngine.Random.Range(8, 16); // 8 ~ 15

        int RandomUpgradeValue = UnityEngine.Random.Range(3, 6); // 3 ~ 5

        buff.buffName = ChanageColorMsg(buff.duration) + "초 동안 " + ChanageColorMsg_Green("이동 속도") + " " + (RandomUpgradeValue * 10) + "% 증가";
        buff.buffExplain = ChanageColorMsg(buff.duration) + "초 동안 " + ChanageColorMsg_Green("이동 속도") + "가 " + (RandomUpgradeValue * 10) + "% 만큼 증가합니다.";

        buff.buffImage = GameUiManager.instance.ICON_MoveSpeed;

        buff.changeMoveSpeed = true;
        buff.bonusStatus.moveSpeedMul = 1 + RandomUpgradeValue / 10.0f;

        return buff;
    }

    private Buff CriticalHitBuff()
    {
        Buff buff = new Buff();
        buff.duration = UnityEngine.Random.Range(8, 16); // 8 ~ 15

        int RandomUpgradeValue = UnityEngine.Random.Range(3, 11); // 3 ~ 10

        buff.buffName = ChanageColorMsg(buff.duration) + "초 동안 " + ChanageColorMsg_Green("크리티컬 확률") + " " + (RandomUpgradeValue * 10) + "% 증가";
        buff.buffExplain = ChanageColorMsg(buff.duration) + "초 동안 " + ChanageColorMsg_Green("크리티컬 확률") + "이 " + (RandomUpgradeValue * 10) + "% 만큼 증가합니다.";

        buff.buffImage = GameUiManager.instance.ICON_CriticalHit;

        buff.changeCriticalHit = true;
        buff.bonusStatus.criticalHitMul = 1 + RandomUpgradeValue / 10.0f;

        return buff;
    }
}
