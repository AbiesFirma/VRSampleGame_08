﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private VRInteractiveItem m_InteractiveItem;
    public bool isGazeOver;

    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
    }

    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
    }

    // 視線が当たったとき
    private void HandleOver()
    {
        isGazeOver = true;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    // 視線が外れたとき
    private void HandleOut()
    {
        isGazeOver = false;
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
}
