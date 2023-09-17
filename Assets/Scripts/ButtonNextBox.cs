using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNextBox : MonoBehaviour, IPointerEnterHandler
{
    private UnityEngine.UI.Button _button;

    public GameObject boxLot;
    void Start()
    {
        _button = this.GetComponent<UnityEngine.UI.Button>();
        _button.onClick.AddListener(nextBox);
    }
    void nextBox()
    {
        boxLot.GetComponent<boxSelection>().nextBox();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped != null)
        {
            nextBox(); 
        }
        
    }
}
