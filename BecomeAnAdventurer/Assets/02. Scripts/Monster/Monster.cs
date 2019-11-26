using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : MonoBehaviour
{
    [Header("[ Monster ]")]
    public string nickname;
    public float hp;
    public float currHp;
    public float traceDist;
    public float attackDist;
    public int attackDamage;
    public Vector3 scaleFactor;
    public bool isDie = false;
    [HideInInspector] public bool isAttack = false;
    [HideInInspector] public bool isAngry = false;
    public float AngryPower;
    public int exp;
    

    [Header("[ 가지고 있어야 하는 정보 ]")]
    public Transform playerTr;
    public NavMeshAgent nvAgent;
    public Animator anim;
}
