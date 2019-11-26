using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.PostProcessing;

public class CameraCtrl : MonoBehaviour
{
    public Transform target;
    public float targetY = 3;

    private float xRotMin = -35;
    private float xRotMax = 15;
    private float rotSpeed = 250;
    private float scrollSpeed = 100;

    private float distance = 6;
    private float minDistance = 3;
    private float maxDistance = 6;

    private float xRot;
    private float yRot;
    private Vector3 targetPos;
    private Vector3 dir;

    private bool isStop = false;

    private Camera cam;

    [HideInInspector] public float ORIGIN_FieldOfView;
    [HideInInspector] public float MAX_FieldOfView;
    [HideInInspector] public float Curr_FieldOfView;
    [HideInInspector] public float Dest_FieldOfView;
    
    [HideInInspector] public float Curr_Intensity;

    private PostProcessingProfile postProcessing;
    
    private void Start()
    {
        isStop = false;

        postProcessing = GetComponent<PostProcessingBehaviour>().profile;

        cam = Camera.main;
        ORIGIN_FieldOfView = cam.fieldOfView;
        MAX_FieldOfView = 80f;
        Curr_FieldOfView = cam.fieldOfView;
        Dest_FieldOfView = Curr_FieldOfView;

        Curr_Intensity = 0;

        VignetteModel.Settings vigenttedSetting = postProcessing.vignette.settings;
        vigenttedSetting.intensity = Curr_Intensity;
        postProcessing.vignette.settings = vigenttedSetting;
    }

    public void PlayerDamaged(float intensity)
    {
        // 0.5가 최대라고 생각하면 편함
        Curr_Intensity = intensity;
    }

    private void Update()
    {
        if (Curr_Intensity > 0)
        {
            if (Curr_Intensity < 0.1f) // 거의 끝나간다 싶으면
            {
                Curr_Intensity = 0; // 그냥 끝냄    
                return;
            }

            Curr_Intensity = Mathf.Lerp(Curr_Intensity, 0, 0.01f);

            VignetteModel.Settings vigenttedSetting = postProcessing.vignette.settings;
            vigenttedSetting.intensity = Curr_Intensity;
            postProcessing.vignette.settings = vigenttedSetting;
        }
    }

    private void LateUpdate()
    {
        if (Curr_FieldOfView != Dest_FieldOfView)
        {
            Curr_FieldOfView = Mathf.Lerp(Curr_FieldOfView, Dest_FieldOfView, 0.2f);
            cam.fieldOfView = Curr_FieldOfView;
        }

        if (isStop)
        {
            return;
        }

        xRot += Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
        yRot += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        distance += -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

        xRot = Mathf.Clamp(xRot, xRotMin, xRotMax);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        targetPos = target.position + Vector3.up * targetY;

        dir = Quaternion.Euler(-xRot, yRot, 0f) * Vector3.forward;
        transform.position = targetPos + dir * -distance;
        transform.LookAt(targetPos);
    }

    //// 플레이어 뒤로 카메라 위치 초기화
    //public void CameraPosBehindPlayerPos()
    //{
    //    Vector3 pos = target.position + target.up * targetY;
    //    transform.position = pos + (target.forward * -distance);
    //    transform.LookAt(pos);
    //}

    public void StopCamera()
    {
        isStop = true;
    }

    public void StartCamera()
    {
        isStop = false;
    }
}
