using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuVariables : MonoBehaviour
{
    public GameObject saveManager;
    public GameObject save;
    public GameObject enciclopedia;
    public GameObject quit;

    void Start()
    {
        saveManager = GameObject.Find("SaveManager");
        Button saveButton = save.GetComponent<Button>();
        saveButton.onClick.AddListener(() =>{ saveManager.GetComponent<SaveManager>().Save();});
        Button enciclopediaButton = enciclopedia.GetComponent<Button>();
    }

}
