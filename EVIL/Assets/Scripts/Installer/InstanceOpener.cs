using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class InstanceOpener : MonoBehaviour
{
    [SerializeField] private List<InstanceOpenData> watchFor;

    void Start()
    {
        StartCoroutine(CheckAndOpen());
    }

    private IEnumerator CheckAndOpen()
    {
        while (true)
        {
            for (int i = 0; i < watchFor.Count; i++)
            {
                InstanceOpenData data = watchFor[i];

                if (InstanceData.Exists("Open" + data.name))
                {
                    if (InstanceData.GetBool("Open" + data.name))
                    {
                        InstanceData.SetBool("Open" + data.name, false);
                        InstanceManager.CreateNewInstance(data.sceneID);
                    }
                }
            }
            yield return new WaitForSeconds(.5f);
        }
    }
}
