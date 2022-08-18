using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayButton : MonoBehaviour
{
    public Image howToPlayPanel;

    public void HowToPlayBtn()
    {
        howToPlayPanel.gameObject.SetActive(true);
    }
}
