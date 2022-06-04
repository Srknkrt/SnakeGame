using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skore;
    [SerializeField] private TextMeshProUGUI info;

    public void SkorGuncelle(int skor)
    {
        skore.text = "SKOR: " + skor.ToString();
    }

    public void OyunDuruk()
    {
        info.text = "Hareket etmek için W,A,S,D veya ok yön tuþlarýný kullanýn.";
    }

    public void OyunHareketli()
    {
        info.text = "Yýlaný elmaya götür.";
    }

    public void OyunBitti()
    {
        info.text = "Oyunu Kaybettin\nTekrar baþlamak için 'enter'a basýn.";
    }
}
