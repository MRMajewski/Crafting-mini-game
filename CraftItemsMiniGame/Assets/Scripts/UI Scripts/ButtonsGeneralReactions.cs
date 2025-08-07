using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static ButtonsGeneralReactionsManager;


public class ButtonsGeneralReactions : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected ButtonAnimationType animationType;

    public void OnSelect(BaseEventData eventData)
    {
        ButtonsGeneralReactionsManager.Instance.OnButtonSelected(animationType, this.transform);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ButtonsGeneralReactionsManager.Instance.OnButtonDeselected(animationType, this.transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonsGeneralReactionsManager.Instance.OnButtonSelected(animationType, this.transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonsGeneralReactionsManager.Instance.OnButtonDeselected(animationType, this.transform);
    }

}
