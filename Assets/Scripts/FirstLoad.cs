using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class FirstLoad : MonoBehaviour
{
    public Species[] species;
    // Start is called before the first frame update
    void Start()
    {
        species = Resources.LoadAll("Species",typeof(Species)).Cast<Species>().ToArray();
        foreach (var t in species)
        {
            print(t.speciesName);
        }
    }
}
