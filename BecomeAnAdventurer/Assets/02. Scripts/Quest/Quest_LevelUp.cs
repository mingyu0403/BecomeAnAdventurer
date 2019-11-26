using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_LevelUp : Quest
{
    [Header(" [ Quest_LevelUp ]")]
    public int targetLevel;

    private void Start()
    {
        base.description = CustomFormat(base.description);
        base.shortDescription = CustomFormat(base.shortDescription);

        // ex) 5명 처치라면, 0 / 5 로 초기화
        Set_StrQuestProgress();
    }

    private void Set_StrQuestProgress()
    {
        base.progress = GameManager.instance.playerManager.playerInfo.level + " / " + targetLevel;
    }

    // 편의를 위해서 퀘스트 설명문에 변수이름을 적음, 그러므로 변수이름을 변수값으로 모두 변경 
    private string CustomFormat(string str)
    {
        str = str.Replace("${PlayerName}", GameManager.instance.playerManager.playerInfo.nickname);
        str = str.Replace("${TargetLevel}", this.targetLevel.ToString());
        str = str.Replace("${Reward}", base.reward.ToString());

        return str;
    }

    public override void AddQuest()
    {
        base.questProgressType = Quest.QuestProgressType.PROGRESS;
        
        SetQuestEvent();
    }

    public override void SetQuestEvent()
    {
        GameManager.instance.playerManager.LevelUpEvent += QuestProgress;
        GameManager.instance.playerManager.AddQuest(this);
        
        // 레벨을 넘었을 수도 있으니까 한번 판정함.
        Set_StrQuestProgress();
        GameUiManager.instance.questListUiController.UpdateQuestUI(this);

        if (GameManager.instance.playerManager.playerInfo.level >= targetLevel)
        {
            base.questProgressType = Quest.QuestProgressType.SUCCESS;
            GameUiManager.instance.Notice("퀘스트 \'" + base.shortDescription + "\' 달성", false);
        }
    }

    public override void RemoveQuestEvent()
    {
        GameManager.instance.playerManager.LevelUpEvent -= QuestProgress;
        GameManager.instance.playerManager.RemoveQuest(this);
    }

    // 퀘스트 진행
    public void QuestProgress(PlayerInfo playerInfo)
    {
        // 퀘스트 진행 중일 때만
        if (base.questProgressType != Quest.QuestProgressType.PROGRESS)
        {
            return;
        }
        Set_StrQuestProgress();

        int curr_Level = playerInfo.level;
        if (curr_Level >= targetLevel)
        {
            base.questProgressType = Quest.QuestProgressType.SUCCESS;
            GameUiManager.instance.Notice("퀘스트 \'" + base.shortDescription + "\' 달성", false);
        }
        GameUiManager.instance.questListUiController.UpdateQuestUI(this);
    }

    public override void QuestComplete()
    {
        base.questProgressType = Quest.QuestProgressType.COMPLETE;
        
        Debug.Log("퀘스트 성공, 경험치 " + base.reward + "획득!!");
        GameManager.instance.playerManager.CompleteQuestEvent(this);

        RemoveQuestEvent();
    }


    public override Quest DeepCopy()
    {
        Quest_LevelUp newCopyQuest = GameManager.instance.playerManager.QuestComponentSave.AddComponent<Quest_LevelUp>();

        // 퀘스트 카피
        newCopyQuest.QuestProgressTypeChangeEvent = base.QuestProgressTypeChangeEvent;
        newCopyQuest.number = base.number;
        newCopyQuest.questProgressType = base.questProgressType;
        newCopyQuest.description = base.description;
        newCopyQuest.shortDescription = base.shortDescription;
        newCopyQuest.npcName = base.npcName;
        newCopyQuest.progress = base.progress;
        newCopyQuest.reward = base.reward;

        // 몬스터 킬 카피
        newCopyQuest.targetLevel = this.targetLevel;

        return newCopyQuest;
    }
}
