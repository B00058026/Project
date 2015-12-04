using UnityEngine;
using System.Collections;

public enum TreeType { PALM, BERRY };

public class TreeHealth : MonoBehaviour {

    public TreeType type;
    private int health;
    private Camera cam;
    public GameObject axe;
    private float timer;
    public GameObject[] pickups;

    // Use this for initialization
    void Start () {
        if(type == TreeType.PALM)
        {
            health = 5;
        }
        else if(type == TreeType.BERRY)
        {
            health = 1;
        }

        cam = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
        timer = 0f; 
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        if (health > 0 && axe.activeSelf)
        {
            if (Vector3.Distance(transform.position, cam.transform.root.transform.position) < 5f)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && timer >= 1.0f)
                {
                    timer = 0f;
                    Ray ray = new Ray(cam.transform.position, cam.transform.forward);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 5f))
                    {
                        if (hit.collider.gameObject == gameObject)
                        {
                            health--;
                        }
                    }
                }
            }
        }
        
        if (health <= 0)
        {
            if(type == TreeType.PALM)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().AddTorque(cam.transform.right * 10f, ForceMode.Impulse);
                timer += Time.deltaTime;
                if (timer >= 7.0f)
                {
                    createPickups();
                }
            }
            else if(type == TreeType.BERRY)
            {
                createPickups();
            }
            
        }

    }

    void createPickups()
    {
        Destroy(gameObject);

        foreach(GameObject pickup in pickups)
        {
            Instantiate(pickup, transform.position, transform.rotation);
        }
    }



}
