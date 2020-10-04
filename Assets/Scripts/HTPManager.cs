using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HTPManager : MonoBehaviour
{
    private int Page = 1;
    private int pageCount = 8;
    public TextMeshProUGUI pageText;
    public Transform Pages;

    private void Start()
    {
        pageText.text = Page.ToString() + "/" + pageCount.ToString();

        for (int i = 0; i < Pages.childCount; i++)
        {
            Pages.GetChild(i).gameObject.SetActive(false);
        }

        Pages.GetChild(Page - 1).gameObject.SetActive(true);
    }

    public void changePage(int p)
    {
        Page += p;

        Page = Page % pageCount;

        if (Page <= 0)
        {
            Page = pageCount + Page;
        }

        pageText.text = Page.ToString() + "/" + pageCount.ToString();

        for (int i = 0; i < Pages.childCount; i++)
        {
            Pages.GetChild(i).gameObject.SetActive(false);
        }

        Pages.GetChild(Page - 1).gameObject.SetActive(true);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
