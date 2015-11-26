using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

    public float minutes = 1.0f;
    float seconds;
    float timer;
    float percentage;
    float speed;

	// Use this for initialization
	void Start () {
        seconds = minutes * 60.0f;
        timer = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        checkTime();
        updateLights();
        rotate();
    }

    void checkTime()
    {
        timer += Time.deltaTime;
        percentage = timer / seconds;
        if(timer > seconds)
        {
            timer = 0.0f;
        }
    }

    void updateLights()
    {
        Light light = GetComponent<Light>();
        bool night = false;
        if (percentage > 0.5f)
        {
            night = true;
        }
        if (night)
        {
            if(light.intensity > 0.0f)
            {
                light.intensity -= 0.05f;
            }
        }
        else
        {
            if(light.intensity < 1.0f)
            {
                light.intensity += 0.05f;
            }
        }
    }

    void rotate()
    {
        speed = 360.0f / seconds * Time.deltaTime;
        transform.RotateAround(transform.position, transform.right, speed);
    }
}
