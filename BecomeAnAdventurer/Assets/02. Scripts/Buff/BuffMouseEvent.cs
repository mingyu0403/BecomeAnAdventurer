using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuffMouseEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Buff buff = new Buff();

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameUiManager.instance.ShowTooltip(buff.buffExplain, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameUiManager.instance.HideTooltip();
    }
}
