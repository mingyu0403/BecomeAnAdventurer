using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject SpawnObject;

    public Transform[] RespawnPositionArr;

    public int obj_MaxCnt = 5;
    public int obj_CurrCnt = 0;

    // 생성
    private void Start()
    {
        InvokeRepeating("CreateContinually", 2f, 7f);
    }

    private void CreateContinually()
    {
        if (obj_CurrCnt >= obj_MaxCnt) // 현재 개수가 Max 개수보다 많다면 그만 생성함.
        {
            return;
        }

        int index = Random.Range(0, RespawnPositionArr.Length);
        Vector3 positionOffset = Vector3.right * Random.Range(-3f, 3f) + Vector3.forward * Random.Range(-3f, 3f);
        CreateObject(RespawnPositionArr[index].position + positionOffset);
        
    }

    private void CreateObject(Vector3 position)
    {
        obj_CurrCnt++;

        GameObject obj = (GameObject)Instantiate(SpawnObject, this.transform);
        obj.transform.position = position;
        obj.GetComponent<BuffCreator>().BuffDestroyEvent += ObjDestroy;
        obj.SetActive(true);
    }

    public void ObjDestroy()
    {
        obj_CurrCnt--;
    }
}
