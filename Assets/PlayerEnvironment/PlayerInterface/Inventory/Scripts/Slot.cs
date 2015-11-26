using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerClickHandler {

    private Stack <Item> items;

    public Stack<Item> Items
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    public Text amountText;
    public Sprite slotEmpty, slotHighlight;

    public bool isEmpty
    {
        get { return Items.Count == 0; }
    }

    public Item currentItem
    {
        get { return Items.Peek(); }
    }

    public bool spaceAvailable
    {
        get { return currentItem.maxSize > Items.Count; }
    }

    // Use this for initialization
    void Start () {
        Items = new Stack<Item>();
        RectTransform slotTransform = GetComponent<RectTransform>();
        RectTransform textTransform = amountText.GetComponent<RectTransform>();

        int textScale = (int)(slotTransform.sizeDelta.x * 0.60);
        amountText.resizeTextMaxSize = textScale;
        amountText.resizeTextMinSize = textScale;

        textTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotTransform.sizeDelta.x);
        textTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotTransform.sizeDelta.y);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addItem(Item item)
    {
        Items.Push(item);

        if(Items.Count > 1)
        {
            amountText.text = Items.Count.ToString();
        }

        changeSpite(item.spriteNeutral, item.spriteHighlighted);

    }

    public void addItems(Stack<Item> items)
    {
        this.Items = new Stack<Item>(items);
        stackText();
        changeSpite(currentItem.spriteNeutral, currentItem.spriteHighlighted);
    }

    private void changeSpite(Sprite neutral, Sprite highlighted)
    {
        GetComponent<Image>().sprite = neutral;

        SpriteState spriteState = new SpriteState();
        spriteState.highlightedSprite = highlighted;
        spriteState.pressedSprite = neutral;

        GetComponent<Button>().spriteState = spriteState;
    }

    private void useItem()
    {
        if (!isEmpty)
        {
            Items.Pop().use();
            stackText();

            if (isEmpty)
            {
                changeSpite(slotEmpty, slotHighlight);
                Inventory.EmptySlots++;
            }
        }
    }

    public void clearSlot()
    {
        items.Clear();
        changeSpite(slotEmpty, slotHighlight);
        GetComponent<Image>().color = Color.white;
        amountText.text = null;
    }

    public void stackText()
    {
        if (Items.Count > 1)
        {
            amountText.text = Items.Count.ToString();
        }
        else
        {
            amountText.text = null;
        }
    }

    public Stack<Item> removeItems(int removeAmount)
    {
        Stack<Item> temp = new Stack<Item>();

        for(int i = 0; i < removeAmount; i++)
        {
            temp.Push(items.Pop());
        }

        stackText();
        return temp;
    }

    public Item removeItem()
    {
        Item temp;
        temp = items.Pop();
        stackText();
        return temp;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && Inventory.CanvasGroup.alpha > 0)
        {
            useItem();
        }
        else if(eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && !isEmpty && !GameObject.Find("Hover"))
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Inventory.Instance.canvas.transform as RectTransform, Input.mousePosition, Inventory.Instance.canvas.worldCamera, out position);
            Inventory.Instance.stackSplit.SetActive(true);
            Inventory.Instance.stackSplit.transform.position = Inventory.Instance.canvas.transform.TransformPoint(position);
            Inventory.Instance.setStackInfo(items.Count);
        }
    }
}
