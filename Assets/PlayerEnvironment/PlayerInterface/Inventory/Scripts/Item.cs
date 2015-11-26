using UnityEngine;
using System.Collections;

public enum ItemType {AXE, LOG, BERRY, COCONUT};

public class Item : MonoBehaviour {

    public ItemType type;
    public Sprite spriteNeutral, spriteHighlighted;
    public int maxSize;
    public string itemName, itemDescription;
    public static bool useAxe;
    private Health health;

	public void use()
    {
        health = GameObject.Find("FPSController").GetComponent<Health>();
        switch (type)
        {
            case ItemType.AXE:
                useAxe = true;
                break;
            case ItemType.LOG:
                Debug.Log("Go to beach");
                break;
            case ItemType.BERRY:
                health.changeHealth(+1);
                break;
            case ItemType.COCONUT:
                health.changeHealth(+5); ;
                break;
        }
    }

    public string tooltipInfo()
    {
        string info = string.Empty;
        string color = string.Empty;

        switch (type)
        {
            case ItemType.AXE:
                color = "green";
                break;
            case ItemType.LOG:
                color = "brown";
                break;
            case ItemType.BERRY:
                color = "blue";
                break;
            case ItemType.COCONUT:
                color = "yellow";
                break;
        }

        info = "<color=" + color + "><size=16>" + itemName + "</size></color><size=14>\n" + itemDescription + "</size>";
        return info;
    }
}
