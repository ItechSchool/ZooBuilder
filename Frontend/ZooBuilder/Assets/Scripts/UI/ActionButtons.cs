using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtons : MonoBehaviour
{
    private static ActionButtons _instance;
    [SerializeField] private GameObject container;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        Show();
    }

    public static void Show()
    {
        _instance.container.SetActive(true);
    }

    public static void Hide()
    {
        _instance.container.SetActive(false); 
    }
}