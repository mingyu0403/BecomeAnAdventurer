using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDescriptionController : MonoBehaviour
{
    public Text Text_NPCName;
    public Text Text_Description;

    public Button Button_Accept;
    public Button Button_Cancel;
    

    public void ChangeBasicSpeechBubble()
    {
        Button_Accept.gameObject.SetActive(false);
        Button_Cancel.GetComponentInChildren<Text>().text = "확인";
    }

    public void ChangeQuestSpeechBubble()
    {
        Button_Accept.gameObject.SetActive(true);
        Button_Cancel.GetComponentInChildren<Text>().text = "거절";
    }
}
