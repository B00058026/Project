using UnityEngine;
using System.Collections;

public class PlayerCommands : MonoBehaviour {

    public Inventory inventory;
    public GameObject UseAxe;
    private GameObject axe;
    private Animation axeAnimation;
    private Health health;
    public static bool onBeach;

    // Use this for initialization
    void Start () {
        health = GetComponent<Health>();  
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Item.useAxe)
        {
            UseAxe.SetActive(true);
        }

        if (UseAxe.activeSelf == true)
        {
            axe = GameObject.Find("axe");
            axeAnimation = axe.GetComponent<Animation>();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                axeAnimation.Play("AxeSwing");
                health.changeHealth(-10);
            }
        }

        
    }

    private void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag == "Item")
        {
            inventory.addItem(c.gameObject.GetComponent<Item>());
            Destroy(c.gameObject);
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        Debug.Log("Enter not in");
        if (c.gameObject.tag == "Beach")
        {
            onBeach = true;
            Debug.Log(onBeach);
        }

    }

    public void OnTriggerExit(Collider c)
    {
        Debug.Log("Exit not in");
        if (c.gameObject.tag == "Beach")
        {
            onBeach = false;
            Debug.Log(onBeach);
        }
    }

}
