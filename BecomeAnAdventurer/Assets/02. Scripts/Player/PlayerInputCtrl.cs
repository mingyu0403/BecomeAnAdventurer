using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputCtrl : MonoBehaviour
{
    private Transform tr;
    private Transform cameraTr;
    private Rigidbody rb;
    private PlayerManager playerManager;
    
    [HideInInspector]
    public float h, v;

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        cameraTr = Camera.main.transform;
        playerManager = GetComponent<PlayerManager>();
    }
    
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        
        ClickEventByUser();
    }

    private void FixedUpdate()
    {
        if (playerManager.isDie)
        {
            return;
        }

        Move();
    }

    void Move()
    {
        Vector3 cameraLocalForward = cameraTr.forward;
        cameraLocalForward.y = 0;

        bool isInputKey = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (!(h == 0 && v == 0) && isInputKey)
        {
            if (playerManager.isAttack)
            {
                playerManager.setAnimBool(PlayerManager.AnimationType.RUN, true);

                return;
            }
            
            float deg = Mathf.Atan2(v, h) - Mathf.PI / 2;
            float x = Mathf.Cos(deg) * cameraLocalForward.x - Mathf.Sin(deg) * cameraLocalForward.z;
            float z = Mathf.Cos(deg) * cameraLocalForward.z + Mathf.Sin(deg) * cameraLocalForward.x;

            Vector3 dir = new Vector3(x, 0, z);

            dir = dir.normalized;
            tr.rotation = Quaternion.Euler(0, Mathf.Atan2(x, z) * Mathf.Rad2Deg, 0);
            tr.position += (dir * playerManager.playerInfo.moveSpeed * Time.deltaTime);

            playerManager.setAnimBool(PlayerManager.AnimationType.RUN, true);
        }
        else
        {
            playerManager.setAnimBool(PlayerManager.AnimationType.RUN, false);
        }
    }

    void ClickEventByUser()
    {
        // 체력 물약
        if (Input.GetKeyDown(playerManager.playerInfo.SHORTKEY_SKILL2)) // 버프
        {
            playerManager.Skill2();
        }
        // 마나 물약
        if (Input.GetKeyDown(playerManager.playerInfo.SHORTKEY_SKILL3)) // 버프
        {
            playerManager.Skill3();
        }

        // 물약은 공격 중에도 사용 가능하다.
        // 공격 중이면 이동 불가, 액티브 스킬 사용 불가.

        if (playerManager.isAttack || playerManager.isJump)
        {
            return;
        }

        /*
        if (Input.GetKeyDown(playerManager.playerInfo.SHORTKEY_JUMP)) // 점프
        {
            playerManager.setAnimTrigger(PlayerManager.AnimationType.JUMP);
            rb.AddForce(Vector3.up * 5.5f, ForceMode.Impulse);
        }
        */

        if (Input.GetKeyDown(playerManager.playerInfo.SHORTKEY_ATTACK)) // 공격
        {
            playerManager.setAnimTrigger(PlayerManager.AnimationType.ATTACK1);
        }

        if (Input.GetKeyDown(playerManager.playerInfo.SHORTKEY_SKILL1)) // 버프
        {
            playerManager.Skill1();
        }

        /*
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50.0f, Color.green);
        */

        /*
        // 마우스 우측버튼 클릭
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                float distanceToTarget = (hit.collider.transform.position - tr.position).sqrMagnitude;

                // 사정거리 안에 있으면
                if (distanceToTarget < Mathf.Pow(playerManager.attackRange, 2))
                {
                    if (hit.collider.CompareTag("ENEMY"))
                    {
                        playerManager.setAnimTrigger(PlayerManager.AnimationType.ATTACK);
                    }
                }
            }
        }
        */
    }
}
