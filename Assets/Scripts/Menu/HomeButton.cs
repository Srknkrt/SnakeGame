using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeButton : MonoBehaviour
{
    public Image howToPlayPanel;

    public void HomeBtn()
    {
        howToPlayPanel.gameObject.SetActive(false);
    }
}
