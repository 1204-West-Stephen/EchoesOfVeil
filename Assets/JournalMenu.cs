using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JournalMenu : MonoBehaviour
{
    public List<string> pages;
    public int pageIndex;
    public TextMeshProUGUI leftPage;
    public TextMeshProUGUI rightPage;
    void Start()
    {
        pageIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ShowPages();
    }
    public void NextPage()
    {
        if (pageIndex < pages.Count)
        {
            pageIndex++;
            pageIndex++;
        }
        else
        {

        }
    }

    public void PreviousPage()
    {
        if (pageIndex > 0) 
        { 
            pageIndex--;
            pageIndex--;
        }
        else
        {
            
        }
    }

    public void ShowPages()
    {
        leftPage.text = pages[pageIndex];
        if (pageIndex + 1 < pages.Count)
        {
            rightPage.text = pages[pageIndex + 1];
        }
        else
        {
            rightPage.text = " ";
        }
    }
}
