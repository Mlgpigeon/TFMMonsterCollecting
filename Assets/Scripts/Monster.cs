using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster")]
public class Monster : ScriptableObject
{
   public string customName;
   public bool shiny;
   public Species species;
   public float size;
   public int currentLevel;
   public int currentExp;
   public int currentHealth;
   public int maxHealth => currentLevel * 5;
   public int levelExpCeil => (currentLevel + 1) * 100;
   
   public Monster(Species newSpecies, bool newShiny, float newSize, int level)
   {
      species = newSpecies;
      shiny = newShiny;
      size = newSize;
      currentLevel = level;
      currentHealth = maxHealth;
   }

   public bool gainExp(int exp)
   {
      bool evolved = false;
      if (currentLevel == 100)
      {
         currentExp = 0;
      }
      else
      {
         currentExp += exp; 
         if (currentExp >= levelExpCeil)
         {
           evolved = levelUp();
         } 
      }

      return evolved;
   }
   public bool levelUp()
   {
      bool evolved = false;
      while (currentExp >= levelExpCeil && currentLevel <= 100)
      {
         currentExp = currentExp - levelExpCeil;
         currentLevel++;
         currentHealth = maxHealth;
         
         if (currentLevel == species.EvolutionLevel) { 
            evolved= evolve();
         }
      }

      return evolved;
   }

   public bool evolve()
   {
      species = species.speciesEvolution;
      return true;
   }
   public void restoreHealth()
   {
      currentHealth = maxHealth;
   }

   public bool receiveDamage(int damage)
   {
      currentHealth = currentHealth - damage;
      return fainted();
   }

   public bool fainted()
   {
      if (currentHealth <= 0)
      {
         return true;
      }
      return false;
   }

   public GameObject spawn()
   {
      GameObject spawned = Instantiate(Resources.Load("Monsters/"+species.speciesId +"/"+species.speciesId) as GameObject);
      if (shiny)
      {
         foreach (Transform child in spawned.transform)
         {
            SkinnedMeshRenderer skinned = child.GetComponent<SkinnedMeshRenderer>();
            if (skinned != null)
            {
               string name = skinned.material.name;
               name = name.Substring(0, name.Length - 11);
               skinned.material =
                  Resources.Load<Material>("Monsters/" + species.speciesId + "/Files/Materials/Shiny/" + name);
            }
         }
      }
      spawned.GetComponent<MonsterController>().monster = this;
      return spawned;
   }
}
