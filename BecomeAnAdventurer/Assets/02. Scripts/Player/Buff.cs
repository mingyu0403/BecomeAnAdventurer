using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public string buffName;
    public string buffExplain;
    public Sprite buffImage;
    public float duration;

    // 어느 부분에 대한 버프인지 확인하는 변수
    public bool changeAttackPower;
    public bool changeAttackSpeed;
    public bool changeMoveSpeed;
    public bool changeCriticalHit;

    public PlayerInfo bonusStatus = new PlayerInfo();
}
