using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpBarScript : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 scale = Vector3.one;

    private Slider hpSlider;
    private float currHpValue;

    private TextMeshProUGUI textHp;
    private TextMeshProUGUI textNick;

    GameObject ui;

    private float showTime = 5f;
    private float currShowTime = 0f;
    private bool isShow = false;


    public GameObject hpBarSliderCanvas; 

    private void Awake()
    {
        if (hpBarSliderCanvas == null)
        {
            hpBarSliderCanvas = Resources.Load("Canvas_Info") as GameObject;

            ui = Instantiate(hpBarSliderCanvas
                , this.transform.position + offset
                , Quaternion.identity
                , this.transform);

            ui.transform.localScale = scale;
        }
        else
        {
            ui = hpBarSliderCanvas;
        }


        hpSlider = ui.GetComponent<CanvasInfoController>().hpSilder;
        textHp = ui.GetComponent<CanvasInfoController>().textHp;
        textNick = ui.GetComponent<CanvasInfoController>().textNick;
    }

    private void Update()
    {
        if (isShow)
        {
            if (currHpValue != hpSlider.value)
            {
                hpSlider.value = Mathf.Lerp(hpSlider.value, currHpValue, 0.2f);
            }
            currShowTime += Time.smoothDeltaTime;
            if (currShowTime > showTime)
            {
                HideCanvas();
            }
        }
    }


    public Slider GetHpSlider()
    {
        return hpSlider;
    }

    // 초기화
    public void Init(float maxValue, string nick)
    {
        hpSlider.maxValue = maxValue;
        hpSlider.value = maxValue;
        currHpValue = hpSlider.value;

        textNick.text = nick;
        textHp.text = currHpValue + " / " + hpSlider.maxValue;
    }

    public void SetHpValue(float hp)
    {
        currShowTime = 0; // 보여주는 시간 초기화

        if (!isShow)
        {
            ShowCanvas(); // 보여주기
        }

        currHpValue = hp;
        textHp.text = currHpValue + " / " + hpSlider.maxValue;
    }

    public float GetHpValue()
    {
        return currHpValue;
    }

    public void HideCanvas()
    {
        isShow = false;
        ui.SetActive(false);
    }

    public void ShowCanvas()
    {
        isShow = true;
        ui.SetActive(true);
    }

}
