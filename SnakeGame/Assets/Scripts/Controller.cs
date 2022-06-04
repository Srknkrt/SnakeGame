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
        info.text = "Hareket etmek i�in W,A,S,D veya ok y�n tu�lar�n� kullan�n.";
    }

    public void OyunHareketli()
    {
        info.text = "Y�lan� elmaya g�t�r.";
    }

    public void OyunBitti()
    {
        info.text = "Oyunu Kaybettin\nTekrar ba�lamak i�in 'enter'a bas�n.";
    }
}
