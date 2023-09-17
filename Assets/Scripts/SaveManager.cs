using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.Rendering;


public class SaveManager : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject teamBox;
    public GameObject boxLot;
    private List<BoxSlot> teamBoxSlots;
    private List<BoxSlot> boxSlots; // Reference to the FirstLoad script
    public GameObject boxItemPrefab;
    public Species[] species;
    private string saveFilePath;
    private void Start()
    {
        
        // Get the save file path
        print(Application.persistentDataPath);
        saveFilePath =Path.Combine(Application.persistentDataPath, "saveData.json");
        
        species = Resources.LoadAll("Species",typeof(Species)).Cast<Species>().ToArray();
        teamBoxSlots = teamBox.GetComponentsInChildren<BoxSlot>().ToList();
        List<BoxSlot> auxBoxSlots = new List<BoxSlot>();
        for (int i = 0; i < boxLot.transform.childCount; i++)
        {
            Transform box = boxLot.transform.GetChild(i);
            BoxSlot[] boxSlotChildren = box.GetComponentsInChildren<BoxSlot>();
            auxBoxSlots.AddRange(boxSlotChildren);
        }
        boxSlots = auxBoxSlots.ToList(); // Add unique BoxSlot components to the boxSlots list

        // Check if the save file exists
        print(File.Exists(saveFilePath));
        if (File.Exists(saveFilePath))
        {
            // Save file exists
            Debug.Log("Save file found!");
            LoadGame();
        }
        else
        {
            // Save file does not exist
            Debug.Log("No save file found. Starting a new game.");
        }
    }
    
    private void LoadGame()
    
    {
        string saveDataJson = File.ReadAllText(saveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(saveDataJson);

        // Restore the monster data
        List<MonsterSaveData> monsterSaveDataList = saveData.monsterSaveDataList;
        RestoreMonsterData(monsterSaveDataList);

        // Restore the player position
        Vector3 playerPosition = saveData.playerPosition;
        RestorePlayerPosition(playerPosition);

    }
    
    private void RestoreMonsterData(List<MonsterSaveData> monsterSaveDataList)
    {
        // Iterate over the monster save data and restore the monsters
        for (int i = 0; i < monsterSaveDataList.Count; i++)
        {
            MonsterSaveData monsterSaveData = monsterSaveDataList[i];
            // Restore the monster using the saved data
            InstantiateMonster(monsterSaveData);
        }
    }

    private void RestorePlayerPosition(Vector3 playerPosition)
    {
        // Restore the player position
        playerTransform.position = playerPosition;
    }

    private void InstantiateMonster(MonsterSaveData monsterSaveData)
    {
    // Find the corresponding species based on the species name
    Species speciesSingle = species.FirstOrDefault(s => s.speciesId == monsterSaveData.species);
    BoxSlot slot = new BoxSlot();
    if (monsterSaveData.isInTeamBoxSlots)
    {
        slot = teamBoxSlots[monsterSaveData.indexInSlot];
    }
    else if (monsterSaveData.isInBoxSlots)
    {
        slot = boxSlots[monsterSaveData.indexInSlot];
    }
    // Check if the species is found
    if (species != null)
    {
        // Create a new Monster object using the species and save data
        Monster monster = new Monster(speciesSingle, monsterSaveData.shiny, monsterSaveData.size, monsterSaveData.currentLevel);
        monster.currentExp = monsterSaveData.currentExp;
        monster.currentHealth = monsterSaveData.currentHealth;

        GameObject monsterGO;
        // Create a new GameObject to represent the monster
        if (slot.transform.childCount > 0)
        {
            monsterGO = slot.transform.GetChild(0).gameObject;
        }
        else
        {
             monsterGO = Instantiate(boxItemPrefab, slot.transform);
        }
       

        // Get the BoxItem component attached to the monster GameObject
        BoxItem boxItem = monsterGO.GetComponent<BoxItem>();

        // Set the monster data for the BoxItem
        boxItem.InitialiseItem(monster);

    }
}


    public void Save()
    {
        // Create a list to store the monster save data
        List<MonsterSaveData> monsterSaveDataList = new List<MonsterSaveData>();

        // Iterate over the teamBoxSlots and add the monster save data
        for (int i = 0; i < teamBoxSlots.Count; i++)
        {
            BoxSlot slot = teamBoxSlots[i];
            BoxItem monsterInSlot = slot.GetComponentInChildren<BoxItem>();

            if (monsterInSlot != null)
            {
                Monster monster = monsterInSlot.monster;
                MonsterSaveData monsterSaveData = new MonsterSaveData(monster, true, false, i);
                monsterSaveDataList.Add(monsterSaveData);
            }
        }

        // Iterate over the boxSlots and add the monster save data
        for (int i = 0; i < boxSlots.Count; i++)
        {
            BoxSlot slot = boxSlots[i];
            BoxItem monsterInSlot = slot.GetComponentInChildren<BoxItem>();

            if (monsterInSlot != null)
            {
                Monster monster = monsterInSlot.monster;
                MonsterSaveData monsterSaveData = new MonsterSaveData(monster, false, true, i);
                monsterSaveDataList.Add(monsterSaveData);
            }
        }

        // Convert the monsterSaveDataList to JSON string using JsonUtility
        // Create a SaveData object and assign the monster data and player position
        SaveData saveData = new SaveData();
        saveData.monsterSaveDataList = monsterSaveDataList;
        Vector3 offsetPos = new Vector3(playerTransform.position.x, playerTransform.position.y + 1f,
            playerTransform.position.z);
        saveData.playerPosition = offsetPos;

        string saveDataJson = JsonUtility.ToJson(saveData, true);
        
        File.WriteAllText(saveFilePath, saveDataJson);
        Debug.Log("Save data: " + saveDataJson);
    }
}

[System.Serializable]
public class SaveData
{
    public List<MonsterSaveData> monsterSaveDataList;
    public Vector3 playerPosition;
}

[System.Serializable]
public class MonsterSaveData
{
    public string customName;
    public bool shiny;
    public string species;
    public float size;
    public int currentLevel;
    public int currentExp;
    public int currentHealth;
    public bool isInTeamBoxSlots; // Indicates whether the monster is in the teamBoxSlots
    public bool isInBoxSlots; // Indicates whether the monster is in the boxSlots
    public int indexInSlot; // Index of the monster in the slot

    public MonsterSaveData()
    {
        // Empty constructor for serialization
    }
    public MonsterSaveData(Monster monster, bool inTeamBoxSlots, bool inBoxSlots, int slotIndex)
    {
        customName = monster.customName;
        shiny = monster.shiny;
        species = monster.species.speciesId;
        size = monster.size;
        currentLevel = monster.currentLevel;
        currentExp = monster.currentExp;
        currentHealth = monster.currentHealth;
        isInTeamBoxSlots = inTeamBoxSlots;
        isInBoxSlots = inBoxSlots;
        indexInSlot = slotIndex;
    }
}
