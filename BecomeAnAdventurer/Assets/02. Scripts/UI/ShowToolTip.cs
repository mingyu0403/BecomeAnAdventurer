using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tooltip = "";

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameUiManager.instance.ShowTooltip(tooltip, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameUiManager.instance.HideTooltip();
    }
}
