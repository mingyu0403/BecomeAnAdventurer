using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Cooldown : MonoBehaviour
{
    [HideInInspector] public float cooldown;
    public Image cooldownImg;
    public TextMeshProUGUI cooldownText;

    private float curr_cooldown = 0f;
    public bool isOnEnableStart = false;

    public bool canUse = true;

    private void OnEnable()
    {
        if (isOnEnableStart)
        {
            StartCoolDown();
        }
    }

    private void Awake()
    {
        InitUI_IMG();
        InitUI_TEXT();
    }

    private void InitUI_IMG()
    {
        if (cooldownImg != null)
        {
            cooldownImg.fillAmount = 0;
        }
    }
    private void InitUI_TEXT()
    {
        if (cooldownText != null)
        {
            cooldownText.text = "";
        }
    }

    public void StartCoolDown()
    {
        if (cooldownImg != null && cooldownText != null)
        {
            canUse = false;
            StartCoroutine(CooldownUI(cooldown));
        }
    }

    public void CurrCooldownPlus(float plusValue)
    {
        curr_cooldown += plusValue;
    }

    IEnumerator CooldownUI(float cooldown)
    {
        cooldownText.text = cooldown + "";
        curr_cooldown = 0f;

        while (curr_cooldown < cooldown)
        {
            curr_cooldown += Time.smoothDeltaTime;
            cooldownImg.fillAmount = 1 - (curr_cooldown / cooldown);

            int txt = Mathf.CeilToInt(cooldown - curr_cooldown);
            cooldownText.text = txt.ToString();
            yield return new WaitForFixedUpdate();
        }

        InitUI_IMG();
        InitUI_TEXT();

        canUse = true;

        yield break;
    }
}
