using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public List<Canvas> pages;
    public int startPageIndex;
    private Canvas leftPage;
    private Canvas rightPage;

    private void Start()
    {
        startPageIndex = 0;
    }

    private void Update()
    {
        ShowUI();
    }

    private void NextPage()
    {
        startPageIndex++;
        startPageIndex++;
    }
    private void PreviousPage()
    {
        startPageIndex--;
        startPageIndex--;
    }

    private void ShowUI()
    {
        leftPage = pages[startPageIndex];
        rightPage = pages[startPageIndex + 1];
    }
}
