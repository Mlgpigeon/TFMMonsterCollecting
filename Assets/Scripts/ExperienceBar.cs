using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public void UpdateExp(Monster monster)
    {
        if (monster != null)
        {
            // Calculate the fill amount based on currentHealth and maxHealth
            float fillAmount = (float)monster.currentExp / monster.levelExpCeil;
            
            // Update the fill amount of the fill image
            this.GetComponent<Image>().fillAmount = fillAmount;
        }
    }
}
