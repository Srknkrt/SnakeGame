using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatManager : MonoBehaviour
{
    [SerializeField] private AudioSource eatSound;

    public void YemekYendi()
    {
        eatSound.Play();
    }
}
