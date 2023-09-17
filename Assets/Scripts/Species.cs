using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Species")]
public class Species : ScriptableObject
{
    public string speciesName;
    public string speciesId;
    public Sprite sprite;
    public Sprite shinySprite;
    public GrowRate growth;
    [Range(1, 101)] // Set the range from 2 to 100
    public int EvolutionLevel;
    public Species speciesEvolution;
    public enum GrowRate
    {  
        Slow,
        Medium,
        Fast
    }
}
