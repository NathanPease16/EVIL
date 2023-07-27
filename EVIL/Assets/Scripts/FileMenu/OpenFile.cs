using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenFile : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [Header("Opening")]
    [SerializeField] private string fileToOpen;
    [SerializeField] private float timeToDoubleClick;
    private float currentClickTime;
    private bool hasClicked;

    [Header("Directory")]
    [SerializeField] private GameObject directory;
    [SerializeField] private bool isDirectory;

    [Header("Values")]
    [SerializeField] private float unselected;
    [SerializeField] private float highlighted;
    [SerializeField] private float selected;
    private bool isOver;
    private bool isHighlighted;
    private bool isSelected;

    [Header("References")]
    private Image highlighter;

    void Awake()
    {
        highlighter = transform.Find("Selected").GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isOver)
            {
                isSelected = false;
                highlighter.color = new Color(highlighter.color.r, highlighter.color.g, highlighter.color.b, unselected);
            }
            else if (isOver && !hasClicked)
                hasClicked = true;
            else if (hasClicked && currentClickTime <= timeToDoubleClick)
            {
                if (!isDirectory)
                {
                    InstanceData.SetBool("Open" + fileToOpen, true);
                }
                else
                {

                    isOver = false;
                    isSelected = false;
                    isHighlighted = false;
                    hasClicked = false;
                    currentClickTime = 0;
                    highlighter.color = new Color(highlighter.color.r, highlighter.color.g, highlighter.color.b, unselected);

                    if (directory == null) return;

                    DirectoryManager.currentDirectory = directory;
                    DirectoryManager.parentDirectories.Push(transform.parent.gameObject);
                    
                    DirectoryManager.currentDirectory.SetActive(true);
                    DirectoryManager.parentDirectories.Peek().SetActive(false);
                }
                hasClicked = false;
                currentClickTime = 0;

            }
        }
        if (currentClickTime > timeToDoubleClick)
        {
            hasClicked = false;
            currentClickTime = 0;
        }

        if (hasClicked)
            currentClickTime += Time.deltaTime;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isHighlighted = false;
        isSelected = true;
        highlighter.color  = new Color(highlighter.color.r, highlighter.color.g, highlighter.color.b, selected);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;
        if (!isSelected)
        {
            isHighlighted = true;
            highlighter.color  = new Color(highlighter.color.r, highlighter.color.g, highlighter.color.b, highlighted);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
        if (isHighlighted)
            highlighter.color = new Color(highlighter.color.r, highlighter.color.g, highlighter.color.b, unselected);
    }
}
