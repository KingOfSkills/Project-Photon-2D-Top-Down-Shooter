using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private Menu[] menuList;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowMenu(string menuName)
    {
        for (int i = 0; i < menuList.Length; i++)
        {
            if (menuName == menuList[i].menuName)
            {
                menuList[i].Show();
            }
            else if (menuList[i].isOpen)
            {
                HideMenu(menuList[i]);
            }
        }
    }

    public void ShowMenu(Menu menu)
    {
        for (int i = 0; i < menuList.Length; i++)
        {
            if (menuList[i].isOpen)
            {
                menuList[i].Hide();
            }
        }
        menu.Show();
    }

    public void HideMenu(Menu menu)
    {
        menu.Hide();
    }
}
