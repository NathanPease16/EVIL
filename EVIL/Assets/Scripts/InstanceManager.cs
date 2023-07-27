using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using System;

public class InstanceManager : MonoBehaviour
{
    [Header("Processes")]
    private static List<Process> processes;

    [Header("ID")]
    private static int id;
    private static string idFileName = "IDs";
    private static string startSceneFileName = "StartScene";

    private void Awake()
    {
        Application.wantsToQuit += WantsToQuit;
        processes = new List<Process>();

        id = GetID();
        LoadScene();
    }

    public static void CreateNewInstance(int scene=0)
    {
        if (id != 0) return; 
        InstanceData.SetInt(startSceneFileName, scene);
        string appPath = Path.Combine(InstanceData.AppDirectory, Application.productName + ".exe");
        processes.Add(Process.Start(appPath));
    }

    private int GetID()
    {
        if (InstanceData.Exists(idFileName))
        {
            int currentID = InstanceData.GetInt(idFileName);
            InstanceData.SetInt(idFileName, currentID + 1);

            return currentID;
        }

        InstanceData.SetInt(idFileName, 1);
        return 0;
    }

    private void LoadScene()
    {
        if (InstanceData.Exists(startSceneFileName))
        {
            int scene = InstanceData.GetInt(startSceneFileName);
            if (scene == 0)
                return;
            SceneManager.LoadScene(scene);
        }
    }

    private bool WantsToQuit()
    {
        if (id == 0)
        {
            foreach(string path in Directory.GetFiles(InstanceData.DataDirectory))
                File.Delete(path);

            foreach (Process process in processes)
                process.Kill();
        }
        return true;
    }
}
