using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectoryManager : MonoBehaviour
{
    public static GameObject currentDirectory;
    public static Stack<GameObject> parentDirectories;

    void Awake()
    {
        parentDirectories = new Stack<GameObject>();
    }

    public static void Previous()
    {
        if (parentDirectories.Count == 0) 
        {
            Debug.Log("0!");
            return;
        }

        currentDirectory.SetActive(false);
        GameObject dir = parentDirectories.Pop();
        dir.SetActive(true);

        currentDirectory = dir;
    }
}
