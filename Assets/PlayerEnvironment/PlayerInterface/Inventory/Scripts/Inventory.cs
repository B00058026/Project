using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

    private RectTransform inventoryTransform;
    private float inventoryWidth, inventoryHeight;
    public int slotNum, rowNum;
    public float slotPadding;
    public float slotSize;
    private static int emptySlots;
    public static int EmptySlots
    {
        get
        {
            return emptySlots;
        }

        set
        {
            emptySlots = value;
        }
    }
    public GameObject slotPrefab;
    private List <GameObject> slots;
    private static Slot from, to;
    public GameObject icon;
    private static GameObject hover;
    public Canvas canvas;
    private float hoverOffset;
    public EventSystem eventSystem;
    private static CanvasGroup canvasGroup;
    public static CanvasGroup CanvasGroup
    {
        get
        {
            return canvasGroup;
        }

        set
        {
            canvasGroup = value;
        }
    }
    private static Inventory instance;
    public static Inventory Instance
    {
        get
        {
            if(instance == null) 
            {
                instance = GameObject.FindObjectOfType<Inventory>();
            }

            return instance;
        }
    }
    private bool fadingIn, fadingOut;
    public float fadeTime;
    private static GameObject slotClicked;
    public GameObject stackSplit;
    private static GameObject stackSplitStatic;
    public Text splitText;
    private int splitAmount;
    private int stackCountMax;
    private static Slot movingSlot;
    public GameObject tooltipObject;
    public static GameObject tooltip;
    public Text tooltipTextObject;
    private static Text tooltipText;
    public Text visualTextObject;
    private static Text visualText;
    private int logCount;
    public int LogCount
    {
        get
        {
            return logCount;
        }
        set
        {
            logCount = value;
        }
    }
    public Text logCountText;
    public Text messageText;
    public bool makeRaftAttepted;
    
    // Use this for initialization
    void Start () {
        tooltip = tooltipObject;
        tooltipText = tooltipTextObject;
        visualText = visualTextObject;
        stackSplitStatic = stackSplit;
        canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        createLayout();
        movingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
	}
	
	// Update is called once per frame
	void Update () {
	
        if(hover != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y - hoverOffset);
            hover.transform.position = canvas.transform.TransformPoint(position);
        }


        if (Input.GetMouseButtonUp(0)) 
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null) 
            {
                from.GetComponent<Image>().color = Color.white;
                from.clearSlot();
                Destroy(GameObject.Find("Hover"));
                to = null;
                from = null;
                emptySlots++;
            }
            else if(!eventSystem.IsPointerOverGameObject(-1) && !movingSlot.isEmpty)
            {
                movingSlot.clearSlot();
                Destroy(GameObject.Find("Hover"));
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(CanvasGroup.alpha > 0)
            {
                StartCoroutine("fadeOut");
                putItemBack();
            }
            else
            {
                StartCoroutine("fadeIn");
            }
        }

        checkLogCount();
        updateMessages();
	}

    private void createLayout()
    {
        slots = new List<GameObject>();
        hoverOffset = slotSize * 0.01f;
        EmptySlots = slotNum;
        inventoryWidth = (slotNum / rowNum) * (slotSize + slotPadding) + slotPadding;
        inventoryHeight = rowNum * (slotSize + slotPadding) + slotPadding;
        inventoryTransform = GetComponent<RectTransform>();
        inventoryTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        int cols = slotNum / rowNum;

        for(int y = 0; y < rowNum; y++)
        {
            for(int x = 0; x < cols; x++)
            {
                GameObject newSlot = (GameObject) Instantiate(slotPrefab);
                RectTransform slotTransform = newSlot.GetComponent<RectTransform>();
                newSlot.name = "Slot";
                newSlot.transform.SetParent(this.transform.parent);

                float xPosition = slotPadding * (x + 1) + (slotSize * x);
                float yPostion = -slotPadding * (y + 1) - (slotSize * y);

                slotTransform.localPosition = inventoryTransform.localPosition + new Vector3(xPosition, yPostion);
                slotTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);


                newSlot.transform.SetParent(this.transform);

                slots.Add(newSlot);
            }
        }

    }

    public bool addItem(Item item)
    {
        if(item.maxSize == 1)
        {
            placeEmpty(item);
            return true;
        }
        else
        {
            foreach(GameObject slot in slots)
            {
                Slot temp = slot.GetComponent<Slot>();

                if (!temp.isEmpty)
                {
                    if(temp.currentItem.type == item.type && temp.spaceAvailable)
                    {
                        if(!movingSlot.isEmpty && slotClicked.GetComponent<Slot>() == temp.GetComponent<Slot>())
                        {
                            continue;
                        }
                        else
                        {
                            temp.addItem(item);
                            return true;
                        }
                        
                    }
                }
            }
            if(EmptySlots > 0)
            {
                placeEmpty(item);
                return true;
            }
         
        }
        return false;
    }
    
    private bool placeEmpty(Item item)
    {
        if(EmptySlots > 0)
        {
            foreach(GameObject slot in slots)
            {
                Slot temp = slot.GetComponent<Slot>();

                if (temp.isEmpty)
                {
                    temp.addItem(item);
                    EmptySlots--;
                    return true;
                }
            }
        }
        return false;
    }

    public void moveItem(GameObject slotClicked)
    {
        Inventory.slotClicked = slotClicked;

        if (!movingSlot.isEmpty)
        {
            Slot temp = slotClicked.GetComponent<Slot>();

            if (temp.isEmpty)
            {
                temp.addItems(movingSlot.Items);
                movingSlot.Items.Clear();
                Destroy(GameObject.Find("Hover"));
            }
            else if (!temp.isEmpty && movingSlot.currentItem.type == temp.currentItem.type && temp.spaceAvailable)
            {
                mergeStacks(movingSlot, temp);
            }
        }
        else if (from == null && canvasGroup.alpha == 1 && !Input.GetKey(KeyCode.LeftShift)) 
        {
            if (!slotClicked.GetComponent<Slot>().isEmpty)
            {
                from = slotClicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.green;

                createHoverIcon();
            }
        }
        else if(to == null && canvasGroup.alpha == 1 && !Input.GetKey(KeyCode.LeftShift))
        {
            to = slotClicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }
        
        if(from != null && to != null)
        {
            if(!to.isEmpty && from.currentItem.type == to.currentItem.type && to.spaceAvailable)
            {
                mergeStacks(from, to);
            }
            else
            {
                Stack<Item> tempTo = new Stack<Item>(to.Items);
                to.addItems(from.Items);

                if(tempTo.Count == 0)
                {
                    from.clearSlot();
                }
                else
                {
                    from.addItems(tempTo);
                }

                from.GetComponent<Image>().color = Color.white;
                to = null;
                from = null;
                Destroy(GameObject.Find("Hover"));
            }
            
        }
    }

    private void createHoverIcon()
    {
        hover = Instantiate(icon);
        hover.GetComponent<Image>().sprite = slotClicked.GetComponent<Image>().sprite;
        hover.name = "Hover";

        RectTransform hoverTransform = hover.GetComponent<RectTransform>();
        RectTransform slotClickedTransform = slotClicked.GetComponent<RectTransform>();

        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotClickedTransform.sizeDelta.x);
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotClickedTransform.sizeDelta.y);

        hover.transform.localScale = slotClicked.transform.localScale;
        hover.transform.SetParent(GameObject.Find("Canvas").transform, true);

        if(movingSlot.Items.Count > 1)
        {
            hover.transform.GetChild(0).GetComponent<Text>().text = movingSlot.Items.Count.ToString();
        }
        else
        {
            hover.transform.GetChild(0).GetComponent<Text>().text = null;
        }
    }

    private void putItemBack()
    {
        if(from != null)
        {
            Destroy(GameObject.Find("Hover"));
            from.GetComponent<Image>().color = Color.white;
            from = null;
        }
        else if (!movingSlot.isEmpty)
        {
            Destroy(GameObject.Find("Hover"));
            foreach(Item item in movingSlot.Items)
            {
                slotClicked.GetComponent<Slot>().addItem(item);
            }
        }
        stackSplit.SetActive(false);
    }

    public void setStackInfo(int stackCountMax)
    {
        stackSplit.SetActive(true); 
        tooltip.SetActive(false);
        splitAmount = 0;
        this.stackCountMax = stackCountMax;
        splitText.text = splitAmount.ToString();
    }

    public void splitStack()
    {
        stackSplit.SetActive(false);
        if(splitAmount == stackCountMax) 
        {
            moveItem(slotClicked);
        }
        else if(splitAmount > 0) 
        {
            movingSlot.Items = slotClicked.GetComponent<Slot>().removeItems(splitAmount);
            createHoverIcon();
        }
    }

    public void changeSplitText(int amount)
    {
        splitAmount += amount;
        if(splitAmount < 0)
        {
            splitAmount = 0;
        }
        if(splitAmount > stackCountMax)
        {
            splitAmount = stackCountMax;
        }

        splitText.text = splitAmount.ToString();
    }

    public void mergeStacks(Slot moving, Slot destination)
    {
        int max = destination.currentItem.maxSize - destination.Items.Count;

        int count;
        if(moving.Items.Count > max)
        {
            count = max;
        }
        else
        {
            count = moving.Items.Count;
        }

        for(int i = 0; i < count; i++)
        {
            destination.addItem(moving.removeItem());
            hover.transform.GetChild(0).GetComponent<Text>().text = count.ToString();
           
        } 
        if(moving.Items.Count == 0)
        {
            moving.clearSlot();
            Destroy(GameObject.Find("Hover"));
        }
    }

    public void showTooltip(GameObject slot)
    {
        Slot temp = slot.GetComponent<Slot>();

        if(!temp.isEmpty && icon == null && !stackSplitStatic.activeSelf)
        {
            tooltipText.text = temp.currentItem.tooltipInfo();
            visualText.text = tooltipText.text;

            tooltip.SetActive(true);

            RectTransform slotRect = slot.GetComponent<RectTransform>();

            float xPosition = slot.transform.position.x + slotPadding + slotRect.sizeDelta.x;
            float yPosition = slot.transform.position.y - slotPadding;

            tooltip.transform.position = new Vector2(xPosition, yPosition);
        }

        
    }

    public void hideTooltip()
    {
        tooltip.SetActive(false);
    }

    private void checkLogCount()
    {
        logCount = 0;

        foreach (GameObject slot in slots)
        {
            Slot temp = slot.GetComponent<Slot>();
            if(!temp.isEmpty && temp.currentItem.type == ItemType.LOG)
            {
                LogCount++;
                logCountText.text = LogCount + " logs collected";
            }
        }
    }

    public void updateMessages()
    {
        foreach (GameObject slot in slots)
        {
            Slot temp = slot.GetComponent<Slot>();
            if (!temp.isEmpty && temp.currentItem.type == ItemType.AXE)
            {
                messageText.text = "Right click on axe to use it";
                makeRaftAttepted = false;
            }
        }

        GameObject axe = GameObject.Find("axe");
        if (axe != null && axe.activeSelf && !makeRaftAttepted)
        {
            messageText.text = "You can now chop down tress to collect logs";
        }
    }

    public void makeRaft()
    {
        makeRaftAttepted = true;
        if (logCount < 6)
        {
            messageText.text = "You need 6 logs to make a raft";
        }
        else if (logCount >= 6 && !PlayerCommands.onBeach)
        {
            messageText.text = "You need to be on the beach to make a raft";
        }
        else if (logCount >= 6 && PlayerCommands.onBeach)
        {
            //load winning scene
            Debug.Log("Winner");
        }
    }

    private IEnumerator fadeOut()
    {
        if (!fadingOut)
        {
            fadingOut = true;
            fadingIn = false;
            StopCoroutine("fadeIn");

            float currentAlpha = CanvasGroup.alpha;
            float fadeRate = 1.0f / fadeTime;
            float progress = 0.0f;

            while(progress < 1.0)
            {
                CanvasGroup.alpha = Mathf.Lerp(currentAlpha, 0, progress);
                progress += fadeRate * Time.deltaTime;
                yield return null; 
            }
            CanvasGroup.alpha = 0; 
            fadingOut = false;
        }
    }

    private IEnumerator fadeIn()
    {
        if (!fadingIn)
        {
            fadingIn = true;
            fadingOut = false;
            StopCoroutine("fadeOut");

            float currentAlpha = CanvasGroup.alpha;
            float fadeRate = 1.0f / fadeTime;
            float progress = 0.0f;

            while (progress < 1.0)
            {
                CanvasGroup.alpha = Mathf.Lerp(currentAlpha, 1, progress);
                progress += fadeRate * Time.deltaTime;
                yield return null; 
            }
            CanvasGroup.alpha = 1; 
            fadingIn = false;
        }
    }
}
