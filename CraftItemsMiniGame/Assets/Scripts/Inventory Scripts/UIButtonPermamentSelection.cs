using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class UIButtonPermamentSelection : MonoBehaviour
{
    public enum Mode
    {
        COLOR,
        IMAGE,
        SCALE
    }

    [SerializeField] protected List<Button> changedButtons = new List<Button>();
    [SerializeField] protected Mode mode = Mode.COLOR;

    [Header("Color mode")]
    [SerializeField] protected Color selectedColor;
    [SerializeField] protected Color normalColor;

    [Header("Image mode")]
    [SerializeField] protected Sprite selectedSprite;
    [SerializeField] protected Sprite normalSprite;

    [Header("Scale mode")]
    [SerializeField] protected Vector3 normalScale = Vector3.one;
    [SerializeField] protected Vector3 selectedScale = Vector3.one * 1.2f;
    [SerializeField] protected float scaleTweenDuration = 0.25f;

    private Dictionary<Button, Tween> scaleTweens = new Dictionary<Button, Tween>();

    #region Select Methods
    public void ChangeToSelected(Button button)
    {
        DeselectAll();

        switch (mode)
        {
            case Mode.COLOR:
                ChangeToSelectedColor(button);
                break;
            case Mode.IMAGE:
                ChangeToSelectedImage(button);
                break;
            case Mode.SCALE:
                ChangeToSelectedScale(button);
                break;
        }

        changedButtons.Add(button);
    }

    private void ChangeToSelectedColor(Button b)
    {
        var colorBlock = b.colors;
        colorBlock.normalColor = selectedColor;
        b.colors = colorBlock;
    }

    private void ChangeToSelectedImage(Button b)
    {
        var tempColor = b.image.color;
        b.image.color = tempColor;
        b.image.sprite = selectedSprite;
    }

    private void ChangeToSelectedScale(Button b)
    {
        if (scaleTweens.ContainsKey(b))
        {
            scaleTweens[b]?.Kill();
        }

        Tween tween = b.transform.DOScale(selectedScale, scaleTweenDuration).SetEase(Ease.OutBack);
        scaleTweens[b] = tween;
    }
    #endregion

    #region Deselect Methods
    public void DeselectAll()
    {
        switch (mode)
        {
            case Mode.COLOR:
                DeselectColor();
                break;
            case Mode.IMAGE:
                DeselectImage();
                break;
            case Mode.SCALE:
                DeselectScale();
                break;
        }

        changedButtons.Clear();
    }

    private void DeselectColor()
    {
        foreach (var button in changedButtons)
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = normalColor;
            button.colors = colorBlock;
        }
    }

    private void DeselectImage()
    {
        foreach (var button in changedButtons)
        {
            var tempColor = button.image.color;
            button.image.color = tempColor;
            button.image.sprite = normalSprite;
        }
    }

    private void DeselectScale()
    {
        foreach (var button in changedButtons)
        {
            if (scaleTweens.ContainsKey(button))
            {
                scaleTweens[button]?.Kill();
            }

            Tween tween = button.transform.DOScale(normalScale, scaleTweenDuration).SetEase(Ease.InOutSine);
            scaleTweens[button] = tween;
        }
    }
    #endregion

    public Selectable GetSelectedButton()
    {
        return changedButtons.FirstOrDefault<Selectable>();
    }

    public Button GetSelectedButtonObject()
    {
        return changedButtons.Count > 0 ? changedButtons.First() : null;
    }

    public void ClearChangedButtonsList()
    {
        changedButtons.Clear();
    }
}
