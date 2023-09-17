using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public void UpdateHealth(Monster monster)
    {
        if (monster != null)
        {
            // Calculate the fill amount based on currentHealth and maxHealth
            float fillAmount = (float)monster.currentHealth / monster.maxHealth;
            
            // Update the fill amount of the fill image
            this.GetComponent<Image>().fillAmount = fillAmount;
        }
    }
}
