using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUiController : MonoBehaviour
{
    List<GameObject> buffUiList;
    private int amountToPool = 3;

    // root 오브젝트
    public Transform Panel_Buff;
    public GameObject buffUi;

    // 생성
    private void Start()
    {
        buffUiList = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            CreateObject();
        }
    }

    private void CreateObject()
    {
        GameObject obj = (GameObject)Instantiate(buffUi, Panel_Buff);
        obj.SetActive(false);
        obj.AddComponent<InvisibleObject>().delayTime = 3f;
        buffUiList.Add(obj);
    }

    // buff 이미지를 바꾸고 뛰워줍니다.
    public void StartAnim(Buff buff, float duration)
    {
        for (int i = 0; i < buffUiList.Count; i++)
        {
            if (!buffUiList[i].activeInHierarchy)
            {
                buffUiList[i].GetComponent<BuffMouseEvent>().buff = buff;
                buffUiList[i].GetComponent<Image>().sprite = buff.buffImage;
                buffUiList[i].GetComponent<InvisibleObject>().delayTime = duration; // duration을 정하고 SetActive True하자
                buffUiList[i].GetComponent<UI_Cooldown>().cooldown = duration;
                buffUiList[i].SetActive(true);
                return;
            }
        }
        // 부족하면 하나 더 생성한 뒤, 그것을 띄워줌.
        CreateObject();

        buffUiList[buffUiList.Count - 1].GetComponent<BuffMouseEvent>().buff = buff;
        buffUiList[buffUiList.Count - 1].GetComponent<Image>().sprite = buff.buffImage;
        buffUiList[buffUiList.Count - 1].GetComponent<InvisibleObject>().delayTime = duration; // duration을 정하고 SetActive True하자
        buffUiList[buffUiList.Count - 1].GetComponent<UI_Cooldown>().cooldown = duration;
        buffUiList[buffUiList.Count - 1].SetActive(true);
        return;
    }
}
