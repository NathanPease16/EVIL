using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePages : MonoBehaviour
{
    [Header("Pages")]
    [SerializeField] private GameObject[] pages;
    private int currentPage;
    public bool allowNext;
    public bool allowBack;

    [Header("Singleton")]
    public static MovePages instance;

    private void Awake()
    {
        instance = this;
    }

    public void Next()
    {
        if (currentPage == pages.Length - 1 || !allowNext) return;

        pages[currentPage].SetActive(false);
        currentPage++;
        pages[currentPage].SetActive(true);
    }

    public void Back()
    {
        if (currentPage == 0 || !allowBack) return;

        pages[currentPage].SetActive(false);
        currentPage--;
        pages[currentPage].SetActive(true);
    }
}
