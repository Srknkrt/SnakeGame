using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resulation : MonoBehaviour
{
    private void Start()
    {
        // The resolution for the phone screen has been adjusted.
        Screen.SetResolution(Screen.currentResolution.width / 3, Screen.currentResolution.height / 3, true);
    }
}
