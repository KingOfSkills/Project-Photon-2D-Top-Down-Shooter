using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;

    public bool isOpen;

    private void Start()
    {
        if (!isOpen)
        {
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        isOpen = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        isOpen = false;
    }
}
