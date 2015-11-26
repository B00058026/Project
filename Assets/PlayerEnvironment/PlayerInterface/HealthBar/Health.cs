using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour {

    public float maxHealth;
    private float currentHealth;
    private Image healthBar;
    private Text healthText;
    private float currentFill;
    private int speed = 5;

    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        healthText.text = "Health: " + CurrentHealth;
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, convertHealth(CurrentHealth), Time.deltaTime * speed);

        if(currentHealth <= 0)
        {
            //Application.LoadLevel(3);
        }
    }
     
    public void changeHealth(float percent)
    {
        currentHealth = (CurrentHealth + percent);
        if(CurrentHealth < 0)
        {
            currentHealth = 0;
        }
        else if(CurrentHealth > 100)
        {
            currentHealth = 100;
        }
    } 

    public float convertHealth(float currentHealth)//converting currrent health to value between 0 - 1;
    {
        return currentHealth / 100;
    }

    
}
