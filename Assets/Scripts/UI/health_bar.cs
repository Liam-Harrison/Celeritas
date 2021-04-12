using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health_bar : MonoBehaviour
{
	public Slider slider;

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetHealth(int health)
	{
		slider.value = health;
	}

	public void SetMaxHealth(int maxHealth)
	{
		slider.maxValue = maxHealth;
	}
}
