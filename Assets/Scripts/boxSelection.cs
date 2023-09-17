using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class boxSelection : MonoBehaviour
{
    private int currentActiveBox;
    public GameObject titleText;

    private int maxBoxes;
    void Start()
    {
        maxBoxes = this.transform.childCount;
        this.transform.GetChild(0).gameObject.SetActive(true);
        currentActiveBox = 0;
        titleText.GetComponent<TextMeshProUGUI>().text = this.transform.GetChild(0).gameObject.name;
        for (int i = 1; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void nextBox()
    {
        if ((currentActiveBox + 1) >= maxBoxes)
        {
            this.transform.GetChild(currentActiveBox).gameObject.SetActive(false);
            this.transform.GetChild(0).gameObject.SetActive(true);
            titleText.GetComponent<TextMeshProUGUI>().text = this.transform.GetChild(0).gameObject.name;
            currentActiveBox = 0;
        }
        else
        {
            this.transform.GetChild(currentActiveBox).gameObject.SetActive(false);
            currentActiveBox++;
            this.transform.GetChild(currentActiveBox).gameObject.SetActive(true);
            titleText.GetComponent<TextMeshProUGUI>().text = this.transform.GetChild(currentActiveBox).gameObject.name;
        }
    }
    
    public void prevBox()
    {
        if (currentActiveBox == 0)
        {
            this.transform.GetChild(currentActiveBox).gameObject.SetActive(false);
            this.transform.GetChild(maxBoxes-1).gameObject.SetActive(true);
            titleText.GetComponent<TextMeshProUGUI>().text = this.transform.GetChild(maxBoxes - 1).gameObject.name;
            currentActiveBox = maxBoxes-1;
        }
        else
        {
            this.transform.GetChild(currentActiveBox).gameObject.SetActive(false);
            currentActiveBox--;
            this.transform.GetChild(currentActiveBox).gameObject.SetActive(true);
            titleText.GetComponent<TextMeshProUGUI>().text = this.transform.GetChild(currentActiveBox).gameObject.name;
        }
    }
}
