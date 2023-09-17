using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMonster : MonoBehaviour
{
    public BoxSlot freedomBox;

    public void freeMonster()
    {
        if (freedomBox.transform.childCount > 0)
        {
            Destroy(freedomBox.transform.GetChild(0).gameObject);
        }
    }
}
