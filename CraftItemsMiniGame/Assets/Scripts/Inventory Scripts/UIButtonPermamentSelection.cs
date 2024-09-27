using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIButtonPermamentSelection : MonoBehaviour
{
    public enum Mode
    {
        COLOR,
        IMAGE,
        SCALE
    }

    [SerializeField]
    protected List<Button> changedButtons = new List<Button>();

    protected List<Image> changedImages = new List<Image>();

    [SerializeField]
    protected Mode mode = Mode.COLOR;

    [Header("Color mode")]
    [SerializeField]
    protected Color selectedColor;

    [SerializeField]
    protected Color normalColor;

    [Header("Image mode")]
    [SerializeField]
    protected Sprite selectedSprite;

    [SerializeField]
    protected Sprite normalSprite;


    [Header("Image mode")]
    [SerializeField]
    protected Vector2 normalScale;

    [SerializeField]
    protected Vector2 selectedScale;

    public void ChangeToSelected(Button button)
    {
        DeselectAll();

        if (mode == Mode.COLOR)
        {
            ChangeToSelectedColor(button);

        }
        else if (mode == Mode.IMAGE)
        {
            ChangeToSelectedImage(button);
        }
        else if (mode == Mode.SCALE)
        {
            ChangeToSelectedScale(button);
        }
    }
    #region Select Methods
    public void ChangeToSelectedColor(Button button)
    {
        var color = button.colors;
        color.normalColor = selectedColor;
        button.colors = color;
        changedButtons.Add(button);
    }
    public void ChangeToSelectedImage(Button button)
    {
        var tempColor = button.image.color;
        button.image.color = tempColor;
        button.image.sprite = selectedSprite;
        changedButtons.Add(button);
    }
    public void ChangeToSelectedScale(Button button)
    {
        button.transform.localScale = selectedScale;
        //button.image.color = tempColor;
        //button.image.sprite = selectedSprite;
        changedButtons.Add(button);
    }

    #endregion Select Methods
    #region Deselect Methods
    public void DeselectAll()
    {
        if (mode == Mode.COLOR)
        {
            DeselectColor();
        }
        else if (mode == Mode.IMAGE)
        {
            DeselectImage();
        }
        else if (mode == Mode.SCALE)
        {
            DeselectScale();
        }
        changedButtons.Clear();
    }

    public void DeselectColor()
    {
        foreach (var item in changedButtons)
        {
            var color = item.colors;
            color.normalColor = normalColor;
            item.colors = color;
        }
    }
    public void DeselectImage()
    {
        foreach (var item in changedButtons)
        {
            var tempColor = item.image.color;
            item.image.color = tempColor;
            item.image.sprite = normalSprite;
        }
    }

    public void DeselectScale()
    {
        foreach (var item in changedButtons)
        {
            item.transform.localScale = normalScale;
        }
    }

    #endregion Deselect Methods
    public Selectable GetSelectedButton()
    {
        return changedButtons[0];
    }
    public Button GetSelectedButtonObject()
    {
        if (changedButtons.Count > 0)
            return changedButtons[0];
        else
            return null;
    }

    public void ClearChangedButtonsList()
    {
        changedButtons.Clear();
    }
}

