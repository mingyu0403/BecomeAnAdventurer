using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  알림 메세지 띄워주기
/// </summary>
public class NoticeUiController : MonoBehaviour
{
    List<GameObject> noticePanelList;
    List<GameObject> specialNoticePanelList;

    private int amountToPool = 0;

    // root 오브젝트
    public Transform Panel_Notices;
    public GameObject noticePanel;
    public GameObject specialNoticePanel;

    // 생성
    private void Start()
    {
        noticePanelList = new List<GameObject>();
        specialNoticePanelList = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            CreateObject(true);
            CreateObject(false);
        }
    }

    private void CreateObject(bool isSpecial)
    {
        // 특별한 알림
        if (isSpecial)
        {
            GameObject specialNotice = (GameObject)Instantiate(specialNoticePanel, Panel_Notices);
            specialNotice.transform.SetAsFirstSibling();
            specialNotice.SetActive(false);
            specialNotice.AddComponent<InvisibleObject>().delayTime = 2.1f;
            specialNoticePanelList.Add(specialNotice);
        }
        else // 기본 알림
        {
            GameObject notice = (GameObject)Instantiate(noticePanel, Panel_Notices);
            notice.transform.SetAsFirstSibling();
            notice.SetActive(false);
            notice.AddComponent<InvisibleObject>().delayTime = 2.1f;
            noticePanelList.Add(notice);
        }
    }

    // 메세지를 바꾸고 뛰워줍니다.
    public void StartAnim(string msg, bool isSpecial)
    {
        if (isSpecial)
        {
            for (int i = 0; i < specialNoticePanelList.Count; i++)
            {
                if (!specialNoticePanelList[i].activeInHierarchy)
                {
                    specialNoticePanelList[i].GetComponentInChildren<Text>().text = msg;
                    specialNoticePanelList[i].transform.SetAsFirstSibling();
                    specialNoticePanelList[i].SetActive(true);
                    return;
                }
            }
            // 부족하면 하나 더 생성한 뒤, 그것을 띄워줌.
            CreateObject(true);

            specialNoticePanelList[specialNoticePanelList.Count - 1].GetComponentInChildren<Text>().text = msg;
            specialNoticePanelList[specialNoticePanelList.Count - 1].SetActive(true);
        }
        else
        {
            for (int i = 0; i < noticePanelList.Count; i++)
            {
                if (!noticePanelList[i].activeInHierarchy)
                {
                    noticePanelList[i].GetComponentInChildren<Text>().text = msg;
                    noticePanelList[i].transform.SetAsFirstSibling();
                    noticePanelList[i].SetActive(true);
                    return;
                }
            }
            // 부족하면 하나 더 생성한 뒤, 그것을 띄워줌.
            CreateObject(false);

            noticePanelList[noticePanelList.Count - 1].GetComponentInChildren<Text>().text = msg;
            noticePanelList[noticePanelList.Count - 1].SetActive(true);
        }
    }
}
