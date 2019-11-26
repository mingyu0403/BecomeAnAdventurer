using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NPCCtrl : MonoBehaviour
{
    public string npcName;
    public GameObject UI_TalkWithNPC;
    public GameObject UI_QuestDescription;

    public TextMeshProUGUI Text_OnHead;

    public bool isTriggerByPlayer = false;

    [Header("${변수이름} = 변수 값으로 변경됩니다. (PlayerName, NpcName) 가능")]
    [TextArea(3,5)]
    public string whenPlayerQuestProgress_Talk;
    [TextArea(3,5)]
    public string whenPlayerQuestSuccess_Talk;
    [TextArea(3,5)]
    public string whenPlayerQuestComplete_Talk;

    public enum NPCProgressType
    {
        BEFORE, // 퀘스트 받기 전
        PROGRESS, // 퀘스트 진행 중
        SUCCESS, // 퀘스트 성공
        COMPLETE  // 퀘스트 완료
    }
    private NPCProgressType _nPCProgressType = NPCProgressType.BEFORE;
    public NPCProgressType nPCProgressType
    {
        set
        {
            _nPCProgressType = value;
            if (NPCProgressTypeChangeEvent != null)
            {
                NPCProgressTypeChangeEvent(_nPCProgressType);
            }
        }
        get
        {
            return _nPCProgressType;
        }
    }
    public Action<NPCProgressType> NPCProgressTypeChangeEvent;

    // 가지고 있는 퀘스트
    Quest[] questArr;
    int questArrIndex = 0; // 현재 진행 중인 퀘스트

    Quest refQuestInPlayer; // 플레이어가 갖고 있는 퀘스트
    
    // 씬이 바뀌었을 때 초기화
    public void SetQuest_WhenChangeScene(Quest refQuestInPlayer)
    {
        if (refQuestInPlayer.enabled == false) // 이미 해결한 퀘스트
        {
            questArrIndex++;
            if (!HaveQuest())
            {
                Text_OnHead.gameObject.SetActive(false);
            }
            return;
        }
        //for (int i=0; i<questArr.Length; i++)
        //{
        //    // Debug.Log("퀘스트리스트 넘버 : " + questArr[i].number + ", 참조퀘스트 넘버 : " + refQuestInPlayer.number);
        //    if (questArr[i].number <= refQuestInPlayer.number)
        //    {
        //        questArrIndex = i;
        //    }
        //}
        // Debug.Log("현재 퀘스트 넘버 : " + questArrIndex);

        this.refQuestInPlayer = refQuestInPlayer;
        this.refQuestInPlayer.QuestProgressTypeChangeEvent += ChangeQuestProgressType;
        ChangeQuestProgressType(refQuestInPlayer.questProgressType);
    }

    // 플레이어가 수락했을 때 NPC에게 이벤트 달기
    public void SetQuest_WhenPlayerAddQuest(Quest refQuestInPlayer)
    {
        this.refQuestInPlayer = refQuestInPlayer;
        this.refQuestInPlayer.QuestProgressTypeChangeEvent += ChangeQuestProgressType;
        ChangeQuestProgressType(refQuestInPlayer.questProgressType);
    }

    private void OnEnable()
    {
        NPCProgressTypeChangeEvent += ChangeNPCProgressType;
    }
    private void OnDisable()
    {
        NPCProgressTypeChangeEvent -= ChangeNPCProgressType;
    }

    private void Awake()
    {
        // 모든 퀘스트 가져오기 (컴포넌트로 추가되어있음)
        questArr = GetComponents<Quest>();
        for (int i=0; i<questArr.Length; i++)
        {
            questArr[i].npcName = this.npcName;
        }
    }

    void Start()
    {
        UI_QuestDescription.SetActive(false);
        
        UI_QuestDescription.GetComponent<QuestDescriptionController>().Text_NPCName.text = this.npcName;

        whenPlayerQuestProgress_Talk = CustomFormat(whenPlayerQuestProgress_Talk);
        whenPlayerQuestSuccess_Talk = CustomFormat(whenPlayerQuestSuccess_Talk);
        whenPlayerQuestComplete_Talk = CustomFormat(whenPlayerQuestComplete_Talk);
    }

    // 편의를 위해서 퀘스트 설명문에 변수이름을 적음, 그러므로 변수이름을 변수값으로 모두 변경 
    private string CustomFormat(string str)
    {
        str = str.Replace("${PlayerName}", GameManager.instance.playerManager.playerInfo.nickname);
        str = str.Replace("${NpcName}", this.npcName);

        return str;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            UI_TalkWithNPC.SetActive(true);
            isTriggerByPlayer = true;
        }
    }

    private void Update()
    {
        Debug.Log("questArrIndex : " + questArrIndex);

        if (!isTriggerByPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            UI_TalkWithNPC.SetActive(false);

            switch (nPCProgressType)
            {
                case NPCProgressType.BEFORE:
                    if (HaveQuest())
                    {
                        UI_QuestDescription.GetComponent<QuestDescriptionController>().Text_Description.text = questArr[questArrIndex].description;
                        UI_QuestDescription.GetComponent<QuestDescriptionController>().ChangeQuestSpeechBubble();
                        GetComponent<Animator>().SetTrigger("HELLO");
                    } else
                    {
                        UI_QuestDescription.GetComponent<QuestDescriptionController>().Text_Description.text = this.whenPlayerQuestComplete_Talk;
                        UI_QuestDescription.GetComponent<QuestDescriptionController>().ChangeBasicSpeechBubble();
                    }
                    break;
                case NPCProgressType.PROGRESS:
                    UI_QuestDescription.GetComponent<QuestDescriptionController>().Text_Description.text = this.whenPlayerQuestProgress_Talk;
                    UI_QuestDescription.GetComponent<QuestDescriptionController>().ChangeBasicSpeechBubble();
                    break;
                case NPCProgressType.SUCCESS:
                    UI_QuestDescription.GetComponent<QuestDescriptionController>().Text_Description.text = 
                        this.whenPlayerQuestSuccess_Talk
                        + "\n\n보상 : 경험치 " + questArr[questArrIndex].reward + "exp";
                    UI_QuestDescription.GetComponent<QuestDescriptionController>().ChangeBasicSpeechBubble();
                    refQuestInPlayer.QuestComplete();
                    GetComponent<Animator>().SetTrigger("COMPLETE");
                    break;
            }
            ShowQuestDescriptionUI();
        }
    }

    private bool HaveQuest()
    {
        int questCnt = questArr.Length - 1;
        Debug.Log("현재 퀘스트 인덱스 : " + questArrIndex + ", 퀘스트 갯수 : " + questCnt);
        if (questArrIndex <= questCnt) // 퀘스트가 남았으면
        {
            return true;
        }
        return false;
    }
    

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            RotateTowards(other.gameObject.transform.position);
        }
    }


    public void ChangeQuestProgressType(Quest.QuestProgressType questProgressType)
    {
        // Debug.Log("퀘스트 진행도 변함 : " + questProgressType);

        switch (questProgressType)
        {
            case Quest.QuestProgressType.BEFORE:
                nPCProgressType = NPCProgressType.BEFORE;
                break;
            case Quest.QuestProgressType.PROGRESS:
                nPCProgressType = NPCProgressType.PROGRESS;
                break;
            case Quest.QuestProgressType.SUCCESS:
                nPCProgressType = NPCProgressType.SUCCESS;
                break;
            case Quest.QuestProgressType.COMPLETE:
                nPCProgressType = NPCProgressType.COMPLETE;
                break;
        }
    }

    public void ChangeNPCProgressType(NPCProgressType type)
    {
        // Debug.Log(npcName + " NPC 진행도 변함 : " + type);

        switch (type)
        {
            case NPCProgressType.BEFORE:
                if (HaveQuest())
                {
                    Text_OnHead.text = "?";
                    Text_OnHead.gameObject.SetActive(true);
                } else
                {
                    Text_OnHead.gameObject.SetActive(false);
                }
                break;
            case NPCProgressType.PROGRESS:
                Text_OnHead.text = "...";
                Text_OnHead.gameObject.SetActive(true);
                break;
            case NPCProgressType.SUCCESS:
                Text_OnHead.text = "!";
                Text_OnHead.gameObject.SetActive(true);
                break;
            case NPCProgressType.COMPLETE:
                questArrIndex++;
                this.nPCProgressType = NPCProgressType.BEFORE;
                break;
        }
    }

    private void ShowQuestDescriptionUI()
    {
        // 환경 세팅 
        GameUiManager.instance.ClearUI(); // UI 없애기
        Camera.main.GetComponent<CameraCtrl>().StopCamera(); // 카메라 멈추기
        GameManager.instance.ShowMouseCursor(); // 커서 보이게 하기
        
        UI_QuestDescription.SetActive(true);
    }

    // 수락 버튼 이벤트
    public void ButtonEvent_QuestAccept()
    {
        // 환경 세팅 
        GameUiManager.instance.ShowUI(); // UI 보이게 하기
        Camera.main.GetComponent<CameraCtrl>().StartCamera(); // 카메라 움직이기
        GameManager.instance.HideMouseCursor(); // 커서 안 보이게 하기
        
        Quest newCopyQuest = questArr[questArrIndex].DeepCopy();
        newCopyQuest.AddQuest();

        UI_QuestDescription.SetActive(false);

        if (isTriggerByPlayer)
        {
            UI_TalkWithNPC.SetActive(true);
        }
    }
    // 거절 버튼 이벤트
    public void ButtonEvent_QuestCancel()
    {
        // 환경 세팅 
        GameUiManager.instance.ShowUI(); // UI 보이게 하기
        Camera.main.GetComponent<CameraCtrl>().StartCamera(); // 카메라 움직이기
        GameManager.instance.HideMouseCursor(); // 커서 안 보이게 하기
        
        UI_QuestDescription.SetActive(false);

        if (isTriggerByPlayer)
        {
            UI_TalkWithNPC.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            UI_TalkWithNPC.SetActive(false);
            isTriggerByPlayer = false;
        }
    }

    private void RotateTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.2f);
    }
}
