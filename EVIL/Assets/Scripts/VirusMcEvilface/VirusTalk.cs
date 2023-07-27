using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class VirusTalk : MonoBehaviour
{
    [Header("Virus McEvilface")]
    [SerializeField] private Image virus;

    [Header("Typing")]
    [SerializeField] private float timeBetweenKeys;

    [Header("Dialogue")]
    [SerializeField] private bool playOnStart;
    [SerializeField] private bool openSceneOnFinish;
    [SerializeField] private string sceneName;
    [SerializeField] private DialogueLine[] lines;
    private bool isTyping;
    private bool skip;
    private bool hasSkipped;

    [Header("References")]
    private TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        if (playOnStart)
            Trigger();
    }

    void Update()
    {
        if (skip && !hasSkipped) return;
        skip = Input.GetMouseButtonDown(0) && isTyping;
    }

    public void Trigger()
    {
        StartCoroutine(DialogueRoutine());
    }

    private IEnumerator DialogueRoutine()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            text.text = "";
            string line = lines[i].line;
            line += " [LMB]";

            virus.sprite = lines[i].face;

            isTyping = true;
            foreach (char character in line)
            {
                if (skip)
                {
                    hasSkipped = true;
                    break;
                }
                text.text += character;
                yield return new WaitForSeconds(timeBetweenKeys);
            }
            hasSkipped = false;
            isTyping = false;
            skip = false;

            text.text = line;
            yield return null;

            while (!Input.GetMouseButtonDown(0))
                yield return null;
            
            yield return null;
        }

        text.text = "";

        if (openSceneOnFinish)
            InstanceData.SetBool("Open" + sceneName, true);
    }
}
