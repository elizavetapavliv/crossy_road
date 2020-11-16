﻿using UnityEngine;
using UnityEngine.UI;

public class CoinsController : MonoBehaviour
{
    [SerializeField]
    private Text coinsText;

    private void Start()
    {
        int coinsCount = PlayerPrefs.GetInt("coins");
        coinsText.text = coinsCount.ToString();
    }
}