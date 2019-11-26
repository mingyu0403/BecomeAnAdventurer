using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    public Rigidbody Skill1_ShootedSphere;
    public GameObject cursor;
    public Transform shootPoint;
    public LayerMask layer;
    public LineRenderer lineVisual;
    public int lineSegment = 10;

    public GameObject Ui;

    public Action SkillRealActive; // 스킬을 발동할 때 이벤트 발생



    private Camera cam;
    private CameraCtrl cameraCtrl;

    private bool isShow = false;
    
    void Start()
    {
        cam = Camera.main;
        cameraCtrl = cam.GetComponent<CameraCtrl>();

        lineVisual.positionCount = lineSegment;

        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShow)
        {
            LaunchProjectile();

            if (Input.GetMouseButtonDown(1))
            {
                Hide();
            }
        }
    }

    public void Hide()
    {
        cursor.SetActive(false);
        lineVisual.enabled = false;
        cameraCtrl.StartCamera();
        Ui.SetActive(false);
        cameraCtrl.Dest_FieldOfView = cameraCtrl.ORIGIN_FieldOfView;
        GameManager.instance.HideMouseCursor();

        SkillRealActive = null;
        isShow = false;
    }

    public void Show()
    {
        cursor.SetActive(true);
        lineVisual.enabled = true;
        cam.GetComponent<CameraCtrl>().StopCamera();
        Ui.SetActive(true);
        cameraCtrl.Dest_FieldOfView = cameraCtrl.MAX_FieldOfView;
        GameManager.instance.ShowMouseCursor();

        LaunchProjectile();
        isShow = true;
    }

    void LaunchProjectile()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, 100f, layer))
        {
            cursor.SetActive(true);
            cursor.transform.position = hit.point;

            Vector3 vo = CalculateVelocity(hit.point, shootPoint.position, 1f);

            Visualize(vo);

            transform.rotation = Quaternion.LookRotation(vo);

            if (Input.GetMouseButtonDown(0))
            {
                Rigidbody obj = Instantiate(Skill1_ShootedSphere, shootPoint.position, Quaternion.identity);
                obj.velocity = vo;
                SkillRealActive();
                Hide();
            }
        }
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }


    void Visualize(Vector3 vo)
    {
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 pos = CalculatePosInTime(vo, i / (float)lineSegment);
            lineVisual.SetPosition(i, pos);
        }
    }
    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;

        Vector3 result = shootPoint.position + vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + shootPoint.position.y;

        result.y = sY;

        return result;
    }
}
