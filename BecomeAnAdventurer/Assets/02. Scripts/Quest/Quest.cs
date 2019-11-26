using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Quest : MonoBehaviour
{
    public Action<QuestProgressType> QuestProgressTypeChangeEvent;

    [Header(" [ Quest ]")]

    public int number;

    public enum QuestProgressType
    {
        BEFORE, // 퀘스트 받기 전
        PROGRESS, // 퀘스트 진행 중
        SUCCESS, // 퀘스트 성공
        COMPLETE  // 퀘스트 완료
    }
    private QuestProgressType _questProgressType = QuestProgressType.BEFORE;
    public QuestProgressType questProgressType
    {
        set
        {
            _questProgressType = value;
            if (QuestProgressTypeChangeEvent != null)
            {
                QuestProgressTypeChangeEvent(_questProgressType);
            }
        }
        get
        {
            return _questProgressType;
        }
    }


    [TextArea(3,5)]
    public string description;
    public string shortDescription;
    public string npcName;
    public string progress = "퀘스트에 따라서 자동으로 바뀝니다. (필수 아님)";

    public int reward;


    /// <summary>
    /// NPC한테 퀘스트 받아서 리스트에 저장할 때, 깊은 복사로 저장해야함 (그냥 객체참조로 하면 씬 전환할 때 Null 됨)
    /// </summary>
    /// <returns></returns>
    public abstract Quest DeepCopy();


    /// <summary>
    /// 플레이어 퀘스트List에 퀘스트 추가하기
    /// </summary>
    public abstract void AddQuest();

    /// <summary>
    /// 이벤트 등록하기
    /// </summary>
    public abstract void SetQuestEvent();

    /// <summary>
    /// 이벤트 제거하기
    /// </summary>
    public abstract void RemoveQuestEvent();

    /// <summary>
    /// 퀘스트 성공 처리
    /// </summary>
    public abstract void QuestComplete();
}
