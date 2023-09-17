using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class GeneratePrefab : EditorWindow
{
    public GameObject prefab;
    
    [MenuItem("Window/Generate Prefab")]
    private static void OpenWindow()
    {
        GeneratePrefab window = GetWindow<GeneratePrefab>();
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate Prefab", EditorStyles.boldLabel);
        prefab = EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false) as GameObject;
        if (GUILayout.Button("Transform"))
        {
            addComponents();
            // Your code to run when the button is clicked
            Debug.Log("Transformed!");
        }
    }

    private void addComponents()
    {
        // Add components or modify the cloned prefab as needed
        if (prefab.GetComponent<Animator>() == null)
        {
            prefab.AddComponent<Animator>();
        }
        prefab.GetComponent<Animator>().enabled = true;
        
        if (prefab.GetComponent<NavMeshAgent>() == null)
        {
            prefab.AddComponent<NavMeshAgent>();
        }
        prefab.GetComponent<NavMeshAgent>().enabled = false;
        
        if (prefab.GetComponent<FollowController>() == null)
        {
            prefab.AddComponent<FollowController>();
        }
        prefab.GetComponent<FollowController>().enabled = false;
        
        if (prefab.GetComponent<MonsterAnimator>() == null)
        {
            prefab.AddComponent<MonsterAnimator>();
        }
        if (prefab.GetComponent<BoundingBox>() == null)
        {
            prefab.AddComponent<BoundingBox>();
        }
        if (prefab.GetComponent<BoxCollider>() == null)
        {
            prefab.AddComponent<BoxCollider>();
        }
        if (prefab.GetComponent<MonsterController>() == null)
        {
            prefab.AddComponent<MonsterController>();
        }
        if (prefab.GetComponent<RoamingAI>() == null)
        {
            prefab.AddComponent<RoamingAI>().enabled = false;
        }
        if (prefab.GetComponent<ExitCollider>() == null)
        {
            prefab.AddComponent<ExitCollider>();
        }
        if (prefab.GetComponent<Rigidbody>() == null)
        {
            prefab.AddComponent<Rigidbody>();
        }
        if (prefab.GetComponent<FightSystem>() == null)
        {
            prefab.AddComponent<FightSystem>();
        }
        prefab.GetComponent<Rigidbody>().useGravity = false;
        prefab.GetComponent<Rigidbody>().isKinematic = true;

        PrefabUtility.SavePrefabAsset(prefab);

    }
    
    
}
