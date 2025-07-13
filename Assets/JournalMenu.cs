using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum PageType
{
    Overview,
    EnemyData,
    HintsPage
}

public class JournalMenu : MonoBehaviour
{
    [Header("Pages")]
    public List<string> overviewPages;
    public List<string> enemyPages;
    public List<string> hintsPages;

    [Header("UI Elements")]
    public TextMeshProUGUI leftPage;
    public TextMeshProUGUI rightPage;

    private Dictionary<PageType, int> pageIndexes = new Dictionary<PageType, int>();
    private PageType currentPageType = PageType.Overview;

    void Start()
    {
        pageIndexes[PageType.Overview] = 0;
        pageIndexes[PageType.EnemyData] = 0;
        pageIndexes[PageType.HintsPage] = 0;
        ShowPages(currentPageType);
    }

    void Update()
    {
    }

    public void NextPage()
    {
        List<string> pages = GetPagesListForType(currentPageType);
        int currentIndex = pageIndexes[currentPageType];

        if (currentIndex < pages.Count - 1)
        {
            pageIndexes[currentPageType] = currentIndex + 1;
            ShowPages(currentPageType);
        }
    }

    public void PreviousPage()
    {
        List<string> pages = GetPagesListForType(currentPageType);
        int currentIndex = pageIndexes[currentPageType];

        if (currentIndex > 0)
        {
            pageIndexes[currentPageType] = currentIndex - 1;
            ShowPages(currentPageType);
        }
    }

    public void ShowPages(PageType pageType)
    {
        List<string> pages = GetPagesListForType(pageType);
        int currentIndex = pageIndexes[pageType];

        leftPage.text = pages[currentIndex];

        if (currentIndex + 1 < pages.Count)
        {
            rightPage.text = pages[currentIndex + 1];
        }
        else
        {
            rightPage.text = " ";
        }
    }

    private List<string> GetPagesListForType(PageType pageType)
    {
        switch (pageType)
        {
            case PageType.Overview:
                return overviewPages;
            case PageType.EnemyData:
                return enemyPages;
            case PageType.HintsPage:
                return hintsPages;
            default:
                return new List<string>();
        }
    }

    public void ShowOverview()
    {
        ChangePageType(PageType.Overview);
    }

    public void ShowHintsPage()
    {
        ChangePageType(PageType.HintsPage);
    }

    public void ShowEnemyDatabase()
    {
        ChangePageType(PageType.EnemyData);
    }

    private void ChangePageType(PageType newPageType)
    {
        currentPageType = newPageType;
        pageIndexes[currentPageType] = 0;
        ShowPages(currentPageType);
    }
}
