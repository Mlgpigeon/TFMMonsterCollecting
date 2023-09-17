using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBackBox : MonoBehaviour, IPointerEnterHandler
{
    private UnityEngine.UI.Button _button;

    public GameObject boxLot;
    void Start()
    {
        _button = this.GetComponent<UnityEngine.UI.Button>();
        _button.onClick.AddListener(prevBox);
    }
    
    void prevBox()
    {
        boxLot.GetComponent<boxSelection>().prevBox();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped != null)
        {
            prevBox(); 
        }
        
    }
}
