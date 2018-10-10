using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class asd : MonoBehaviour {

    [MenuItem("Tools/去除所有BoxCollider")]
    static void RemoveBoxCollider()
    {
        var dixing = GameObject.Find("Map/dixing");
        var boxs = dixing.GetComponentsInChildren<BoxCollider>();
        foreach(var box in boxs)
        {
            GameObject.DestroyImmediate(box);
        }
    }
    [MenuItem("Tools/添加BoxCollider")]
    static void AddBoxCollider()
    {
        var dixing = GameObject.Find("Map/dixing");
        var boxs = dixing.GetComponentsInChildren<MeshRenderer>();
        foreach (var box in boxs)
        {
            if(box.enabled == true)
                box.gameObject.AddComponent<BoxCollider>();
        }
    }
}
